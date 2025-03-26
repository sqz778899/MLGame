using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class DragBase : ToolTipsBase, IPointerDownHandler, IPointerUpHandler, IDragHandler,IPointerClickHandler
{
    //其他属性
    [Header("拖拽相关")]
    internal Vector3 originalPosition; //拖拽物原始位置
    internal Transform originalParent;//拖拽中的物品原始父层级
    internal PointerEventData _eventData;
    
    //鼠标双击时
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Time.time - lastClickTime <= DobuleClickTime) { DoubleClick(); }
        lastClickTime = Time.time;
    }
    internal virtual void DoubleClick() {}//双击逻辑，派生类自己覆写去

    //鼠标按下时
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        UIManager.Instance.IsLockedClick = true;
        _eventData = eventData;
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            originalParent = gameObject.transform.parent;//记录原始父层级
            gameObject.transform.SetParent(UIManager.Instance.CommonUI.DragObjRoot.transform);//改变父层级
            originalPosition = gameObject.transform.position;
        }
        
        if (eventData.button == PointerEventData.InputButton.Right)
            RightClick();
        HideTooltips();
    }

    //鼠标松开时
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        UIManager.Instance.IsLockedClick = false;
        
        if (eventData.button == PointerEventData.InputButton.Right)
            return;
        
        HideTooltips();
        // 在释放鼠标按钮时，我们检查这个位置下是否有一个Slot
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        bool NonHappen = true; // 发生Slot drop down 逻辑
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.TryGetComponent(out SlotBase curSlotSC))
            {
                if (curSlotSC is BulletSlot) continue;
                
                if (curSlotSC.MainID == -1)
                {
                    OnDropEmptySlot(curSlotSC);
                    NonHappen = false;
                    break;
                }
                else
                {
                    OnDropFillSlot(curSlotSC);
                    NonHappen = false;
                    break;
                }
            }
        }
        // 如果这个位置下没有Slot，我们就将拖拽物恢复到原来的位置
        if (NonHappen)
            NonFindSlot();
    }
    
    //拖拽物如果找的Slot,则执行的逻辑
    public virtual void OnDropEmptySlot(SlotBase targetSlot){}
    
    internal virtual void OnDropFillSlot(SlotBase targetSlot){}
    
    internal virtual void NonFindSlot()
    {
        // 如果没有找到槽位，那么物品回到原始位置
        gameObject.transform.position = originalPosition;
        gameObject.transform.SetParent(originalParent,true);//还原父层级
    }

    //右击
    internal virtual void RightClick()
    {
        DisplayRightClickMenu(_eventData);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            return;
        _eventData = eventData;
        // 在拖动时，我们把子弹位置设置为鼠标位置
        Vector3 worldPos = GetWPosByMouse(eventData);
        gameObject.GetComponent<RectTransform>().position = worldPos;
        //拖动不显示Tooltips说明菜单
        HideTooltips(); 
        VOnDrag();
    }
    
    internal virtual void VOnDrag(){}
}
