using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class DamageCalculate
{
    //冰雷 => 超导 伤害 = min(a, b)
    static IEnumerator ApplySuperconduct(List<IDamageable> targets, List<DamageResult> results,
        ElementZoneData a, ElementZoneData b,
        Action<DamageResult, IDamageable> onHitVisual)
    {
        int damage = Mathf.Min(a.ElementalInfusionValue, b.ElementalInfusionValue);
        for (int i = 0; i < targets.Count; i++)
        {
            IDamageable target = targets[i];
            DamageResult result = target.TakeReactionDamage(damage);
            results.Add(result);
            onHitVisual?.Invoke(result, target);
            //Debug.Log($"[冰雷 => 超导] Hit back target for {damage}");
        }
        yield return new WaitForSeconds(0.2f);
    }
    
    static void ApplySuperconductSimulate(List<IDamageable> targets,List<DamageResult> results,
        ElementZoneData a, ElementZoneData b)
    {
        int damage = Mathf.Min(a.ElementalInfusionValue, b.ElementalInfusionValue);
        for (int i = 0; i < targets.Count; i++)
        {
            results.Add(targets[i].TakeReactionDamage(damage));
            //Debug.Log($"[冰雷 => 超导] Hit back target for {damage}");
        }
    }
}