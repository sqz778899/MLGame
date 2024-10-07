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
    public const string AttackBegin_1 = "attack_begin_1";
    public const string AttackBegin_2 = "attack_begin_2";
    public const string Attacking = "attacking";
    public const string Reload = "reload";
    public const string Run = "run";
    
    public const string Hit01 = "hit_01"; //受击动画
    public const string Dead01 = "dead_01"; //死亡动画
    
    public const string Appear = "appear"; //出现动画
    #endregion

    public static void PlayCommon(SkeletonAnimation curAni,float timeScale,string AniType,bool isloop,bool isReset=false,int trackIndex=0)
    {
        #region 容错
        //动画不存在的话直接Return
        if (curAni.Skeleton == null) return;
        if (curAni.Skeleton.Data.FindAnimation(AniType) == null) return;
        #endregion
        
        if (curAni.AnimationName != AniType || curAni.loop != isloop)
        {
            curAni.loop = isloop;
            curAni.AnimationState.SetAnimation(trackIndex, AniType,isloop);
        }

        if (isReset)
        {
            curAni.AnimationState.SetAnimation(trackIndex, AniType,isloop);
        }
        
        curAni.AnimationName = AniType;
        curAni.timeScale = timeScale;
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

    public static void PlayAttack(SkeletonAnimation curAni,ref float anitime,float timeScale=1f,bool isReset=false)
    {
        PlayCommon(curAni, timeScale, AttackBegin,false,isReset);
        PlayCommon(curAni, timeScale, AttackBegin_1,false,isReset,0);
        PlayCommon(curAni, timeScale, AttackBegin_2,false,isReset,1);
        anitime = curAni.state.GetCurrent(0).Animation.Duration;
    }
    
    public static void PlayReload(SkeletonAnimation curAni,ref float anitime,float timeScale=1f)
    {
        PlayCommon(curAni, timeScale, Reload,false);
        anitime = curAni.state.GetCurrent(0).Animation.Duration;
    }
    
    public static void PlayAppear(SkeletonAnimation curAni,ref float anitime,float timeScale=1f)
    {
        PlayCommon(curAni, timeScale, Appear,false);
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