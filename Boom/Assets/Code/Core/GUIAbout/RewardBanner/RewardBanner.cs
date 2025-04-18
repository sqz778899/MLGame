using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class RewardBanner : MonoBehaviour
{
    public RectTransform rectTransform;
    public CanvasGroup canvasGroup;
    public RectTransform iconRect;
    public Image iconImage;
    public TextMeshProUGUI countText;
    
    [Header("UI参数")]
    public float BannerLenth = 1000f;
    public Image rarityBorder;

    [Header("动画参数")]
    public float SlideInDuration = 0.4f;
    public float StayDuration = 1.5f;
    public float SlideOutDuration = 0.5f;
    public float IconBounceScale = 1.2f;
    public float CountScale = 1.3f;
    
    [Header("稀有度颜色（HDR）")]
    [ColorUsage(true, true)]public Color rareColor = new Color(0.3f, 0.6f, 1.5f);      // 蓝
    [ColorUsage(true, true)]public Color epicColor = new Color(1.2f, 0.4f, 1.5f);      // 紫
    [ColorUsage(true, true)]public Color legendaryColor = new Color(2.2f, 1.0f, 0.1f); // 橙
    
    static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    public void Init(Sprite icon, int count, DropedRarity rarity = DropedRarity.Common)
    {
        iconImage.sprite = icon;
        countText.text = "0";
        
        ApplyRarityColor(rarity);
        
        StartCoroutine(PlaySequence(count));
    }
    
    void ApplyRarityColor(DropedRarity rarity)
    {
        Color color = Color.black; // 默认无光
        switch (rarity)
        {
            case DropedRarity.Common:
                rarityBorder.enabled = false;
                return;
            case DropedRarity.Rare:
                color = rareColor;
                break;
            case DropedRarity.Epic:
                color = epicColor;
                break;
            case DropedRarity.Legendary:
                color = legendaryColor;
                break;
        }

        rarityBorder.enabled = true;

        // 取出当前属性
        Material insMat = new Material(rarityBorder.material);
        rarityBorder.material = insMat;
        insMat.SetColor(EmissionColor, color);
    }
    
    IEnumerator PlaySequence(int targetCount)
    {
        // 初始化位置
        rectTransform.anchoredPosition = new Vector2(-BannerLenth, rectTransform.anchoredPosition.y);
        iconRect.localScale = Vector3.one * 0.6f;
        countText.alpha = 0;

        // Step 1: 整体滑入
        yield return null;
        rectTransform.DOAnchorPosX(0f, SlideInDuration).SetEase(Ease.OutBack);

        // Step 2: Icon 弹跳动画
        Sequence iconSeq = DOTween.Sequence();
        iconSeq.Append(iconRect.DOScale(IconBounceScale, 0.2f));
        iconSeq.Append(iconRect.DOScale(1f, 0.2f));
        iconSeq.Play();

        // Step 3: 数量从 0 滚动到目标值
        DOTween.To(() => 0, x =>
        {
            countText.text = x.ToString();
        }, targetCount, 0.5f).SetEase(Ease.OutCubic);

        // 数字淡入 + 弹跳
        countText.DOFade(1f, 0.2f);
        countText.transform.localScale = Vector3.one * 0.8f;
        countText.transform.DOScale(CountScale, 0.2f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            countText.transform.DOScale(1f, 0.2f);
        });

        yield return new WaitForSeconds(StayDuration);

        // Step 4: 整体向上滑动渐隐
        Sequence exit = DOTween.Sequence();
        exit.Append(rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y + 100f, SlideOutDuration));
        exit.Join(canvasGroup.DOFade(0f, SlideOutDuration));
        exit.OnComplete(() => Destroy(gameObject));
    }
}

