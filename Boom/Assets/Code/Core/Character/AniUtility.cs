using Spine;
using Spine.Unity;

public static class AniUtility
{
    #region 动画名字定义
    const string Idle = "idle";
    const string AttackBegin = "attack_begin";
    const string Attacking = "attacking";
    const string Run = "run";
    #endregion

    static void PlayCommon(SkeletonAnimation curAni,float timeScale,string AniType)
    {
        if (curAni.AnimationName != AniType)
        {
            curAni.AnimationName = AniType;
            curAni.timeScale = timeScale;
        }
    }
    public static void PlayIdle(SkeletonAnimation curAni,float timeScale=1f)
    {
        PlayCommon(curAni, timeScale, Idle);
    }
    
    public static void PlayAttack(SkeletonAnimation curAni,ref float anitime,float timeScale=1f)
    {
        PlayCommon(curAni, timeScale, AttackBegin);
        anitime = curAni.state.GetCurrent(0).Animation.Duration;
    }
    
    public static void PlayAttacking(SkeletonAnimation curAni,float timeScale=1f)
    {
        PlayCommon(curAni, timeScale, Attacking);
    }
    
    public static void PlayRun(SkeletonAnimation curAni,float timeScale=1f)
    {
        PlayCommon(curAni, timeScale, Run);
    }
    
    public static void TrunAround(SkeletonAnimation curAni,float face)
    {
        curAni.skeleton.ScaleX = face;
    }
}