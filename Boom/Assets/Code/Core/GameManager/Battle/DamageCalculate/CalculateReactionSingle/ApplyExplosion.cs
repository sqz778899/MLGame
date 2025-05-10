using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static partial class DamageCalculate
{
    //冰火 => 爆炸 伤害 = min(a, b) * 2
    static IEnumerator ApplyExplosion(List<IDamageable> targets, List<DamageResult> results,
        ElementZoneData a, ElementZoneData b,
        Action<DamageResult, IDamageable> onHitVisual)
    {
        if (targets.Count < 2) yield break;
        int damage = Mathf.Min(a.ElementalInfusionValue, b.ElementalInfusionValue) * 2;
        IDamageable target = targets[1];
        DamageResult result = target.TakeReactionDamage(damage);
        results.Add(result);
        onHitVisual?.Invoke(result, target);
        yield return new WaitForSeconds(0.2f);
    }
    
    static void ApplyExplosionSimulate(List<IDamageable> targets, List<DamageResult> results,
        ElementZoneData a, ElementZoneData b)
    {
        if (targets.Count < 2) return;
        int damage = Mathf.Min(a.ElementalInfusionValue, b.ElementalInfusionValue) * 2;
        DamageResult result = targets[1].TakeReactionDamage(damage);
        results.Add(result);
    }
}