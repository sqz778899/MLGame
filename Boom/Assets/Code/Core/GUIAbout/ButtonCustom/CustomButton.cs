using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    ,IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public UnityEvent OnClick;
    //一些额外的回调函数
    public event Action<CustomButton> OnPointerEnterAction;
    public event Action<CustomButton> OnPointerExitAction;
    
    [Header("依赖资产")]
    [SerializeField] Image highlightBG; // 背景图用于高亮
    public Vector3 scale = new Vector3(1.2f, 1.2f, 1.2f);
    void Start() => highlightBG.enabled = false;

    public void Reset()
    {
        transform.localScale = Vector3.one;
        highlightBG.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(scale, 0.2f).SetEase(Ease.OutBack);
        highlightBG.enabled = true;
        OnPointerEnterAction?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        highlightBG.enabled = false;
        OnPointerExitAction?.Invoke(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        highlightBG.enabled = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(scale, 0.1f).SetEase(Ease.OutBack);
        highlightBG.enabled = true;
    }

    public void OnPointerClick(PointerEventData eventData) => OnClick?.Invoke();
}
