using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MiracleOddityView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerMoveHandler
{
    public Image Icon;
    [Header("偏移相关")]
    public Vector2 TooltipOffset;

    private MiracleOddityData _data;
    
    public void BindingData(MiracleOddityData data)
    {
        _data = data;
        RefreshUI();
    }

    void RefreshUI() => Icon.sprite = ResManager.instance.GetMiracleOddityIcon(_data.ID);
    
    public void OnPointerEnter(PointerEventData eventData)
        => TooltipsManager.Instance.Show( _data.BuildTooltip(), transform.position);
    
    public void OnPointerExit(PointerEventData eventData)
        => TooltipsManager.Instance.Hide();

    public void OnPointerMove(PointerEventData eventData)
        => TooltipsManager.Instance.UpdatePosition(TooltipOffset);

    public void OnDestroy() => _data.ClearData();
}