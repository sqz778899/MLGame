using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipsBase : ItemBase,IPointerMoveHandler,IPointerExitHandler
{
    [Header("Tooltips&&右击菜单相关")]
    public float DobuleClickTime = 6f;
    internal float lastClickTime = 0f;
    internal Vector3 ToolTipsOffset;
    Vector3 RightClickMenuOffset;
    bool IsOpenedTooltip = false; //防止反复调用SetTooltipInfo();
    public ToolTipsMenuState CurToolTipsMenuState;
    internal Tooltips CurTooltipsSC;
    RightClickMenu CurRightClickMenuSC;
    internal RectTransform rectTransform => GetComponent<RectTransform>();

    internal virtual void Start()
    {
        DobuleClickTime = 0.35f;
        CurToolTipsMenuState = ToolTipsMenuState.Normal;
        ToolTipsOffset = new Vector3(1.01f, -0.5f, 0);
        RightClickMenuOffset = new Vector3(0.75f, -0.35f, 0);
        CurTooltipsSC = UIManager.Instance.TooltipsGO.GetComponentInChildren<Tooltips>();
        CurRightClickMenuSC = UIManager.Instance.RightClickGO.GetComponentInChildren<RightClickMenu>();
    }
    
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        HideTooltips();
    }

    public virtual void OnPointerMove(PointerEventData eventData)
    {
        if (CurToolTipsMenuState == ToolTipsMenuState.Normal)
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
        if (!IsOpenedTooltip)
        {
            UIManager.Instance.TooltipsGO.SetActive(true);
            IsOpenedTooltip = true;
            SetTooltipInfo();
        }
        
        // 把Tooltips的位置设置为鼠标位置
        UIManager.Instance.TooltipsGO.transform.position = GetWPosByMouse(eventData) + ToolTipsOffset;
    }
    
    internal virtual void SetTooltipInfo(){}
    
    public void HideTooltips()
    {
        CurTooltipsSC?.ClearInfo();
        UIManager.Instance.TooltipsGO.SetActive(false);
        IsOpenedTooltip = false;
    }
    #endregion
    
    #region 右击菜单
    internal void DisplayRightClickMenu(PointerEventData eventData)
    {
        CurToolTipsMenuState = ToolTipsMenuState.RightClick;
        UIManager.Instance.RightClickGO.SetActive(true);
        CurRightClickMenuSC.CurIns = eventData.pointerEnter?.gameObject;
        CurRightClickMenuSC.CurToolTipsBase = this;
        UIManager.Instance.RightClickGO.transform.position = GetWPosByMouse(eventData) + RightClickMenuOffset;
    }
    #endregion
}
