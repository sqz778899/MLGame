using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class DamageCalculate
{
    //火火冰 => 坍缩  伤害 = (a + b + c) * 2
    static IEnumerator ApplyCollapse(List<IDamageable> targets,List<DamageResult> results,
        ElementZoneData a, ElementZoneData b, ElementZoneData c,Action<DamageResult, IDamageable> onHitVisual)
    {
        if (targets.Count == 0) yield break;
        
        int damage = (a.ElementalInfusionValue + b.ElementalInfusionValue + c.ElementalInfusionValue) * 2;
        IDamageable target = targets[0];
        DamageResult result = target.TakeReactionDamage(damage);
        results.Add(result);
        onHitVisual?.Invoke(result, target);
        yield return new WaitForSeconds(0.2f);
        //Debug.Log($"[火火冰 => 坍缩] Hit back target for {damage}");
    }
    
    static void ApplyCollapseSimulate(List<IDamageable> targets,List<DamageResult> results,
        ElementZoneData a, ElementZoneData b, ElementZoneData c)
    {
        if (targets.Count == 0) return;
        int damage = (a.ElementalInfusionValue + b.ElementalInfusionValue + c.ElementalInfusionValue) * 2;
        results.Add(targets[0].TakeReactionDamage(damage));
        //Debug.Log($"[火火冰 => 坍缩] Hit back target for {damage}");
    }
}