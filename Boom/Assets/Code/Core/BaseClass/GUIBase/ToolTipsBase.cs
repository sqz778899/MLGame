using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipsBase : ItemBase,IPointerMoveHandler,IPointerExitHandler
{
    [Header("Tooltips&&右击菜单相关")]
    internal GameObject TooltipsGO;
    internal GameObject RightClickMenuGO;
    internal bool IsToolTipsDisplay;
    internal RectTransform rectTransform => GetComponent<RectTransform>();

    internal virtual void Start()
    {
        IsToolTipsDisplay = true;
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
    internal Vector3 GetWPosByMouse(PointerEventData eventData)
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
    
    internal virtual void SetTooltipInfo(){}
    
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

    public override void SyncData(){}
}
