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
    public GameObject BagTarget;

    #region 总接口
    public void CreatEffect(EParameter EPara,bool IsLine = false,Action onFinish = null)
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
            case EffectType.Shop:
                insPath = PathConfig.AwardCoin;
                targetpos = BagTarget.transform.position;
                break;
            case EffectType.FlipReward:  //新加翻找掉落走这条
                insPath = PathConfig.AwardTrait;
                targetpos = BagTarget.transform.position;
                break;
            default:
                insPath = PathConfig.AwardCoin;
                targetpos = CoinsTarget.transform.position;
                break;
        }

        if (IsLine)
            PlayEffectIsLine(EPara,targetpos, insPath,onFinish);
        else
            PlayAwardEffect(EPara,targetpos, insPath,onFinish);
    }
    
    public void CreatEffect(EParameter EPara,GameObject Ins,bool IsLine = false,Action onFinish = null)
    {
        Vector3 targetpos;
        switch (EPara.CurEffectType)
        {
            case EffectType.CoinsPile:
                targetpos = CoinsTarget.transform.position;
                break;
            case EffectType.RoomKeys:
              
                targetpos = RoomKeysTarget.transform.position;
                break;
            case EffectType.Shop:
                targetpos = BagTarget.transform.position;
                break;
            default:
                targetpos = CoinsTarget.transform.position;
                break;
        }

        if (IsLine)
        {
            PlayEffectIsLine(EPara,targetpos,Ins,onFinish);
        }
        else 
            PlayAwardEffect(EPara,targetpos, Ins,onFinish);
    }
    #endregion

    #region 带一些曲线飞向目标
    //带一些曲线飞向目标
    void PlayAwardEffect(EParameter EPara,Vector3 targetpos,string resPath,Action onFinish = null)
    {
        Sequence seq = DOTween.Sequence();
        float val = Random.Range(EPara.FlyRangeOffset.x,EPara.FlyRangeOffset.y);
        for (int i = 0; i < EPara.InsNum; i++)
        {
            GameObject awardIns = ResManager.instance.CreatInstance(resPath);
            SetEffectSe(awardIns,EPara,targetpos,val,ref seq,onFinish);
        }

        seq.SetUpdate(true);
        seq.AppendCallback(() =>
        {
            onFinish?.Invoke();
        });
    }
    
    void PlayAwardEffect(EParameter EPara,Vector3 targetpos,GameObject Ins,Action onFinish = null)
    {
        Sequence seq = DOTween.Sequence();
        float val = Random.Range(EPara.FlyRangeOffset.x,EPara.FlyRangeOffset.y);
        SetEffectSe(Ins,EPara,targetpos,val,ref seq,onFinish);
        
        seq.SetUpdate(true);
        seq.AppendCallback(() =>
        {
            onFinish?.Invoke();
        });
    } 
    #endregion

    #region 直接飞向目标
    void PlayEffectIsLine(EParameter EPara, Vector3 targetpos, string resPath, Action onFinish = null)
    {
        Sequence seq = DOTween.Sequence();
        float val = Random.Range(EPara.FlyRangeOffset.x,EPara.FlyRangeOffset.y);
        for (int i = 0; i < EPara.InsNum; i++)
        {
            GameObject ins = ResManager.instance.CreatInstance(resPath);
            SetEffectSe(ins, EPara, targetpos, val, ref seq, onFinish);
        }

        seq.SetUpdate(true);
        seq.AppendCallback(() =>
        {
            onFinish?.Invoke();
        });
    }
    
    //直接飞向目标
    void PlayEffectIsLine(EParameter EPara, Vector3 targetpos, GameObject ins, Action onFinish = null)
    {
        Sequence seq = DOTween.Sequence();
        float val = Random.Range(EPara.FlyRangeOffset.x,EPara.FlyRangeOffset.y);
        SetEffectSe(ins,EPara,targetpos,val,ref seq,onFinish);
        seq.SetUpdate(true);
        seq.AppendCallback(() =>
        {
            onFinish?.Invoke();
        });
    }
    #endregion
    
    void SetEffectSe(GameObject awardIns,EParameter EPara,Vector3 targetpos,float val,ref Sequence seq,Action onFinish = null)
    {
        awardIns.transform.SetParent(Root.transform,false);
        float randomTime = Random.Range(EPara.SpawntimeRange.x,EPara.SpawntimeRange.y);
        awardIns.transform.position = EPara.StartPos;
        if (EPara.CurEffectType != EffectType.Shop)
        {
            seq.Insert(0,awardIns.transform.DOMove(
                EPara.StartPos + Random.insideUnitSphere * EPara.Radius,randomTime).SetEase(Ease.OutSine));
        }
        Vector3 cpos = new Vector3(awardIns.transform.position.x + val,
            awardIns.transform.position.y + val,awardIns.transform.position.z);
        seq.Insert(randomTime,awardIns.transform.DOBezier(
            awardIns.transform.position,cpos, targetpos,
            Random.Range(EPara.FlyTimeRange.x,EPara.FlyTimeRange.y), () =>
            {
                FadeOutAndDestroy(awardIns);
                //Destroy(awardIns);
            }));
    }
    
    void FadeOutAndDestroy(GameObject obj)
    {
        var ps = obj.GetComponentInChildren<ParticleSystem>();
        if (ps != null)
        {
            // 1. 停止发射新粒子
            ps.Stop();

            // 2. 等粒子自然结束后销毁
            float delay = ps.main.startLifetime.constantMax; // 粒子最长生命周期
            DOTween.Sequence()
                .AppendInterval(delay)
                .AppendCallback(() => {
                    Destroy(obj);
                });
        }
        else
        {
            // 如果没有粒子，直接销毁
            Destroy(obj);
        }
    }

}

public enum EffectType
{
    CoinsPile = 0,
    RoomKeys = 1,
    Shop = 2,
    FlipReward = 3
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