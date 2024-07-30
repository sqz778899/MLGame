using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public static class AniUtility
{
    #region 动画名字定义
    public const string Idle = "idle";
    public const string Idle01 = "idle_01";//受伤之后的Idle
    public const string AttackBegin = "attack_begin";
    public const string Attacking = "attacking";
    public const string Run = "run";
    
    public const string Hit01 = "hit_01"; //受击动画
    public const string Dead01 = "dead_01"; //死亡动画
    #endregion

    public static void PlayCommon(SkeletonAnimation curAni,float timeScale,string AniType,bool isloop)
    {
        if (curAni.AnimationName != AniType || curAni.loop != isloop)
        {
            curAni.loop = isloop;
            curAni.AnimationState.SetAnimation(0, AniType,isloop);
            Debug.Log(AniType + isloop);
            curAni.AnimationName = AniType;
            curAni.timeScale = timeScale;
        }
    }
    
    public static void PlayResetAni(SkeletonAnimation curAni,float timeScale,string AniType)
    {
        curAni.AnimationState.SetAnimation(0, AniType,curAni.loop);
        curAni.AnimationName = AniType;
        curAni.timeScale = timeScale;
    }
    
    public static void PlayIdle(SkeletonAnimation curAni,float timeScale=1f)
    {
        PlayCommon(curAni, timeScale, Idle,true);
    }
    
    public static void PlayIdle01(SkeletonAnimation curAni,float timeScale=1f)
    {
        PlayCommon(curAni, timeScale, Idle01,true);
    }
    
    public static void PlayDead01(SkeletonAnimation curAni,float timeScale=1f)
    {
        PlayCommon(curAni, timeScale, Dead01,false);
    }

    public static void PlayAttack(SkeletonAnimation curAni,ref float anitime,float timeScale=1f)
    {
        PlayCommon(curAni, timeScale, AttackBegin,false);
        anitime = curAni.state.GetCurrent(0).Animation.Duration;
    }
    
    public static void PlayHit01(SkeletonAnimation curAni,ref float anitime,float timeScale=1f)
    {
        PlayResetAni(curAni, timeScale, Hit01);
        anitime = curAni.state.GetCurrent(0).Animation.Duration;
    }
    
    public static void PlayAttacking(SkeletonAnimation curAni,float timeScale=1f)
    {
        PlayCommon(curAni, timeScale, Attacking,false);
    }
    
    public static void PlayRun(SkeletonAnimation curAni,float timeScale=1f)
    {
        PlayCommon(curAni, timeScale, Run,true);
    }
    
    public static void TrunAround(SkeletonAnimation curAni,float face)
    {
        curAni.skeleton.ScaleX = face;
    }
}