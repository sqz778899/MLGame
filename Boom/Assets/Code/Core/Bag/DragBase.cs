using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragBase : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, 
    IDragHandler,IPointerExitHandler,IPointerMoveHandler
{
    internal GameObject _dragIns; //当前拖拽物
    internal Vector3 originalPosition; //拖拽物原始位置
    internal SlotBase _curSlot; //当前拖拽物所在的Slot
    //ToolTips相关
    internal GameObject TooltipsGO;
    internal GameObject RightClickMenuGO;
    internal PointerEventData _eventData;
    bool IsToolTipsDisplay;
    
    internal virtual void Start()
    {
        _dragIns = gameObject;
        IsToolTipsDisplay = true;
    }

    //鼠标按下时
    public virtual void OnPointerDown(PointerEventData eventData)
    {
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
            SlotBase curSlotSC = result.gameObject.GetComponent<SlotBase>();
            if (curSlotSC!=null)
            {
                _curSlot = curSlotSC;
                DropSlot();
                NonHappen = false;
                break;
            }
        }
        // 如果这个位置下没有Slot，我们就将拖拽物恢复到原来的位置
        if (NonHappen)
            NonFindSlot();
    }
    
    //拖拽物如果找的Slot,则执行的逻辑
    internal virtual void DropSlot()
    {
    }
    
    internal virtual void NonFindSlot()
    {
    }

    //右击
    internal virtual void RightClick()
    {
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            return;
        // 在拖动时，我们把子弹位置设置为鼠标位置
        Vector3 worldPos = GetWPosByMouse(eventData);
        _dragIns.GetComponent<RectTransform>().position = worldPos;
        //拖动不显示Tooltips说明菜单
        DestroyTooltips();
        //IsToolTipsDisplay = false;
    }

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
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        Vector3 worldPoint;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, 
            eventData.position, eventData.pressEventCamera, out worldPoint);
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
        }
        SetTooltipInfo();
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
        RightClickMenu curSc = RightClickMenuGO.GetComponent<RightClickMenu>();
        curSc.CurIns = eventData.pointerEnter;
        RightClickMenuGO.transform.position = GetWPosByMouse(eventData);
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
