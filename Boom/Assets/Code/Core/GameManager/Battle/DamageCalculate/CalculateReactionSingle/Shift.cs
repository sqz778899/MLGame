using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static partial class DamageCalculate
{
    //冰冰雷 => 迁跃  伤害 = bullet + a + b + c
    static IEnumerator ApplyShift(List<IDamageable> targets, List<DamageResult> results,
        ElementZoneData a, ElementZoneData b, ElementZoneData c
        ,BulletData pseudoBullet,Action<DamageResult, IDamageable> onHitVisual)
    {
        List<IDamageable> currentTargets = targets.Where(t => !t.IsDead).ToList();
        if (currentTargets.Count == 0) yield break;
        
        ShiftInfo info = GetShiftInfo(a, b, c, pseudoBullet);
        
        for (int i = 0; i < info.totalHits; i++)
        {
            int targetIndex = Random.Range(0, currentTargets.Count);
            IDamageable target = currentTargets[targetIndex];
            DamageResult result = target.TakeReactionDamage(info.damage);
            results.Add(result);
            onHitVisual?.Invoke(result, target);
            //Debug.Log($"[冰冰雷 => 迁跃] Hit back target for {damage}");
        }
        yield return new WaitForSeconds(0.2f);
    }
    
    static void ApplyShiftSimulate(List<IDamageable> targets, List<DamageResult> results,
        ElementZoneData a, ElementZoneData b, ElementZoneData c,BulletData pseudoBullet)
    {
        List<IDamageable> currentTargets = targets.Where(t => !t.IsDead).ToList();
        if (currentTargets.Count == 0) return;

        ShiftInfo info = GetShiftInfo(a, b, c, pseudoBullet);
        
        for (int i = 0; i < info.totalHits; i++)
        {
            int targetIndex = Random.Range(0, currentTargets.Count);
            results.Add(currentTargets[targetIndex].TakeReactionDamage(info.damage));
            //Debug.Log($"[冰冰雷 => 迁跃] Hit back target for {damage}");
        }
    }

    static ShiftInfo GetShiftInfo(ElementZoneData a, ElementZoneData b, ElementZoneData c,BulletData pseudoBullet)
    {
        ShiftInfo info = new ShiftInfo();
        info.totalHits = pseudoBullet.FinalPiercing;//穿透转化为攻击次数
        info.damage = pseudoBullet.FinalDamage + a.ElementalInfusionValue + b.ElementalInfusionValue + c.ElementalInfusionValue;
        return info;
    }

    class ShiftInfo
    {
        public int totalHits;
        public int damage;
    }
}