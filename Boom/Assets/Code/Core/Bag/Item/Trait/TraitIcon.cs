using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TraitIcon:MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerMoveHandler
{
    public TraitData Data;
    [Header("Icon组件")]
    public Image Icon;

    [Header("偏移设置")]
    public Vector3 TooltipOffset;

    public void BindingData(TraitData data)
    {
        Data = data;
        Icon.sprite = Data.Icon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Data == null) return;
        ToolTipsInfo info = Data.BuildTooltip();
        TooltipsManager.Instance.Show(info, TooltipOffset);
    }

    public void OnPointerExit(PointerEventData eventData)
        => TooltipsManager.Instance.Hide();

    public void OnPointerMove(PointerEventData eventData) 
        => TooltipsManager.Instance.UpdatePosition(TooltipOffset);
}