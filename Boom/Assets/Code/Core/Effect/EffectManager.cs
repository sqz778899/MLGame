using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class EffectManager : MonoBehaviour
{
    public GameObject Root;
    [Header("目标GO")] 
    public GameObject CoinsTarget;
    public GameObject RoomKeysTarget;
    
    public void CreatEffect(EParameter EPara)
    {
        string insPath = "";
        Vector3 targetpos;
        switch (EPara.CurEffectType)
        {
            case EffectType.CoinsPile:
                insPath = PathConfig.AwardCoin;
                targetpos = CoinsTarget.transform.position;
                break;
            case EffectType.RoomKeys:
                insPath = PathConfig.AwardRoomKey;
                targetpos = RoomKeysTarget.transform.position;
                break;
            default:
                insPath = PathConfig.AwardCoin;
                targetpos = CoinsTarget.transform.position;
                break;
        }

        PlayAwardEffect(EPara,targetpos, insPath);
    }
    
    public void PlayAwardEffect(EParameter EPara,Vector3 targetpos,string resPath,Action onFinish = null)
    {
        Sequence seq = DOTween.Sequence();
        float val = Random.Range(EPara.FlyRangeOffset.x,EPara.FlyRangeOffset.y);
        for (int i = 0; i < EPara.InsNum; i++)
        {
            GameObject awardIns = ResManager.instance.CreatInstance(resPath);
            awardIns.transform.SetParent(Root.transform,false);
            float randomTime = Random.Range(EPara.SpawntimeRange.x,EPara.SpawntimeRange.y);
            awardIns.transform.position = EPara.StartPos;
            seq.Insert(0,awardIns.transform.DOMove(
                EPara.StartPos + Random.insideUnitSphere * EPara.Radius,randomTime).SetEase(Ease.OutSine));
            Vector3 cpos = new Vector3(awardIns.transform.position.x + val,
                awardIns.transform.position.y + val,awardIns.transform.position.z);
            seq.Insert(randomTime,awardIns.transform.DOBezier(
                awardIns.transform.position,cpos, targetpos,
                Random.Range(EPara.FlyTimeRange.x,EPara.FlyTimeRange.y), () =>
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

public enum EffectType
{
    CoinsPile,
    RoomKeys
}
[Serializable]
public class EParameter
{
    public EffectType CurEffectType;
    public Vector3 StartPos;        //开始位置
    public int InsNum;              //实例的数量
    public Vector2 SpawntimeRange;  //诞生动画时长
    public float Radius;            // 诞生球半径
    public Vector2 FlyRangeOffset;  // 贝塞尔控制点偏移
    public Vector2 FlyTimeRange;    //飞行动画时长

    public EParameter()
    {
        CurEffectType = EffectType.CoinsPile;
        StartPos = Vector3.zero;
        InsNum = 1;
        SpawntimeRange = new Vector2(0.3f, 0.4f);
        Radius = 1;
        FlyRangeOffset = new Vector2(-1, 1);
        FlyTimeRange = new Vector2(0.4f, 0.6f);
    }
}