using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIWin : MonoBehaviour
{
    Vector3[] originalPositions; // 存储初始位置
    [Header("UI Text 引用")]
    public CanvasGroup baseScoreGroup;
    public CanvasGroup overflowScoreGroup;
    public CanvasGroup perfectScoreGroup;
    public CanvasGroup totalScoreGroup;

    public TextMeshProUGUI baseScoreText;
    public TextMeshProUGUI overflowScoreText;
    public TextMeshProUGUI perfectScoreText;
    public TextMeshProUGUI totalScoreText;
    public Button btnContinue;
    
    [Header("动画设置")]
    public float fadeDuration = 0.5f;    // 淡入动画时间
    public float intervalDuration = 0.5f; // 项目之间间隔时间
    
    [Header("特效与震动")]
    public float shakeDuration = 0.5f;// 震动时间
    public Vector3 shakeIntensity = new Vector3(100,100,0);// 震动强度
    public int shakeFrequency = 100;  // 震动频率
    public float shakeRadom = 88f;  // 震动随机性

    public void Win(Award CurAward)
    {
        //1)计算分数
        AllScoreStruct allScore = ScoreCalculator.CalculateScore(CurAward.BaseScore);
        //2)同步到数据层
        PlayerManager.Instance._PlayerData.ModifyScore(allScore.TotalScore);
        //3)显示UI
        ResetUI();
        ShowScores(allScore.BaseScore, allScore.OverflowBonusScore, allScore.PerfectBonusScore);
    }

    #region UI动效相关
    void Awake() {
        // 记录初始位置
        originalPositions = new Vector3[] {
            baseScoreGroup.transform.localPosition,
            overflowScoreGroup.transform.localPosition,
            perfectScoreGroup.transform.localPosition,
            totalScoreGroup.transform.localPosition
        };
    }
    
    void ResetUI()
    {
        baseScoreGroup.alpha = 0;
        overflowScoreGroup.alpha = 0;
        perfectScoreGroup.alpha = 0;
        totalScoreGroup.alpha = 0;
        
        // 恢复初始位置
        baseScoreGroup.transform.localPosition = originalPositions[0];
        overflowScoreGroup.transform.localPosition = originalPositions[1];
        perfectScoreGroup.transform.localPosition = originalPositions[2];
        totalScoreGroup.transform.localPosition = originalPositions[3];
        
        btnContinue.gameObject.SetActive(false);
    }
    
    void ShowScores(int baseScore, int overflowScore, int perfectScore)
    {
        int total = baseScore + overflowScore + perfectScore;

        baseScoreText.text = $"+ {baseScore}";
        overflowScoreText.text = $"+ {overflowScore}";
        perfectScoreText.text = $"+ {perfectScore}";
        totalScoreText.text = total.ToString();

        baseScoreGroup.transform.localPosition += Vector3.up * 30;
        overflowScoreGroup.transform.localPosition += Vector3.up * 30;
        perfectScoreGroup.transform.localPosition += Vector3.up * 30;
        totalScoreGroup.transform.localPosition += Vector3.up * 30;
        
        // 动画序列
        Sequence seq = DOTween.Sequence();
        // 基础分淡入
        seq.Append(baseScoreGroup.DOFade(1, fadeDuration));
        seq.Join(DOTween.To(() => 0, x => baseScoreText.text = $"+ {x}", baseScore, fadeDuration).SetEase(Ease.OutQuad));
        seq.Join(baseScoreGroup.transform.DOLocalMoveY(-30, fadeDuration).From(true));
        // 等待
        seq.AppendInterval(intervalDuration);
        // 溢出伤害淡入
        seq.Append(overflowScoreGroup.DOFade(1, fadeDuration));
        seq.Join(DOTween.To(() => 0, x => overflowScoreText.text = $"+ {x}", overflowScore, fadeDuration).SetEase(Ease.OutQuad));
        seq.Join(overflowScoreGroup.transform.DOLocalMoveY(-30, fadeDuration).From(true));
        seq.AppendInterval(intervalDuration);
        // 完美伤害淡入
        seq.Append(perfectScoreGroup.DOFade(1, fadeDuration));
        seq.Join(DOTween.To(() => 0, x => perfectScoreText.text = $"+ {x}", perfectScore, fadeDuration).SetEase(Ease.OutQuad));
        seq.Join(perfectScoreGroup.transform.DOLocalMoveY(-30, fadeDuration).From(true));
        seq.AppendInterval(intervalDuration);
        // 总得分淡入（可进一步强化）
        seq.Append(totalScoreGroup.DOFade(1, fadeDuration));
        seq.Join(DOTween.To(() => 0, x => totalScoreText.text = x.ToString(), total, fadeDuration).SetEase(Ease.OutQuad));
        seq.Join(totalScoreGroup.transform.DOLocalMoveY(-30, fadeDuration).From(true));
        // 总分登场后回调（比如播放特效）
        seq.AppendCallback(() => {
            btnContinue.gameObject.SetActive(true);
            btnContinue.transform.localScale = Vector3.zero;
            btnContinue.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
            Shake();
        });
    }
    
    void Shake()
    {
        Vector3 originalPosition = totalScoreText.rectTransform.anchoredPosition;
        totalScoreText.rectTransform.DOShakeAnchorPos(shakeDuration, shakeIntensity, shakeFrequency, shakeRadom)
            .OnComplete(() => totalScoreText.rectTransform.anchoredPosition = originalPosition);
    }
    #endregion
}
