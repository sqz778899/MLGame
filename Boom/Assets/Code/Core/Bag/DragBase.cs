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
    bool IsToolTipsDisplay;
    
    internal virtual void Start()
    {
        _dragIns = gameObject;
        IsToolTipsDisplay = true;
    }

    //鼠标按下时
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        originalPosition = _dragIns.transform.position;
        DestroyTooltips();
    }

    //鼠标松开时
    public virtual void OnPointerUp(PointerEventData eventData)
    {
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

    public virtual void OnDrag(PointerEventData eventData)
    {
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
            TooltipsGO = Instantiate(ResManager.instance
                .GetAssetCache<GameObject>(PathConfig.TooltipAsset));
            CommonTooltip curTip = TooltipsGO.GetComponentInChildren<CommonTooltip>();
            //curTip.SyncInfo(_bulletData.ID,ItemTypes.Bullet);
            TooltipsGO.transform.SetParent(UIManager.Instance.TooltipsRoot.transform);
            TooltipsGO.transform.localScale = Vector3.one;
        }
        // 把Tooltips的位置设置为鼠标位置
        TooltipsGO.transform.position = GetWPosByMouse(eventData);
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
}
