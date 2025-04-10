using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragManager : MonoBehaviour
{
    public static DragManager Instance;
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
        if (go.TryGetComponent<BulletNew>(out var bullet))
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
        EventSystem.current.RaycastAll(eventData, results);

        bool dropped = false;
        foreach (var result in results)
        {
            if (result.gameObject.TryGetComponent(out SlotView targetView))
            {
                ItemBase curItem = draggedObject.GetComponent<ItemBase>();
                ItemDataBase Data = null;
                if (curItem is GemNew gem)
                    Data = gem.Data;
                else if (curItem is BulletNew bullet)
                    Data = bullet.Data;
                if (Data == null) continue;
                
                ISlotController targetCtrl = targetView.Controller;
                if (!targetCtrl.CanAccept(Data)) continue;//先判断是否合法
                
                //如果槽位已满，且是可交换的
                if (!targetCtrl.IsEmpty && targetCtrl.CurData != Data)
                {
                    if (targetCtrl is SlotController gemCtrl)
                        SlotManager.Swap(gemCtrl, Data.CurSlotController as SlotController);
                    else
                        Debug.Log("子弹互换");
                    dropped = true;
                    break;
                }
                //正常放入空槽
                if (targetCtrl.IsEmpty)
                {
                    targetCtrl.Assign(Data, draggedObject);
                    dropped = true;
                    break;
                }
            }
        }

        if (!dropped)
        {
            draggedObject.transform.SetParent(originalParent);
            draggedObject.transform.position = originalPosition;
            //拖拽失败，通知 Spawner 回滚
            if (draggedObject.TryGetComponent(out BulletNew bulletNew))
                bulletNew.OnDragCanceled();
        }

        draggedObject = null;
        originalParent = null;
    }

    #region 手动模拟拖拽
    PointerEventData lastEventData; // 记录传入的 PointerEventData
    public void ForceDrag(GameObject go, PointerEventData eventData)
    {
        draggedObject = go;
        go.transform.SetParent(dragRoot, false);
        lastEventData = eventData;
        TooltipsManager.Instance.Hide();
        TooltipsManager.Instance.Disable();
    }

    void Update()
    {
        if (draggedObject != null && lastEventData != null)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                dragRoot, lastEventData.position, lastEventData.enterEventCamera, out var worldPoint);
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
    }
    #endregion
}