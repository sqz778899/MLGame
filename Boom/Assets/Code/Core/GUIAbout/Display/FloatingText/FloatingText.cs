using TMPro;
using UnityEngine;
using DG.Tweening;

public class FloatingText : MonoBehaviour
{
    [Header("通用参数")]
    public float moveUpDistance = 1f;                   // 上升距离 
    public float duration = 1f;                         // 动画时间
    public Vector3 moveOffset = new Vector3(0, 1f, 0);  // 位置偏移
    public Ease ease = Ease.OutCubic;                   // 动画缓动方式

    [Header("字体组件（自动识别）")]
    public TextMeshPro worldText;
    public TextMeshProUGUI uiText;
    public Renderer textRenderer; // 用于设置 SortingLayer

    public void Animate(string content, Color color, float fontSize = 4f)
    {
        if (worldText != null)
        {
            worldText.text = content;
            worldText.color = color;
            worldText.fontSize = fontSize;
            Play(worldText.transform);
        }
        else if (uiText != null)
        {
            uiText.text = content;
            uiText.color = color;
            uiText.fontSize = fontSize;
            Play(uiText.transform);
        }
    }

    void Play(Transform target)
    {
        Vector3 endPos = target.position + moveOffset;
        target.DOMove(endPos, duration).SetEase(ease);

        CanvasGroup group = GetComponent<CanvasGroup>();
        if (group != null)
        {
            group.DOFade(0f, duration).SetEase(ease);
        }
        else
        {
            // fallback: fade text component
            if (worldText != null) worldText.DOFade(0f, duration).SetEase(ease);
            if (uiText != null) uiText.DOFade(0f, duration).SetEase(ease);
        }

        Destroy(gameObject, duration + 0.1f);
    }
}