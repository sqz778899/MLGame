using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static partial class DamageCalculate
{
    //火雷雷 => 雷霆回响 伤害 = (a + b + c) x 2  50%暴击
    static IEnumerator ApplyEchoingThunder(List<IDamageable> targets, List<DamageResult> results,
        ElementZoneData a, ElementZoneData b, ElementZoneData c,Action<DamageResult, IDamageable> onHitVisual)
    {
        List<IDamageable> currentTargets = targets.Where(t => !t.IsDead).ToList();
        if (currentTargets.Count == 0) yield break;
        
        int damage = GetOverloadDamage(a, b, c);
        IDamageable target = currentTargets[Random.Range(0, currentTargets.Count)];
        DamageResult result = target.TakeReactionDamage(damage);
        results.Add(result);
        onHitVisual?.Invoke(result, target);
        yield return new WaitForSeconds(0.2f);
        //Debug.Log($"[火雷雷 => 雷霆回响] Hit back target for {damage} 是否暴击 {isCritical} ");
    }
    
    static void ApplyEchoingThunderSimulate(List<IDamageable> targets, List<DamageResult> results,
        ElementZoneData a, ElementZoneData b, ElementZoneData c)
    {
        List<IDamageable> currentTargets = targets.Where(t => !t.IsDead).ToList();
        if (currentTargets.Count == 0) return;
        
        int damage = GetOverloadDamage(a, b, c);
        
        IDamageable target = currentTargets[Random.Range(0, currentTargets.Count)];
        results.Add(target.TakeReactionDamage(damage));
        //Debug.Log($"[火雷雷 => 雷霆回响] Hit back target for {damage} 是否暴击 {isCritical} ");
    }

    static int GetOverloadDamage(ElementZoneData a, ElementZoneData b, ElementZoneData c)
    {
        bool isCritical = Random.Range(0f, 1f) < 0.5f;
        int damage = (a.ElementalInfusionValue + b.ElementalInfusionValue + c.ElementalInfusionValue) * 2;
        if (isCritical)
            damage *= 2;
        return damage;
    }
}