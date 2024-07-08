using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using DG.Tweening;

public class RollBase:MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    public float AnimationTime = 0.5f;
    public AnimationCurve RollCurve;
    public RollBulletMatType CurType;
    public Color OrignalColor;
    public TextMeshProUGUI _rollCost;
    
    internal virtual void Update()
    {
        if (_rollCost != null)   
            _rollCost.text = MainRoleManager.Instance.ShopCost.ToString();
    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //如果是分数，不播放动画，如果是材料，有一个小动画
        if (CurType == RollBulletMatType.Score)
        {
            ShopUtility.SelOne(eventData.pointerClick);
            DestroyImmediate(gameObject);
        }
        else
            //飘过去
            StartCoroutine(SelOne(eventData.pointerClick));
     
    }

    IEnumerator SelOne(GameObject SelGO)
    {
        Vector3 targetPos = ShopUtility.GetTargetSlotPos();
       
        Tween moveTween = SelGO.transform.DOMove(targetPos,AnimationTime).SetEase(RollCurve);
        Tween scaleTween = SelGO.transform.DOScale(Vector3.zero, AnimationTime).SetEase(RollCurve);
        
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(moveTween);
        mySequence.Join(scaleTween);
        
        WaitForSeconds wait = new WaitForSeconds(AnimationTime);
        
        yield return wait;
        
        ShopUtility.SelOne(SelGO);
        DestroyImmediate(gameObject);
    }
}