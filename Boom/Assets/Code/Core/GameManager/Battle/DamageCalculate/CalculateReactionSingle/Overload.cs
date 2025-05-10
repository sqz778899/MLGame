using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class DamageCalculate
{
    //雷火 => 过载 伤害 = a + b
    static IEnumerator ApplyOverload(List<IDamageable> targets, List<DamageResult> results,
        ElementZoneData a, ElementZoneData b,Action<DamageResult, IDamageable> onHitVisual)
    {
        if (targets.Count < 1) yield break;
        
        int damage = GetOverloadDamage(a, b);
        IDamageable target = targets[0];
        DamageResult result = target.TakeReactionDamage(damage);
        results.Add(result);
        onHitVisual?.Invoke(result, target);
        yield return new WaitForSeconds(0.2f);
    }
    
    static void ApplyOverloadSimulate(List<IDamageable> targets, List<DamageResult> results,
        ElementZoneData a, ElementZoneData b)
    {
        if (targets.Count < 1) return;
        
        int damage = GetOverloadDamage(a, b);
        results.Add(targets[0].TakeReactionDamage(damage));
    }
    
    static int GetOverloadDamage(ElementZoneData a, ElementZoneData b) =>
        a.ElementalInfusionValue + b.ElementalInfusionValue;
}