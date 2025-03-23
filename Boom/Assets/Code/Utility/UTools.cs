using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class UTools
{
    // 获取UI空间坐标
    public static Vector2 GetUISpacePos(Vector3 envPos,RectTransform canvasRectTransform)
    {
        // 世界坐标 → 屏幕坐标
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(envPos);
        // 手算UI空间坐标
        Vector2 canvasRealSize = canvasRectTransform.rect.size;
        Vector2 viewportPos = new Vector2(screenPoint.x / Screen.width, screenPoint.y / Screen.height);
        Vector2 uiLocalPos = new Vector2(
            (viewportPos.x - canvasRectTransform.pivot.x) * canvasRealSize.x,
            (viewportPos.y - canvasRectTransform.pivot.y) * canvasRealSize.y);
        return uiLocalPos;
    }
}