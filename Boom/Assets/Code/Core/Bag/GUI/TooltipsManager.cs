using UnityEngine;
using UnityEngine.Serialization;

public class TooltipsManager:MonoBehaviour
{
    public static TooltipsManager Instance { get; private set; }

    [Header("Tooltip UI Root")]
    [SerializeField] public GameObject tooltipGO;
    [SerializeField] public Tooltips tooltipSC;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        if (tooltipGO != null)
            tooltipGO.SetActive(false);
    }

    /// <summary>
    /// 显示 Tooltips
    /// </summary>
    public void Show(TooltipsInfo info, Vector3 worldPosition)
    {
        if (tooltipGO == null || tooltipSC == null) return;

        tooltipGO.SetActive(true);
        tooltipSC.ClearInfo();
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

        tooltipSC.ClearInfo();
        tooltipGO.SetActive(false);
    }
}

public interface ITooltipBuilder
{
    TooltipsInfo BuildTooltip();
}