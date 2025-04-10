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
    
    public bool DisableTooltip { get; set; } = false;
    
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        behaviour = GetComponent<IItemInteractionBehaviour>();
    }

    #region UI交互逻辑
    // 绑定数据（泛型适配）
    public void BindData(ItemDataBase data) => Data = data;

    public void OnPointerEnter(PointerEventData eventData) => ShowTooltips();
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
        TooltipsManager.Instance.Disable();
        DragManager.Instance.BeginDrag(gameObject, eventData);
    }

    public void OnDrag(PointerEventData eventData) => DragManager.Instance.OnDrag(eventData);
    public void OnEndDrag(PointerEventData eventData)
    {
        DragManager.Instance.EndDrag(eventData);
        TooltipsManager.Instance.Enable();
        if (!DisableTooltip)
            ShowTooltips();
    }
    
    public void OnPointerMove(PointerEventData eventData)
    {
        Vector3 pos = UTools.GetWPosByMouse(rectTransform);
        if (Data.CurSlotController != null)
            pos += Data.CurSlotController.TooltipOffset;
        TooltipsManager.Instance.UpdatePosition(pos);
    }
    #endregion

    public void ShowTooltips()
    {
        if (Data is ITooltipBuilder builder)
        {
            Vector3 pos = UTools.GetWPosByMouse(rectTransform);
            if (Data.CurSlotController != null)
                pos += Data.CurSlotController.TooltipOffset;
            TooltipsManager.Instance.Show(builder.BuildTooltip(), pos);
        }
    }
}

public interface IItemInteractionBehaviour
{
    void OnDoubleClick();
    void OnRightClick();
}