using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class DamageCalculate
{
    //冰冰火 => 霜爆 伤害 = a + b + c
    static IEnumerator ApplyCryoExplosion(List<IDamageable> targets, List<DamageResult> results,
        ElementZoneData a, ElementZoneData b, ElementZoneData c,Action<DamageResult, IDamageable> onHitVisual)
    {
        int damage = a.ElementalInfusionValue + b.ElementalInfusionValue + c.ElementalInfusionValue;
        for (int i = 0; i < targets.Count; i++)
        {
            //只攻击前两个目标
            if(i >= 2)  yield break;
            IDamageable target = targets[i];
            DamageResult result = target.TakeReactionDamage(damage);
            results.Add(result);
            onHitVisual?.Invoke(result, target);
            //Debug.Log($"[冰冰火 => 霜爆] Hit back target for {damage}");
        }
        yield return new WaitForSeconds(0.2f);
    }
    
    static void ApplyCryoExplosionSimulate(List<IDamageable> targets, List<DamageResult> results,
        ElementZoneData a, ElementZoneData b, ElementZoneData c)
    {
        int damage = a.ElementalInfusionValue + b.ElementalInfusionValue + c.ElementalInfusionValue;
        for (int i = 0; i < targets.Count; i++)
        {
            //只攻击前两个目标
            if(i >= 2) break;
            results.Add(targets[i].TakeReactionDamage(damage));
            //Debug.Log($"[冰冰火 => 霜爆] Hit back target for {damage}");
        }
    }
}