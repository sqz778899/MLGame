using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInteractionHandler: MonoBehaviour, 
    IPointerEnterHandler, IPointerExitHandler,IPointerDownHandler,
    IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler,IPointerMoveHandler
{
    IItemInteractionBehaviour behaviour;
    RectTransform rectTransform;
    public ItemDataBase Data { get; private set; }

    float lastClickTime;
    const float doubleClickThreshold = 0.3f;
    
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        behaviour = GetComponent<IItemInteractionBehaviour>();
    }
    
    public void Remove()
    {
        var slot = Data.CurSlotController;
        if (slot != null)
        {
            slot.Unassign(); // 清除当前槽位
            // 移回背包或销毁等操作
            
            GM.Root.InventoryMgr._InventoryData.AddGemToBag((GemData)Data); // 示例
        }
    }

    #region UI交互逻辑
    // 绑定数据（泛型适配）
    public void BindData(ItemDataBase data) => Data = data;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Data is ITooltipBuilder builder)
        {
            Vector3 pos = GetWPosByMouse(eventData);
            if (Data.CurSlotController != null)
                pos += Data.CurSlotController.TooltipOffset;
            TooltipsManager.Instance.Show(builder.BuildTooltip(), pos);
        }
    }
    public void OnPointerExit(PointerEventData eventData) => TooltipsManager.Instance.Hide();
    
    public void OnPointerDown(PointerEventData eventData) => TooltipsManager.Instance.Hide();

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            behaviour?.OnRightClick();
        else if (Time.time - lastClickTime < doubleClickThreshold)
            behaviour?.OnDoubleClick();

        lastClickTime = Time.time;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!DragManager.Instance.CanDrag()) return;
        TooltipsManager.Instance.Hide();
        DragManager.Instance.BeginDrag(gameObject, eventData);
    }

    public void OnDrag(PointerEventData eventData) => DragManager.Instance.OnDrag(eventData);
    public void OnEndDrag(PointerEventData eventData) => DragManager.Instance.EndDrag(eventData);
    public void OnPointerMove(PointerEventData eventData) => 
        TooltipsManager.Instance.UpdatePosition(GetWPosByMouse(eventData) 
            + Data.CurSlotController?.TooltipOffset ?? Vector3.zero);
    
    Vector3 GetWPosByMouse(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, 
            eventData.position, eventData.pressEventCamera, out Vector3 worldPoint);
        return worldPoint;
    }
    #endregion
}

public interface IItemInteractionBehaviour
{
    void OnDoubleClick();
    void OnRightClick();
}