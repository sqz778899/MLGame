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
    public Vector2 TooltipOffset = new Vector3(0.7f, -0.6f); // 可在Inspector里调
    RectTransform rectTransform;
    Canvas myCanvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        myCanvas = GetComponentInParent<Canvas>();
    }

    public void BindingData(BuffData _data)
    {
        Data = _data;
        Icon.sprite = Data.Icon;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Data == null) return;
        ToolTipsInfo info = Data.BuildTooltip();
        /*ToolTipsInfo info = Data.BuildTooltip();
        Vector3 worldPos = UTools.GetWPosByMouse(transform as RectTransform);
        worldPos += TooltipOffset;
        TooltipsManager.Instance.Show(info, worldPos,TooltipOffset);*/
        
        Vector3 screenPos = Input.mousePosition; // 直接拿鼠标屏幕坐标
        TooltipsManager.Instance.Show(info, screenPos, TooltipOffset);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipsManager.Instance.Hide();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        Vector2 screenPos = Input.mousePosition;
        TooltipsManager.Instance.UpdatePosition(screenPos + TooltipOffset);
    }
}