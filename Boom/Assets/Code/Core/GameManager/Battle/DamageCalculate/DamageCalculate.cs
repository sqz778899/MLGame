using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public static class DamageCalculate
{
    /*public static List<DamageResult> ApplyElementReactionDamage(
        ElementReactionType reaction, ElementZoneData a,
        ElementZoneData b, ElementZoneData c)
    {
        // 构造敌人目标列表（从前到后打）
        List<IDamageable> targets = GetIDamageList();
        if (targets == null) return null;
        int damage = 0;

        List<DamageResult> resultList = new List<DamageResult>();
        
        switch (reaction)
        {
            case ElementReactionType.Explosion://冰火 => 爆炸
                ApplyExplosion(targets,resultList,a,b); break;
            case ElementReactionType.Overload://雷火 => 过载
                ApplyOverload(targets,resultList,a,b); break;
            case ElementReactionType.Superconduct://冰雷 => 超导
                ApplySuperconduct(targets,resultList,a,b); break;
            case ElementReactionType.CryoExplosion://冰冰火 => 霜爆
                ApplyCryoExplosion(targets,resultList,a,b,c); break;
            case ElementReactionType.Collapse://火火冰 => 坍缩
                ApplyCollapse(targets,resultList,a,b,c); break;
            case ElementReactionType.Shift://冰冰雷 => 迁跃
                ApplyShift(targets,resultList,a,b,c); break;
            case ElementReactionType.Thunderburst://冰雷雷 => 雷涡
                ApplyEchoingThunder(targets,resultList,a,b,c); break;
            case ElementReactionType.EchoingThunder://火雷雷 => 雷霆回响
                ApplyThunderburst(targets,resultList,a,b,c); break;
            case ElementReactionType.BlazingTrail://火火雷 => 流火
                ApplyBlazingTrail(targets,resultList,a,b,c); break;
            default:
                Debug.LogError($"Unknown reaction: {reaction}");
                break;
        }
        return resultList;
    }*/
    
    
    public static IEnumerator ApplyElementReactionDamageAsync(
        ElementReactionType reaction,
        ElementZoneData a,
        ElementZoneData b,
        ElementZoneData c,
        BulletData pseudoBullet,
        Action<DamageResult, IDamageable> onHitVisual = null)
    {
        List<IDamageable> targets = GetIDamageList();
        if (targets == null || targets.Count == 0) yield break;

        List<DamageResult> results = new();

        switch (reaction)
        {
            case ElementReactionType.Explosion:
                yield return ApplyExplosion(targets, results, a, b, pseudoBullet, onHitVisual); break;
            case ElementReactionType.Overload:
                yield return ApplyOverload(targets, results, a, b, pseudoBullet, onHitVisual); break;
            /*case ElementReactionType.Superconduct:
                yield return ApplySuperconduct(targets, results, a, b, pseudoBullet, onHitVisual); break;
            case ElementReactionType.CryoExplosion:
                yield return ApplyCryoExplosion(targets, results, a, b, c, pseudoBullet, onHitVisual); break;
            case ElementReactionType.Collapse:
                yield return ApplyCollapse(targets, results, a, b, c, pseudoBullet, onHitVisual); break;
            case ElementReactionType.Shift:
                yield return ApplyShift(targets, results, a, b, c, pseudoBullet, onHitVisual); break;*/
            /*case ElementReactionType.EchoingThunder:
                yield return ApplyEchoingThunder(targets, results, a, b, c, pseudoBullet, onHitVisual); break;*/
            case ElementReactionType.Thunderburst:
                yield return ApplyThunderburst(targets, results, a, b, c, pseudoBullet, onHitVisual); break;
            /*case ElementReactionType.BlazingTrail:
                yield return ApplyBlazingTrail(targets, results, a, b, c, pseudoBullet, onHitVisual); break;*/
        }
    }

    #region 核心计算公式
    //冰火 => 爆炸 伤害 = min(a, b) * 2
    static IEnumerator ApplyExplosion(List<IDamageable> targets, List<DamageResult> results,
        ElementZoneData a, ElementZoneData b, BulletData pseudoBullet,
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
    
    //雷火 => 过载 伤害 = a + b
    static IEnumerator ApplyOverload(List<IDamageable> targets, List<DamageResult> results,
        ElementZoneData a, ElementZoneData b, BulletData pseudoBullet,
        Action<DamageResult, IDamageable> onHitVisual)
    {
        if (targets.Count < 1) yield break;
        
        int damage = a.ElementalInfusionValue + b.ElementalInfusionValue;
        IDamageable target = targets[0];
        DamageResult result = target.TakeReactionDamage(damage);
        results.Add(result);
        onHitVisual?.Invoke(result, target);
        yield return new WaitForSeconds(0.2f);
    }
    //冰雷 => 超导 伤害 = min(a, b)
    static void ApplySuperconduct(List<IDamageable> targets,List<DamageResult> results,
        ElementZoneData a, ElementZoneData b)
    {
        int damage = Mathf.Min(a.ElementalInfusionValue, b.ElementalInfusionValue);
        for (int i = 0; i < targets.Count; i++)
        {
            results.Add(targets[i].TakeReactionDamage(damage));
            //Debug.Log($"[冰雷 => 超导] Hit back target for {damage}");
        }
    }
    //冰冰火 => 霜爆 伤害 = a + b + c
    static void ApplyCryoExplosion(List<IDamageable> targets, List<DamageResult> results,
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
    //火火冰 => 坍缩  伤害 = (a + b + c) * 2
    static void ApplyCollapse(List<IDamageable> targets,List<DamageResult> results,
        ElementZoneData a, ElementZoneData b, ElementZoneData c)
    {
        if (targets.Count == 0) return;
        int damage = (a.ElementalInfusionValue + b.ElementalInfusionValue + c.ElementalInfusionValue) * 2;
        results.Add(targets[0].TakeReactionDamage(damage));
        //Debug.Log($"[火火冰 => 坍缩] Hit back target for {damage}");
    }
    //冰冰雷 => 迁跃  伤害 = bullet + a + b + c
    static void ApplyShift(List<IDamageable> targets, List<DamageResult> results,
        ElementZoneData a, ElementZoneData b, ElementZoneData c)
    {
        List<IDamageable> currentTargets = targets.Where(t => !t.IsDead).ToList();
        if (currentTargets.Count == 0) return;
        if (GM.Root.BattleMgr.battleData.CurAttackBullet == null) return;
        //穿透转化为攻击次数
        BulletData curAttcBulletData = GM.Root.BattleMgr.battleData.CurAttackBullet;
        int curBulletPiercing = curAttcBulletData.FinalPiercing;
        int curBulletDamage = curAttcBulletData.FinalDamage;
        int damage = curBulletDamage + a.ElementalInfusionValue + b.ElementalInfusionValue + c.ElementalInfusionValue;
        
        for (int i = 0; i < curBulletPiercing; i++)
        {
            int targetIndex = Random.Range(0, currentTargets.Count);
            results.Add(currentTargets[targetIndex].TakeReactionDamage(damage));
            //Debug.Log($"[冰冰雷 => 迁跃] Hit back target for {damage}");
        }
    }
    //冰雷雷 => 雷涡 伤害 = Random(a, b, c)
    static IEnumerator ApplyThunderburst(
        List<IDamageable> targets, List<DamageResult> results,
        ElementZoneData a, ElementZoneData b, ElementZoneData c, 
        BulletData pseudoBullet, Action<DamageResult, IDamageable> onHitVisual)
    {
        List<IDamageable> currentTargets = targets.Where(t => !t.IsDead).ToList();
        if (currentTargets.Count == 0) yield break;

        List<int> damageOptions = new() { a.ElementalInfusionValue, b.ElementalInfusionValue, c.ElementalInfusionValue };
        int totalHits = 5;
        int maxHitsPerTarget = 2;
        Dictionary<IDamageable, int> hitCounts = new();

        int attempts = 0;
        int maxAttempts = 20;
        int hitSoFar = 0;

        while (hitSoFar < totalHits && attempts < maxAttempts)
        {
            attempts++;

            // 随机选择目标
            IDamageable target = currentTargets[Random.Range(0, currentTargets.Count)];

            if (hitCounts.TryGetValue(target, out int count) && count >= maxHitsPerTarget)
                continue;

            // 随机选择伤害值
            int damage = damageOptions[Random.Range(0, damageOptions.Count)];
            DamageResult result = target.TakeReactionDamage(damage);
            results.Add(result);

            //yield return PlayReactionVFX("vfx_01", target);
            // 演出
            onHitVisual?.Invoke(result, target);

            // 更新命中次数
            if (!hitCounts.ContainsKey(target)) hitCounts[target] = 0;
            hitCounts[target]++;
            hitSoFar++;

            yield return new WaitForSeconds(0.2f);
        }
    }
    
    //火雷雷 => 雷霆回响 伤害 = (a + b + c) x 2  50%暴击
    static void ApplyEchoingThunder(List<IDamageable> targets, List<DamageResult> results,
        ElementZoneData a, ElementZoneData b, ElementZoneData c)
    {
        List<IDamageable> currentTargets = targets.Where(t => !t.IsDead).ToList();
        if (currentTargets.Count == 0) return;
        
        bool isCritical = Random.Range(0f, 1f) < 0.5f;
        int damage = (a.ElementalInfusionValue + b.ElementalInfusionValue + c.ElementalInfusionValue) * 2;
        if (isCritical)
            damage *= 2;
        
        IDamageable target = currentTargets[Random.Range(0, currentTargets.Count)];
        results.Add(target.TakeReactionDamage(damage));
        //Debug.Log($"[火雷雷 => 雷霆回响] Hit back target for {damage} 是否暴击 {isCritical} ");
    }
    //火火雷 => 流火  伤害 = (a + b + c)
    static void ApplyBlazingTrail(List<IDamageable> targetsAll, List<DamageResult> results,
        ElementZoneData a, ElementZoneData b, ElementZoneData c)
    {
        int damage = a.ElementalInfusionValue + b.ElementalInfusionValue + c.ElementalInfusionValue;
        int maxRepeat = 10; // 避免极端死循环
        int repeatCount = 0;

        List<IDamageable> currentTargets = targetsAll.Where(t => !t.IsDead).ToList();

        while (currentTargets.Count > 0 && repeatCount < maxRepeat)
        {
            repeatCount++;
            bool someoneDied = false;

            foreach (var target in currentTargets)
            {
                var result = target.TakeReactionDamage(damage);
                results.Add(result);

                if (result.IsDestroyed)
                    someoneDied = true;
            }

            if (!someoneDied)
                break;

            // 更新目标列表（去掉死的）
            currentTargets = targetsAll.Where(t => !t.IsDead).ToList();
        }
        //Debug.Log($"[流火] BlazingTrail triggered {repeatCount} times, {results.Count} hits, total {results.Sum(r => r.EffectiveDamage)} dmg.");
    }
    #endregion

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
    
    static IEnumerator PlayReactionVFX(string vfxName, IDamageable target)
    {
        Transform hitPoint = null;

        // 尝试获取特效位置
        /*if (target is Enemy e)
            hitPoint = e.Controller.GetHitPosition();
        else if (target is Shield s)
            hitPoint = s.Controller.GetHitPosition();*/

        if (hitPoint != null)
        {
            //GameObject vfx = VFXFactory.Create(vfxName, hitPoint.position); // 自定义你的特效工厂
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