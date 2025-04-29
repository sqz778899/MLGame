using UnityEngine;
using UnityEngine.Serialization;

public class TooltipsManager:MonoBehaviour
{
    [Header("Tooltip资产")]
    [SerializeField] public GameObject tooltipGO;
    [SerializeField] public Tooltips tooltipSC;
    
    bool isEnabled = true;
    public void Disable() => isEnabled = false;
    public void Enable() => isEnabled = true;
    public static TooltipsManager Instance { get; private set; }
    
    private float DefaultOrthoSize;

    public void Init()
    {
        Instance = this;
        if (tooltipGO != null)
            tooltipGO.SetActive(false);
        DefaultOrthoSize = Camera.main.orthographicSize;
    }

    /// <summary>
    /// 显示 Tooltips
    /// </summary>
    public void Show(ToolTipsInfo info, Vector3 offset = default)
    {
        if (!isEnabled) return;
        
        if (tooltipGO == null || tooltipSC == null) return;

        tooltipGO.SetActive(true);
        tooltipSC.SetInfo(info);
        Vector3 finalWorldPos = ScreenToCanvasWorldPos(Input.mousePosition + offset);
        tooltipGO.transform.position = finalWorldPos;
    }
    
    private Vector3 ScreenToCanvasWorldPos(Vector3 screenPos)
    {
        RectTransform canvasRect = tooltipGO.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        Camera cam = canvasRect.GetComponentInParent<Canvas>().worldCamera; // 注意Canvas要绑定了Camera！

        RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRect, screenPos, cam, out Vector3 worldPoint);
        return worldPoint;
    }
    
    
    //public void UpdatePosition(Vector3 screenPos)=>tooltipGO.transform.position = screenPos;
    
    public void UpdatePosition(Vector3 Offfset = default)
        => tooltipGO.transform.position = ScreenToCanvasWorldPos(Input.mousePosition + Offfset);

    /// <summary>
    /// 隐藏 Tooltips
    /// </summary>
    public void Hide()
    {
        if (tooltipGO == null || tooltipSC == null) return;
        tooltipGO.SetActive(false);
    }
}

public interface ITooltipBuilder
{
    ToolTipsInfo BuildTooltip();
}