using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public static partial class DamageCalculate
{
    public static IEnumerator ApplyElementReactionDamageAsync(
        ElementReactionType reaction, ElementZoneData a, ElementZoneData b, ElementZoneData c,
        BulletData pseudoBullet, Action<DamageResult, IDamageable> onHitVisual = null)
    {
        List<IDamageable> targets = GetIDamageList();
        if (targets == null || targets.Count == 0) yield break;

        List<DamageResult> results = new();
        switch (reaction)
        {
            case ElementReactionType.Explosion://冰火 => 爆炸 伤害 = min(a, b) * 2
                yield return ApplyExplosion(targets, results, a, b, onHitVisual); break;
            case ElementReactionType.Overload: //雷火 => 过载 伤害 = a + b
                yield return ApplyOverload(targets, results, a, b, onHitVisual); break;
            case ElementReactionType.Superconduct://冰雷 => 超导 伤害 = min(a, b)
                yield return ApplySuperconduct(targets, results, a, b, onHitVisual); break;
            case ElementReactionType.CryoExplosion: //冰冰火 => 霜爆 伤害 = a + b + c
                yield return ApplyCryoExplosion(targets, results, a, b, c, onHitVisual); break;
            case ElementReactionType.Collapse://火火冰 => 坍缩  伤害 = (a + b + c) * 2
                yield return ApplyCollapse(targets, results, a, b, c, onHitVisual); break;
            case ElementReactionType.Shift://冰冰雷 => 迁跃  伤害 = bullet + a + b + c
                yield return ApplyShift(targets, results, a, b, c, pseudoBullet, onHitVisual); break;
            case ElementReactionType.Thunderburst://冰雷雷 => 雷涡 伤害 = Random(a, b, c)
                yield return ApplyThunderburst(targets, results, a, b, c, onHitVisual); break;
            case ElementReactionType.EchoingThunder://火雷雷 => 雷霆回响 伤害 = (a + b + c) x 2  50%暴击
                yield return ApplyEchoingThunder(targets, results, a, b, c, onHitVisual); break;
            case ElementReactionType.BlazingTrail: //火火雷 => 流火  伤害 = (a + b + c) 致死循环
                yield return ApplyBlazingTrail(targets, results, a, b, c, onHitVisual); break;
        }
    }

    public static void ApplyElementReactionDamageSimulate(
        ElementReactionType reaction, ElementZoneData a, ElementZoneData b, ElementZoneData c,
        BulletData pseudoBullet, List<IDamageable> targets)
    {
        if (targets == null || targets.Count == 0) return;

        List<DamageResult> results = new();
        switch (reaction)
        {
            case ElementReactionType.Explosion://冰火 => 爆炸 伤害 = min(a, b) * 2
                ApplyExplosionSimulate(targets, results, a, b); break;
            case ElementReactionType.Overload: //雷火 => 过载 伤害 = a + b
                ApplyOverloadSimulate(targets, results, a, b); break;
            case ElementReactionType.Superconduct://冰雷 => 超导 伤害 = min(a, b)
                ApplySuperconductSimulate(targets, results, a, b); break;
            case ElementReactionType.CryoExplosion: //冰冰火 => 霜爆 伤害 = a + b + c
                ApplyCryoExplosionSimulate(targets, results, a, b, c); break;
            case ElementReactionType.Collapse://火火冰 => 坍缩  伤害 = (a + b + c) * 2
                ApplyCollapseSimulate(targets, results, a, b, c); break;
            case ElementReactionType.Shift://冰冰雷 => 迁跃  伤害 = bullet + a + b + c
                ApplyShiftSimulate(targets, results, a, b, c, pseudoBullet); break;
            case ElementReactionType.Thunderburst: //冰雷雷 => 雷涡 伤害 = Random(a, b, c)
                ApplyThunderburstSimulate(targets, results, a, b, c); break;
            case ElementReactionType.EchoingThunder://火雷雷 => 雷霆回响 伤害 = (a + b + c) x 2  50%暴击
                ApplyEchoingThunderSimulate(targets, results, a, b, c); break;
            case ElementReactionType.BlazingTrail: //火火雷 => 流火  伤害 = (a + b + c) 致死循环
                ApplyBlazingTrailSimulate(targets, results, a, b, c); break;
        }
    }

    #region 不关心的方法
    //构建敌人序列以便于处理伤害
    static List<IDamageable> GetIDamageList()
    {
        if (GM.Root.BattleMgr.battleData.CurEnemy == null) return null;
        Enemy curEnemy = GM.Root.BattleMgr.battleData.CurEnemy;
        List<Shield> curShields = GM.Root.BattleMgr.battleData.CurShields;
        
        IDamageable curTarget = null;
        if (GM.Root.BattleMgr.battleData.CurDamageable is Shield tSheild)
            curTarget = tSheild;
        if (GM.Root.BattleMgr.battleData.CurDamageable is Enemy tEnemy)
            curTarget = tEnemy;
        
        // 构造敌人目标列表（从前到后打）
        List<IDamageable> targets = new List<IDamageable>();
        for (int i = curShields.Count - 1; i >= 0; i--)
        {
            // 如果护盾是死亡状态但正是命中的目标，仍保留用于反应伤害处理
            if(curShields[i].IsDead && curShields[i] != curTarget)
                continue;
            targets.Add(curShields[i]);
        }
        targets.Add(curEnemy); // 本体在最后
        return targets;
    }
    
    static IEnumerator PlayReactionVFX(string vfxPath, IDamageable target)
    {
        Vector3 hitPoint = Vector3.zero;

        // 尝试获取特效位置
        if (target is Enemy e)
            hitPoint = e.Controller.GetHitPosition();
        else if (target is Shield s)
            hitPoint = s.Controller.GetHitPosition();
        
        if (hitPoint != null)
        {
            float hitPosDuration = Random.Range(0.03f, 0.15f);
            HitPauseManager.DoHitPause(hitPosDuration,0);
            EternalCavans.Instance.ScreenFlashEffectSC.Flash(baseColor:new Color(0.8f, 0.8f, 0.5f, 0.2f));
            Vector3 shakeStrength = new Vector3(Random.Range(0.3f, 1f),Random.Range(0.3f, 0.5f),0);
            BattleCameraUtility.ShakeCamera(_strength:shakeStrength);// 震动
            VFXFactory.PlayFx(vfxPath, hitPoint);// 播放特效
            yield return new WaitForSeconds(0.1f); // 等待特效起跳或命中特效展示
        }
    }  

    #endregion
}

public static class ElementReactionResolver
{
    public static ElementReactionType Resolve(ElementZoneData first,
        ElementZoneData second, ElementZoneData third)
    {
        if (first == null || second == null)
            return ElementReactionType.Non;

        //元素反应 冰火
        if (first.ElementalType == ElementalTypes.Ice &&
            second.ElementalType == ElementalTypes.Fire)
            return ElementReactionType.Explosion;
        if (first.ElementalType == ElementalTypes.Fire &&
            second.ElementalType == ElementalTypes.Ice)
            return ElementReactionType.Explosion;
        
        //元素反应 雷火
        if (first.ElementalType == ElementalTypes.Fire &&
            second.ElementalType == ElementalTypes.Thunder)
        {
            if (third == null || third.ElementalType != ElementalTypes.Thunder)
                return ElementReactionType.Overload;
            //元素反应 雷霆回响
            if (third.ElementalType == ElementalTypes.Thunder)
                return ElementReactionType.EchoingThunder;
        }
        if (first.ElementalType == ElementalTypes.Thunder &&
            second.ElementalType == ElementalTypes.Fire)
            return ElementReactionType.Overload;
        
        //元素反应 冰雷
        if (first.ElementalType == ElementalTypes.Ice &&
            second.ElementalType == ElementalTypes.Thunder)
        {
            if (third == null || third.ElementalType != ElementalTypes.Thunder)
                return ElementReactionType.Superconduct;
            //元素反应 雷涡
            if (third.ElementalType == ElementalTypes.Thunder)
                return ElementReactionType.Thunderburst;
        }
        if (first.ElementalType == ElementalTypes.Thunder &&
            second.ElementalType == ElementalTypes.Ice)
            return ElementReactionType.Superconduct;
        
        //元素反应 霜爆
        if (first.ElementalType == ElementalTypes.Ice &&
            second.ElementalType == ElementalTypes.Ice &&
            third != null && third.ElementalType == ElementalTypes.Fire)
            return ElementReactionType.CryoExplosion;
        
        //元素反应 坍缩
        if (first.ElementalType == ElementalTypes.Fire &&
            second.ElementalType == ElementalTypes.Fire &&
            third != null && third.ElementalType == ElementalTypes.Ice)
            return ElementReactionType.Collapse;
        
        //元素反应 迁跃
        if (first.ElementalType == ElementalTypes.Ice &&
            second.ElementalType == ElementalTypes.Ice &&
            third != null && third.ElementalType == ElementalTypes.Thunder)
            return ElementReactionType.Shift;
        
        //元素反应 流火
        if (first.ElementalType == ElementalTypes.Fire &&
            second.ElementalType == ElementalTypes.Fire &&
            third != null && third.ElementalType == ElementalTypes.Thunder)
            return ElementReactionType.BlazingTrail;
        
        return ElementReactionType.Non;
    }

    public static int GetReactionCount(ElementReactionType _reactionType)
    {
        switch (_reactionType)
        {
            case ElementReactionType.Explosion: return 2;
            case ElementReactionType.Overload: return 2;
            case ElementReactionType.Superconduct: return 2;
            case ElementReactionType.CryoExplosion: return 3;
            case ElementReactionType.Collapse: return 3;
            case ElementReactionType.Shift: return 3;
            case ElementReactionType.EchoingThunder: return 3;
            case ElementReactionType.Thunderburst: return 3;
            case ElementReactionType.BlazingTrail: return 3;
        }
        return 0;
    }
}