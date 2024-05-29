using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Reward : MonoBehaviour
{
    public AnimationCurve Curve;
    CanvasGroup canvasGroup;
    public GameObject Ctrl;
    public float duration = 2f;
    void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        Vector3[] path = new Vector3[]
        {
            transform.position,
            Ctrl.transform.position,
            UIManager.Instance.TitleGold.transform.position
        };

        rectTransform.DOPath(path, duration, PathType.CatmullRom).SetEase(Curve);
        rectTransform.DOScale(rectTransform.localScale * 0.7f, duration);
        rectTransform.DORotate(new Vector3(180, 180, 0), duration,
                RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental).SetEase(Curve);
        
        float maintainDuration = duration * (2f/3f);
        float fadeDuration = duration - maintainDuration;
        canvasGroup.DOFade(0f, fadeDuration).SetDelay(maintainDuration + 0.01f).OnComplete(() => Destroy(gameObject));
    }
}
