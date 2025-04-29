using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInteractionHandler: MonoBehaviour, 
    IPointerEnterHandler, IPointerExitHandler,IPointerDownHandler,
    IPointerClickHandler, IBeginDragHandler, IEndDragHandler,
    IDragHandler,IPointerMoveHandler,IPointerUpHandler
{
    [Header("测试")]
    public Vector2 Offset = default;
    
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        //商店宝石的UI高亮
        if (gameObject.TryGetComponent<IHighlightableUI>(out var highlightable))
            highlightable.SetHighlight(true);
        
        ShowTooltips();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //商店宝石的UI高亮
        if (gameObject.TryGetComponent<IHighlightableUI>(out var highlightable))
            highlightable.SetHighlight(false);
        
        TooltipsManager.Instance.Hide();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //点击小优化的接口触发
        if (gameObject.TryGetComponent<IPressEffect>(out var pressEffect))
            pressEffect.OnPressDown();
        
        TooltipsManager.Instance.Hide();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //点击小优化的接口触发
        if (gameObject.TryGetComponent<IPressEffect>(out var pressEffect))
            pressEffect.OnPressUp();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            behaviour?.OnRightClick();
        }
        else
        {
            behaviour?.OnClick(); //主动调用 OnClick()！

            if (Time.time - lastClickTime < doubleClickThreshold)
                behaviour?.OnDoubleClick();

            lastClickTime = Time.time;
        }

        lastClickTime = Time.time;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!DragManager.Instance.CanDrag()) return;
        if (behaviour is { CanDrag: false }) return; //新增判断
        
        behaviour?.OnBeginDrag();
        TooltipsManager.Instance.Hide();
        TooltipsManager.Instance.Disable();
        DragManager.Instance.BeginDrag(gameObject, eventData);
    }

    public void OnDrag(PointerEventData eventData) => DragManager.Instance.OnDrag(eventData);
    public void OnEndDrag(PointerEventData eventData)
    {
        DragManager.Instance.EndDrag(eventData);
        TooltipsManager.Instance.Enable();
        behaviour?.OnEndDrag();
        if (!DisableTooltip)
            ShowTooltips();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (Data.CurSlotController == null) return;
        TooltipsManager.Instance.UpdatePosition(Data.CurSlotController.TooltipOffset);
    }

    #endregion

    public void ShowTooltips()
    {
        if (Data.CurSlotController == null) return;
        if (Data is ITooltipBuilder builder)
        {
            TooltipsManager.Instance.Show(builder.BuildTooltip(),
                Data.CurSlotController.TooltipOffset);
        }
    }
}

public interface IItemInteractionBehaviour
{
    void OnBeginDrag();
    void OnEndDrag();
    void OnDoubleClick();
    void OnRightClick();
    
    void OnClick(); // 用于 GemShopPreview 或特殊交互用例
    bool CanDrag { get; }
}