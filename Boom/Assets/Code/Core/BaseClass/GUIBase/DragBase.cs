using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragBase : ItemBase, IPointerDownHandler, IPointerUpHandler, 
    IDragHandler,IPointerExitHandler,IPointerMoveHandler
{
    //其他属性
    internal GameObject _dragIns; //当前拖拽物
    internal Vector3 originalPosition; //拖拽物原始位置
    internal SlotBase _curSlot; //当前拖拽物所在的Slot

    internal Transform originalParent;//拖拽中的物品原始父层级
    internal Transform dragObjParent; //拖拽中的物品所在的父层级
    //
    RectTransform rectTransform;
    //ToolTips相关
    internal GameObject TooltipsGO;
    internal GameObject RightClickMenuGO;
    internal PointerEventData _eventData;
    internal bool IsToolTipsDisplay;
    
    internal virtual void Start()
    {
        _dragIns = gameObject;
        rectTransform = GetComponent<RectTransform>();
        dragObjParent = UIManager.Instance.DragObjRoot.transform;
        IsToolTipsDisplay = true;
    }
    
    public override void SyncData() {}//实现一下继承的抽象类

    //鼠标按下时
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        UIManager.Instance.IsLockedClick = true;
        //改变父层级
        originalParent = _dragIns.transform.parent;
        _dragIns.transform.SetParent(dragObjParent);

        _eventData = eventData;
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            originalPosition = _dragIns.transform.position;
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            RightClick();
        }
        DestroyTooltips();
    }

    //鼠标松开时
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        UIManager.Instance.IsLockedClick = false;
        
        if (eventData.button == PointerEventData.InputButton.Right)
            return;
        DestroyTooltips();
        IsToolTipsDisplay = true;
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
                    _curSlot = curSlotSC;
                    VOnDrop();
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
    internal virtual void VOnDrop(){}
    
    internal virtual void NonFindSlot()
    {
        // 如果没有找到槽位，那么物品回到原始位置
        _dragIns.transform.position = originalPosition;
        _curSlot.MainID = ID;
        _dragIns.transform.SetParent(originalParent,true);//还原父层级
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
        _dragIns.GetComponent<RectTransform>().position = worldPos;
        //拖动不显示Tooltips说明菜单
        DestroyTooltips();
        VOnDrag();
    }
    
    internal virtual void VOnDrag(){}

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        DestroyTooltips();
    }

    public virtual void OnPointerMove(PointerEventData eventData)
    {
        if (!IsToolTipsDisplay) return;
        
        DisplayTooltips(eventData);
    }
    
    //捕捉鼠标位置转化为世界空间位置
    Vector3 GetWPosByMouse(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, 
            eventData.position, eventData.pressEventCamera, out Vector3 worldPoint);
        return worldPoint;
    }
    
    #region Tooltips说明窗口
    internal void DisplayTooltips(PointerEventData eventData)
    {
        // 加载Tooltips
        if (TooltipsGO == null)
        {
            TooltipsGO = ResManager.instance.CreatInstance(PathConfig.TooltipAsset);
            TooltipsGO.transform.SetParent(
                UIManager.Instance.TooltipsRoot.transform,false);
            SetTooltipInfo();
        }
        // 把Tooltips的位置设置为鼠标位置
        TooltipsGO.transform.position = GetWPosByMouse(eventData);
    }
    
    internal virtual void SetTooltipInfo()
    {
    }
    
    internal void DestroyTooltips()
    {
        for (int i = UIManager.Instance.TooltipsRoot.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(UIManager.Instance.TooltipsRoot
                .transform.GetChild(i).gameObject);
        }
    }
    #endregion

    #region 右击菜单
    internal void DisplayRightClickMenu(PointerEventData eventData)
    {
        if (RightClickMenuGO == null)
        {
            RightClickMenuGO = ResManager.instance.CreatInstance(PathConfig.RightClickMenu);
            RightClickMenuGO.transform.SetParent(
                UIManager.Instance.RightClickMenuRoot.transform,false);
        }
        if (RightClickMenuGO.TryGetComponent(out RightClickMenu curSc))
        {
            curSc.CurIns = eventData.pointerEnter?.transform.parent?.gameObject;
            RightClickMenuGO.transform.position = GetWPosByMouse(eventData);
        }
    }
    
    internal void DestroyRightClickMenu()
    {
        for (int i = UIManager.Instance.RightClickMenuRoot.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(UIManager.Instance.RightClickMenuRoot
                .transform.GetChild(i).gameObject);
        }
    }
    #endregion
}
