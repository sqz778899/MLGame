using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointEnterMove:MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public float AniTime = 0.5f;
    public AnimationCurve Curve;
    public Transform TargetTrans;
    Vector3 OriginPos;

    void Start()
    {
        OriginPos = transform.position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOMove(TargetTrans.position, AniTime).SetEase(Curve);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOMove(OriginPos, AniTime).SetEase(Curve);
    }
}