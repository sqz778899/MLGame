using DG.Tweening;
using UnityEngine;

public class BagIconAnimator : MonoBehaviour
{
    private bool isAnimating = false; // 锁

    public void PlayHeartbeatBump()
    {
        if (isAnimating) return; // 正在播放，忽略后续触发

        isAnimating = true;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(1.15f, 0.06f).SetEase(Ease.OutCubic))
            .Append(transform.DOScale(0.7f, 0.03f).SetEase(Ease.InCubic))
            .Append(transform.DOScale(1.15f, 0.06f).SetEase(Ease.OutCubic))
            .Append(transform.DOScale(0.7f, 0.05f).SetEase(Ease.InCubic))
            .Append(transform.DOScale(1.15f, 0.06f).SetEase(Ease.InCubic))
            .Append(transform.DOScale(1f, 0.03f).SetEase(Ease.InCubic))
            .OnComplete(() =>
            {
                // 动画播放完才解锁
                isAnimating = false;
            })
            .SetUpdate(true);
    }
}
