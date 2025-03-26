using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class ConquerTheLevelGUI : MonoBehaviour
{
    [Header("UI引用")]
    public CanvasGroup panelGroup; // 整个结算面板CanvasGroup
    public CanvasGroup totalScoreGroup;
    public CanvasGroup scoreToDustGroup;
    public CanvasGroup coinToDustGroup;
    public CanvasGroup totalDustGroup;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreToDustText_Score;
    public TextMeshProUGUI scoreToDustText_Dust;
    public TextMeshProUGUI coinToDustText_Coin;
    public TextMeshProUGUI coinToDustText_Dust;
    public TextMeshProUGUI totalDustText;
    public Button returnTownButton;

    [Header("动画参数")]
    public float fadeInDuration = 0.5f;
    public float intervalBetweenSteps = 0.7f;
    
    [Header("特效与震动")]
    public float shakeDuration = 0.5f;// 震动时间
    public Vector3 shakeIntensity = new Vector3(100,100,0);// 震动强度
    public int shakeFrequency = 100;  // 震动频率
    public float shakeRadom = 88f;  // 震动随机性

    private void Start()
    {
        ResetUI();
        PlaySettlementAnimation();
    }

    public void ConquerTheLevel()
    {
        ResetUI();
        PlaySettlementAnimation();
    }

    #region UI动效相关
    void ResetUI()
    {
        panelGroup.alpha = 0;
        totalScoreGroup.alpha = 0;
        scoreToDustGroup.alpha = 0;
        coinToDustGroup.alpha = 0;
        totalDustGroup.alpha = 0;
        scoreText.text = "0";
        scoreToDustText_Score.text = "";
        scoreToDustText_Dust.text = "";
        coinToDustText_Coin.text = "";
        coinToDustText_Dust.text = "";
        totalDustText.text = "";
        returnTownButton.gameObject.SetActive(false);
        
        // 新增：重置所有动画相关UI的初始位置
        totalScoreGroup.transform.localPosition = Vector3.zero;
        scoreToDustGroup.transform.localPosition = Vector3.zero;
        coinToDustGroup.transform.localPosition = Vector3.zero;
        totalDustGroup.transform.localPosition = Vector3.zero;
    }

    public void PlaySettlementAnimation()
    {
        int unspentCoins = PlayerManager.Instance._PlayerData.Coins;
        int totalScore = PlayerManager.Instance._PlayerData.Score;
        int scoreDustAmount = ScoreCalculator.ScoreToDust(totalScore);
        int coinDustAmount = ScoreCalculator.CoinToDust(unspentCoins);
        int totalDustAmount = scoreDustAmount + coinDustAmount;

        Sequence seq = DOTween.Sequence();

        // 面板淡入
        seq.Append(panelGroup.DOFade(1, fadeInDuration));

        // 显示积分动画
        seq.AppendInterval(0.3f);
        seq.AppendCallback(() => PlayNumberAnimation(scoreText, 0, totalScore, "{0}"));
        seq.Join(totalScoreGroup.DOFade(1, fadeInDuration));
        seq.Join(totalScoreGroup.transform.DOLocalMoveY(-30, fadeInDuration).From(true));
        seq.AppendInterval(intervalBetweenSteps);

        // 显示积分兑换魔尘动画
        seq.AppendCallback(() =>
        {
            scoreToDustText_Score.text = $"{totalScore}";
            PlayNumberAnimation(scoreToDustText_Dust, 0, scoreDustAmount, "{0}");
        });
        seq.Join(scoreToDustGroup.DOFade(1, fadeInDuration));
        seq.Join(scoreToDustGroup.transform.DOLocalMoveY(-30, fadeInDuration).From(true));
        seq.AppendInterval(intervalBetweenSteps);

        // 显示金币兑换魔尘动画
        seq.AppendCallback(() =>
        {
            coinToDustText_Coin.text = $"{unspentCoins}";
            PlayNumberAnimation(coinToDustText_Dust, 0, coinDustAmount, "{0}");
        });
        seq.Join(coinToDustGroup.DOFade(1, fadeInDuration));
        seq.Join(coinToDustGroup.transform.DOLocalMoveY(-30, fadeInDuration).From(true));
        seq.AppendInterval(intervalBetweenSteps);

        // 显示总魔尘动画（特效突出）
        seq.AppendCallback(() =>
        {
            PlayNumberAnimation(totalDustText, 0, totalDustAmount, "{0}", true);
        });
        seq.Join(totalDustGroup.DOFade(1, fadeInDuration));
        seq.Join(totalDustGroup.transform.DOLocalMoveY(-30, fadeInDuration).From(true));
        seq.AppendInterval(intervalBetweenSteps);

        // 显示返回按钮
        seq.AppendCallback(() =>
        {
            returnTownButton.gameObject.SetActive(true);
            returnTownButton.transform.localScale = Vector3.zero;
            returnTownButton.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
        });
    }

    void PlayNumberAnimation(TextMeshProUGUI tmpText, int from, int to, string format, bool playSpecialEffect = false)
    {
        DOTween.To(() => from, x =>
        {
            tmpText.text = string.Format(format, x);
        }, to, 0.8f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            if (playSpecialEffect)
            {
                PlaySpecialEffect(tmpText.rectTransform);
            }
        });
    }

    void PlaySpecialEffect(RectTransform target)
    {
        // 播放屏幕抖动或特效粒子，增强兴奋感
        target.DOShakeAnchorPos(shakeDuration, shakeIntensity, shakeFrequency, shakeRadom);
    }
    #endregion
}
