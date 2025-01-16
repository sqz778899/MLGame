using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class EffectManager : MonoBehaviour
{
    public void PlayAwardEffect(Vector3 startPos,Vector3 targetPos,int num = 20,Action onFinish = null)
    {
        Sequence seq = DOTween.Sequence();
        int val = Random.Range(-300, 300);
        for (int i = 0; i < num; i++)
        {
            GameObject awardIns = ResManager.instance.CreatInstance("PathConfig.AwardEffect");
            awardIns.transform.SetParent(transform,false);
            float randomTime = Random.Range(0.4f, 0.6f);
            awardIns.transform.position = startPos;
            seq.Insert(0,awardIns.transform.DOMove(
                startPos + Random.insideUnitSphere*200,randomTime).SetEase(Ease.OutSine));
            Vector3 cpos = new Vector3(startPos.x + val,startPos.y + val,startPos.z);
            seq.Insert(randomTime,awardIns.transform.DOBezier(startPos,cpos,targetPos,
                Random.Range(0.8f,1.2f), () =>
                {
                    Destroy(awardIns);
                    onFinish?.Invoke();
                }));
        }

        seq.SetUpdate(true);
        seq.AppendCallback(() =>
        {
            onFinish?.Invoke();
        });
    }
}
