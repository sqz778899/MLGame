using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public UnityEvent OnClick;
    [Header("依赖资产")]
    [SerializeField] Image highlightBG; // 背景图用于高亮
    public Vector3 scale = new Vector3(1.2f, 1.2f, 1.2f);
    void Start() =>highlightBG.enabled = false;
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(scale, 0.2f).SetEase(Ease.OutBack);
        highlightBG.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        highlightBG.enabled = false;
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

    public void OnPointerClick(PointerEventData eventData) =>  OnClick?.Invoke();
}
