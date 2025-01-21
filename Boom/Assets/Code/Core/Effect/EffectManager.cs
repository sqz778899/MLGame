using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class EffectManager : MonoBehaviour
{
    public GameObject Root;
    public GameObject StartPosIns;
    public GameObject TargetPosIns;

    [Header("金币诞生动画时长")] public Vector2 spawntimeRange;
    [Header("金币诞生数量")] public int num;
    [Header("金币诞生球半径")]  public float radius;
    [Header("金币飞行贝塞尔控制点偏移")] public Vector2 flyRangeOffset; 
    [Header("金币飞行动画时长")] public Vector2 flyTimeRange; 
    
    public void CreatEffect(string effectName,Vector3 startPos)
    {
        string insPath = "";
        switch (effectName)
        {
            case "CoinsPile":
                insPath = PathConfig.AwardCoin;
                break;
        }

        PlayAwardEffect(startPos, insPath);
    }
    
    public void PlayAwardEffect(Vector3 startPos,string resPath,int num = 20,Action onFinish = null)
    {
        Sequence seq = DOTween.Sequence();
        float val = Random.Range(flyRangeOffset.x,flyRangeOffset.y);
        for (int i = 0; i < num; i++)
        {
            GameObject awardIns = ResManager.instance.CreatInstance(resPath);
            awardIns.transform.SetParent(Root.transform,false);
            float randomTime = Random.Range(spawntimeRange.x, spawntimeRange.y);
            awardIns.transform.position = startPos;
            seq.Insert(0,awardIns.transform.DOMove(
                startPos + Random.insideUnitSphere * radius,randomTime).SetEase(Ease.OutSine));
            Vector3 cpos = new Vector3(awardIns.transform.position.x + val,
                awardIns.transform.position.y + val,awardIns.transform.position.z);
            seq.Insert(randomTime,awardIns.transform.DOBezier(awardIns.transform.position,cpos,
                TargetPosIns.transform.position,
                Random.Range(flyTimeRange.x,flyTimeRange.y), () =>
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
