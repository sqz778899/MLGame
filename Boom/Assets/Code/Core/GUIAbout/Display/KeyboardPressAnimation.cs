using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

//新手教程在用的那种按下键盘的动画
public class KeyboardPressAnimation : MonoBehaviour
{
    public Sprite normalSprite;  // 正常图片
    public Sprite pressedSprite; // 按下图片
    public TextMeshProUGUI text; 
    Vector2 initialTextPos;
    Vector2 pressedTextOffset;
    Image keyImage;
    Sequence pressAnimation;

    void Awake()
    {
        keyImage = GetComponent<Image>();
        initialTextPos = text.GetComponent<RectTransform>().anchoredPosition;
        pressedTextOffset = new Vector2(0, -12f);
    }

    void Start()
    {
        float dur = 0.15f;
        // 无限循环动画，模拟按下释放效果
        pressAnimation = DOTween.Sequence()
            .AppendCallback(() => keyImage.sprite = pressedSprite)    // 按下图片
            .Append(keyImage.rectTransform.DOScale(0.98f, dur))      // 缩小一点点
            .Join(text.rectTransform.DOScale(0.97f, dur))
            .Join(text.rectTransform.DOAnchorPos(initialTextPos + pressedTextOffset, dur))
            .AppendInterval(0.3f)
            .AppendCallback(() => keyImage.sprite = normalSprite)     // 还原图片
            .Append(keyImage.rectTransform.DOScale(1f, dur))        // 还原大小
            .Join(text.rectTransform.DOScale(1f, dur))
            .Join(text.rectTransform.DOAnchorPos(initialTextPos, dur))
            .AppendInterval(0.3f)
            .SetLoops(-1);
    }

    void OnDestroy()
    {
        pressAnimation.Kill();
        keyImage.rectTransform.localScale = Vector3.one;
        keyImage.sprite = normalSprite;
    }
}