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
    public ItemDataBase Data { get; private set; }

    float lastClickTime;
    const float doubleClickThreshold = 0.3f;
    
    bool isHovered = false; //悬停标记，为了解决拖拽结束后是否显示Tooltips的问题
    
    void Awake() => behaviour = GetComponent<IItemInteractionBehaviour>();

    void Start() => DragManager.Instance.OnMgrEndDrag += ShowTooltips;
    void OnDestroy() => DragManager.Instance.OnMgrEndDrag -= ShowTooltips;

    #region UI交互逻辑
    // 绑定数据（泛型适配）
    public void BindData(ItemDataBase data) => Data = data;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        //商店宝石的UI高亮
        if (gameObject.TryGetComponent<IHighlightableUI>(out var highlightable))
            highlightable.SetHighlight(true);
        
        ShowTooltips();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
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
        if (!isHovered) return;
        if (Data.CurSlotController == null) return;
        
        if (Data is GemData gemData)
        {
            if (gemData.CurSlotController is GemSlotController gemSlot &&
                gemSlot.ParentBullet != null)
            {
                TooltipsManager.Instance.Show(
                    gemData.BuildTooltipInBulletContext(gemSlot.ParentBullet),
                    gemData.CurSlotController.TooltipOffset);
                return;
            }
        }
        
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