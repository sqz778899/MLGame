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
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggedObject != null)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(dragRoot, eventData.position, eventData.enterEventCamera, out var worldPoint);
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
                if (Data == null) continue;
                
                SlotController targetCtrl = targetView.Controller;
                if (!targetCtrl.CanAccept(Data)) continue;//先判断是否合法
                
                //如果槽位已满，且是可交换的
                if (!targetCtrl.IsEmpty && targetCtrl.CurData != Data)
                {
                    SlotManager.Swap(targetCtrl, Data.CurSlotController as SlotController);
                   
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
        }

        draggedObject = null;
        originalParent = null;
    }
}