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

    public void Init()
    {
        Instance = this;
        if (tooltipGO != null)
            tooltipGO.SetActive(false);
    }

    /// <summary>
    /// 显示 Tooltips
    /// </summary>
    public void Show(ToolTipsInfo info, Vector3 worldPosition)
    {
        if (!isEnabled) return;
        
        if (tooltipGO == null || tooltipSC == null) return;

        tooltipGO.SetActive(true);
        tooltipSC.SetInfo(info);
        tooltipGO.transform.position = worldPosition;
    }
    
    public void UpdatePosition(Vector3 screenPos) =>tooltipGO.transform.position = screenPos;

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