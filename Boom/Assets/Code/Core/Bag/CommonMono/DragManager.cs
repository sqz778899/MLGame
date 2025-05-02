using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragManager : MonoBehaviour
{
    public static DragManager Instance;
    public event Action OnMgrEndDrag;
    public RectTransform dragRoot;
    GameObject draggedObject;
    Vector3 originalPosition;
    Transform originalParent;

    public void Init() => Instance = this;

    public bool CanDrag() => !EternalCavans.Instance.TutorialDragGemLock;

    public void BeginDrag(GameObject go, PointerEventData eventData)
    {
        draggedObject = go;
        originalParent = go.transform.parent;
        originalPosition = go.transform.position;
        go.transform.SetParent(dragRoot);
        
        //如果是 Bullet，切换为 EditA 模式
        if (go.TryGetComponent<Bullet>(out var bullet))
            bullet.SwitchMode(BulletInsMode.EditA);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggedObject != null)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(dragRoot, 
                eventData.position, eventData.enterEventCamera, out Vector3 worldPoint);
            draggedObject.transform.position = worldPoint;
        }
    }

    public void EndDrag(PointerEventData eventData)
    {
        if (draggedObject == null) return;

        List<RaycastResult> results = new List<RaycastResult>();
        eventData.position = Input.mousePosition;
        EventSystem.current.RaycastAll(eventData, results);
        bool dropped = false;
        foreach (var result in results)
        {
            if (result.gameObject.TryGetComponent(out SlotView targetView))
            {
                ItemBase curItem = draggedObject.GetComponent<ItemBase>();
                ItemDataBase Data = null;
                if (curItem is Gem gem)
                    Data = gem.Data;
                else if (curItem is Bullet bullet)
                    Data = bullet.Data;
                else if (curItem is Item item)
                    Data = item.Data;
                if (Data == null) continue;
                
                ISlotController targetCtrl = targetView.Controller;
                if (!targetCtrl.CanAccept(Data)) continue;//先判断是否合法
                
                //如果槽位已满，且是可交换的
                if (!targetCtrl.IsEmpty 
                    && targetCtrl.CurData != Data
                    && targetCtrl.SlotType != SlotType.SpawnnerSlot
                    && targetCtrl.SlotType != SlotType.SpawnnerSlotInner)
                {
                    SlotManager.Swap(targetCtrl.CurData, Data,draggedObject);
                    dropped = true;
                    break;
                }
                //正常放入空槽
                if (targetCtrl.IsEmpty
                    && targetCtrl.SlotType != SlotType.SpawnnerSlot
                    && targetCtrl.SlotType != SlotType.SpawnnerSlotInner)
                {
                    targetCtrl.Assign(Data, draggedObject);
                    dropped = true;
                    break;
                }
                //战场内放回Spawner
                if (targetCtrl.IsEmpty &&
                    targetCtrl.SlotType == SlotType.SpawnnerSlotInner)
                {
                    draggedObject.TryGetComponent(out Bullet bulletNew);
                    Data.CurSlotController.Unassign();
                    bulletNew.OnDragCanceled();
                    dropped = true;
                    break;
                }
            }
        }
        
        //未成功落槽的判断
        if (!dropped)
            NonDropped();
        
        draggedObject = null;
        originalParent = null;
        OnMgrEndDrag?.Invoke();
    }
    
    public void CancelDrag()
    {
        PointerEventData fakeEvent = new PointerEventData(EventSystem.current)
        {position = Input.mousePosition };
        EndDrag(fakeEvent);
        EndForceDrag();
    }

    //未成功落槽的判断
    void NonDropped()
    {
        //是子弹
        if (draggedObject.TryGetComponent(out Bullet bulletNew))
        {
            //拖拽失败，通知 Spawner 回滚
            if (bulletNew.CreateFlag == BulletCreateFlag.Spawner || 
                bulletNew.CreateFlag == BulletCreateFlag.SpawnerInner)
                bulletNew.OnDragCanceled();
            
            if (bulletNew.CreateFlag == BulletCreateFlag.Spawnered)
            {
                bulletNew.SwitchMode(BulletInsMode.EditB);
                draggedObject.transform.SetParent(originalParent);
                draggedObject.transform.position = originalPosition;
            }
            //战场内空拖回弹
            if (bulletNew.CreateFlag == BulletCreateFlag.None)
                bulletNew.Data.CurSlotController.Assign(bulletNew.Data,draggedObject);
        }
        else//是宝石
        {
            draggedObject.transform.SetParent(originalParent);
            draggedObject.transform.position = originalPosition;
        }
    }
    
    #region 手动模拟拖拽
    PointerEventData lastEventData; // 记录传入的 PointerEventData
    
    public void ForceDrag(GameObject go, PointerEventData eventData)
    {
        ForceDrag(go);
        lastEventData = eventData;
    }
    
    public void ForceDrag(GameObject go)
    {
        draggedObject = go;
        go.transform.SetParent(dragRoot, false);
        TooltipsManager.Instance.Hide();
        TooltipsManager.Instance.Disable();
        PointerEventData simulatedEvent = new PointerEventData(EventSystem.current)
            { position = Input.mousePosition };
        lastEventData = simulatedEvent;
    }

    void Update()
    {
        if (draggedObject != null)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                dragRoot, Input.mousePosition, Camera.main, out Vector3 worldPoint);
            draggedObject.transform.position = worldPoint;
            
            // 监听鼠标松开（左键）
            if (Input.GetMouseButtonUp(0))
            {
                EndDrag(lastEventData);       // 调用和正常拖拽一样的结束逻辑
                EndForceDrag();               // 清理模拟状态
            }
        }
    }

    public void EndForceDrag()
    {
        draggedObject = null;
        lastEventData = null;
        TooltipsManager.Instance.Enable();
        OnMgrEndDrag?.Invoke();
    }
    #endregion
}