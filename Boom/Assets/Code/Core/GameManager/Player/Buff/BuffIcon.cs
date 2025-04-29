using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuffIcon:MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerMoveHandler
{
    public BuffData Data;
    [Header("Icon")]
    public Image Icon;
    [Header("偏移相关")]
    public Vector2 TooltipOffset;

    public void BindingData(BuffData _data)
    {
        Data = _data;
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