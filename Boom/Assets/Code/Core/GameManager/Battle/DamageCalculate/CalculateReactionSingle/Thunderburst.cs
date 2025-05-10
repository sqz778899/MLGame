using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static partial class DamageCalculate
{
     //冰雷雷 => 雷涡 伤害 = Random(a, b, c)
    static IEnumerator ApplyThunderburst(
        List<IDamageable> targets, List<DamageResult> results,
        ElementZoneData a, ElementZoneData b, ElementZoneData c, 
        Action<DamageResult, IDamageable> onHitVisual)
    {
        List<IDamageable> currentTargets = targets.Where(t => !t.IsDead).ToList();
        if (currentTargets.Count == 0) yield break;

        ThunderburstInfo newInfo = new ThunderburstInfo(new()
            { a.ElementalInfusionValue, b.ElementalInfusionValue, c.ElementalInfusionValue });
        Dictionary<IDamageable, int> hitCounts = new();

        int attempts = 0;
        int maxAttempts = 20;
        int hitSoFar = 0;

        while (hitSoFar < newInfo.totalHits && attempts < maxAttempts)
        {
            attempts++;
            // 随机选择目标
            IDamageable target = currentTargets[Random.Range(0, currentTargets.Count)];
            if (hitCounts.TryGetValue(target, out int count) && count >= newInfo.maxHitsPerTarget)
                continue;
            // 随机选择伤害值
            int damage = newInfo.damageOptions[Random.Range(0, newInfo.damageOptions.Count)];
            DamageResult result = target.TakeReactionDamage(damage);
            results.Add(result);
            // 更新命中次数
            if (!hitCounts.ContainsKey(target)) hitCounts[target] = 0;
            hitCounts[target]++;
            hitSoFar++;
            
            //触发受击特效演出
            yield return PlayReactionVFX(PathConfig.VFXThunderHit01, target);
            onHitVisual?.Invoke(result, target);
            yield return new WaitForSeconds(0.2f);
        }
    }
    
    static void ApplyThunderburstSimulate(
        List<IDamageable> targets, List<DamageResult> results,
        ElementZoneData a, ElementZoneData b, ElementZoneData c)
    {
        List<IDamageable> currentTargets = targets.Where(t => !t.IsDead).ToList();
        if (currentTargets.Count == 0) return;

        ThunderburstInfo newInfo = new ThunderburstInfo(new()
            { a.ElementalInfusionValue, b.ElementalInfusionValue, c.ElementalInfusionValue });
        Dictionary<IDamageable, int> hitCounts = new();

        int attempts = 0;
        int maxAttempts = 20;
        int hitSoFar = 0;

        while (hitSoFar < newInfo.totalHits && attempts < maxAttempts)
        {
            attempts++;
            // 随机选择目标
            IDamageable target = currentTargets[Random.Range(0, currentTargets.Count)];
            if (hitCounts.TryGetValue(target, out int count) && count >= newInfo.maxHitsPerTarget)
                continue;
            // 随机选择伤害值
            int damage = newInfo.damageOptions[Random.Range(0, newInfo.damageOptions.Count)];
            DamageResult result = target.TakeReactionDamage(damage);
            results.Add(result);
            // 更新命中次数
            if (!hitCounts.ContainsKey(target)) hitCounts[target] = 0;
            hitCounts[target]++;
            hitSoFar++;
        }
    }
    
    class ThunderburstInfo
    {
        public List<int> damageOptions;
        public int totalHits;        //最大打击数
        public int maxHitsPerTarget; //同一敌人最大命中次数
        public ThunderburstInfo(List<int> _damageOptions,int _totalHits = 5, int _maxHitsPerTarget = 2)
        {
            damageOptions = _damageOptions;
            totalHits = _totalHits;
            maxHitsPerTarget = _maxHitsPerTarget;
        }
    }
}