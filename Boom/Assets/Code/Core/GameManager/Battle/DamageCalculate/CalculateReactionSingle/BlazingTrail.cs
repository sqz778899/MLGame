using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static partial class DamageCalculate
{
    //火火雷 => 流火  伤害 = (a + b + c) 致死循环
    static IEnumerator ApplyBlazingTrail(List<IDamageable> targetsAll, List<DamageResult> results,
        ElementZoneData a, ElementZoneData b, ElementZoneData c,Action<DamageResult, IDamageable> onHitVisual)
    {
        int damage = GetBlazingTrailDamage(a, b, c);
        List<IDamageable> currentTargets = targetsAll.Where(t => !t.IsDead).ToList();

        int maxRepeat = 10; // 避免极端死循环
        int repeatCount = 0;
        while (currentTargets.Count > 0 && repeatCount < maxRepeat)
        {
            repeatCount++;
            bool someoneDied = false;

            foreach (IDamageable target in currentTargets)
            {
                DamageResult result = target.TakeReactionDamage(damage);
                results.Add(result);
                onHitVisual?.Invoke(result, target);
                if (result.IsDestroyed)
                    someoneDied = true;
                yield return new WaitForSeconds(0.2f);
            }

            if (!someoneDied)
                break;
            // 更新目标列表（去掉死的）
            currentTargets = targetsAll.Where(t => !t.IsDead).ToList();
        }
    }
    
    static void ApplyBlazingTrailSimulate(List<IDamageable> targetsAll, List<DamageResult> results,
        ElementZoneData a, ElementZoneData b, ElementZoneData c)
    {
        int damage = GetBlazingTrailDamage(a, b, c);
        List<IDamageable> currentTargets = targetsAll.Where(t => !t.IsDead).ToList();

        int maxRepeat = 10; // 避免极端死循环
        int repeatCount = 0;
        while (currentTargets.Count > 0 && repeatCount < maxRepeat)
        {
            repeatCount++;
            bool someoneDied = false;

            foreach (IDamageable target in currentTargets)
            {
                DamageResult result = target.TakeReactionDamage(damage);
                results.Add(result);

                if (result.IsDestroyed)
                    someoneDied = true;
            }

            if (!someoneDied)
                break;
            // 更新目标列表（去掉死的）
            currentTargets = targetsAll.Where(t => !t.IsDead).ToList();
        }
    }
    
    static int GetBlazingTrailDamage(ElementZoneData a, ElementZoneData b, ElementZoneData c) => 
        a.ElementalInfusionValue + b.ElementalInfusionValue + c.ElementalInfusionValue;
}