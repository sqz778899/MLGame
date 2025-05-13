using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIPopAnimator : MonoBehaviour
{
    public UIShowStyle showStyle = UIShowStyle.PopScale;
    public UIHideStyle hideStyle = UIHideStyle.FadeOut;

    [Header("依赖组件")]
    public RectTransform target;
    public CanvasGroup canvasGroup;

    [Header("动画参数")]
    public float duration = 0.3f;
    public Ease easing = Ease.OutBack;

    [Header("延迟隐藏参数")]
    public bool useDelayedHide = true;
    public float hideDelay = 0.2f;
    private bool isHiding = false;//是否正在隐藏,是的话不重新调用show(否则会闪烁)
    Coroutine hideCoroutine;//延迟隐藏协程

    public void Start() => CloseInteractable();

    public void PlayShow(Vector3? fromWorldPos = null)
    {
        // 正在隐藏，说明还在显示中，先不执行
        if (isHiding&&hideCoroutine!=null)
        {
            StopCoroutine(hideCoroutine);
            isHiding = false;
            return;
        }
       
        if (hideCoroutine != null) StopCoroutine(hideCoroutine);
        if (target == null) target = GetComponent<RectTransform>();
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();

        OpenInteractable();

        switch (showStyle)
        {
            case UIShowStyle.PopScale:
                target.localScale = Vector3.zero;
                target.DOKill();
                target.DOScale(1f, duration).SetEase(easing);
                break;

            case UIShowStyle.Fade:
                canvasGroup.alpha = 0;
                canvasGroup.DOKill();
                canvasGroup.DOFade(1, duration);
                break;
        }
    }

    public void PlayHide(Action onComplete = null)
    {
        if (useDelayedHide)
        {
            if (hideCoroutine != null) StopCoroutine(hideCoroutine);
            hideCoroutine = StartCoroutine(DelayedHide(onComplete));
        }
        else
            ExecuteHide(onComplete);
    }

    IEnumerator DelayedHide(Action onComplete)
    {
        isHiding = true;//开始隐藏协程，加锁
        yield return new WaitForSeconds(hideDelay);
        ExecuteHide(onComplete);
        hideCoroutine = null;
    }

    void ExecuteHide(Action onComplete)
    {
        if (target == null) target = GetComponent<RectTransform>();
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();

        void Finish()
        {
            CloseInteractable();
            isHiding = false;
            onComplete?.Invoke();
        }
        
        switch (hideStyle)
        {
            case UIHideStyle.Shrink:
                target.DOKill();
                target.DOScale(0f, duration).SetEase(Ease.InBack).OnComplete(Finish);
                break;

            case UIHideStyle.FadeOut:
                canvasGroup.DOKill();
                canvasGroup.DOFade(0, duration).OnComplete(Finish);
                break;
        }
    }
    
    void OpenInteractable()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    
    void CloseInteractable()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}

public enum UIShowStyle { PopScale, Fade, SlideFromTop, None }
public enum UIHideStyle { Shrink, FadeOut, SlideToBottom, None }
