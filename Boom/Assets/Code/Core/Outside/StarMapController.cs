using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StarMapController : MonoBehaviour, IPointerDownHandler, IDragHandler, IScrollHandler
{
    public RectTransform StarMap;
    public RectTransform MaskRect; // 遮罩窗口
    public float ZoomSpeed = 0.1f;
    public float MinZoom = 0.5f;
    public float MaxZoom = 2f;

    private Vector2 lastMousePosition;
    private float currentZoom = 1f;

    public void OnPointerDown(PointerEventData eventData)
    {
        lastMousePosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 delta = eventData.position - lastMousePosition;
        StarMap.anchoredPosition += delta;
        lastMousePosition = eventData.position;
        ClampPosition();
    }

    public void OnScroll(PointerEventData eventData)
    {
        float scroll = eventData.scrollDelta.y * ZoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom + scroll, MinZoom, MaxZoom);
        StarMap.localScale = Vector3.one * currentZoom;
        ClampPosition();
    }

    private void ClampPosition()
    {
        float halfMaskWidth = MaskRect.rect.width / 2f;
        float halfMaskHeight = MaskRect.rect.height / 2f;
        float scaledWidth = StarMap.rect.width * StarMap.localScale.x / 2f;
        float scaledHeight = StarMap.rect.height * StarMap.localScale.y / 2f;

        float minX = -scaledWidth + halfMaskWidth;
        float maxX = scaledWidth - halfMaskWidth;
        float minY = -scaledHeight + halfMaskHeight;
        float maxY = scaledHeight - halfMaskHeight;

        float clampX = Mathf.Clamp(StarMap.anchoredPosition.x, minX, maxX);
        float clampY = Mathf.Clamp(StarMap.anchoredPosition.y, minY, maxY);

        StarMap.anchoredPosition = new Vector2(clampX, clampY);
    }
}
