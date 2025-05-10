using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementZoneManager
{
    public event Action OnZoneChanged;
    public event Action<ElementReactionType, int> OnReaction;
    public List<ElementZoneData> ActiveZones = new();
    List<BulletInner> bulletInners => GM.Root.InventoryMgr.CurBulletsInFight;
    List<ElementReactionInfo> _predictReaction;
    int curReactionIndex = 0;

    public void InitData()
    {
        BattleEventBus.OnFire -= InitDataOnFire;
        BattleEventBus.OnFire += InitDataOnFire;
    }

    //开火之后先预计算元素反应
    public void InitDataOnFire()
    {
        curReactionIndex = 0;
        ActiveZones.Clear();
        _predictReaction = PredictReactions();
    }

    //子弹压入元素场域
    public void ApplyZone(BulletData bulletData)
    {
        if (bulletData.ElementalType == ElementalTypes.Non) return;
        ActiveZones.Add(new ElementZoneData(bulletData.ElementalType, bulletData.ElementalInfusionValue));
        OnZoneChanged?.Invoke();
    }
    
    public void ApplyZoneSimulate(BulletData bulletData)
    {
        if (bulletData.ElementalType == ElementalTypes.Non) return;
        ActiveZones.Add(new ElementZoneData(bulletData.ElementalType, bulletData.ElementalInfusionValue));
    }

    public void TriggerReaction()
    {
        Debug.Log("TriggerReaction");
        if (curReactionIndex >= _predictReaction.Count) return;
        
        ElementZoneData first = ActiveZones.Count > 0 ? ActiveZones[0] : null;
        ElementZoneData second = (1 < ActiveZones.Count) ? ActiveZones[1] : null;
        ElementZoneData third = (2 < ActiveZones.Count) ? ActiveZones[2] : null;
        
        //没有第二个场域，不处理元素反应
        if (first == null || second == null) return;
        
        ElementReactionType reaction = ElementReactionResolver.Resolve(first, second, third);
        //没有元素反应
        if (reaction == ElementReactionType.Non) return;
        //和预测不一样，证明是需要后续元素的三元素反应，等下个子弹打过来，更新场域之后再触发
        ElementReactionType expected = _predictReaction[curReactionIndex].Reaction;
        if (reaction != expected || reaction == ElementReactionType.Non) return;
        
        // 消耗掉场域
        int consumeCount = ElementReactionResolver.GetReactionCount(reaction);
        int pseudoSlotID = 1000 + curReactionIndex;
        
        BulletData reactionBullet = BulletFactory.CreateVirtualReactionBullet(reaction, pseudoSlotID);
        // 锁战斗流程
        GM.Root.BattleMgr.battleFlowLock.LockReaction();
        /// 延迟处理逻辑：交由协程执行
        BattleManager.Instance.StartCoroutine(TriggerReactionAsync(reaction, first, second, third, reactionBullet, consumeCount));

        // 启动协程处理演出 + 战报 + 伤害
        curReactionIndex++;
    }

    public void TriggerReactionSimulate(BulletData pseudoBullet,List<IDamageable> targets)
    {
        if (curReactionIndex >= _predictReaction.Count) return;

        ElementZoneData a = ActiveZones.Count > 0 ? ActiveZones[0] : null;
        ElementZoneData b = ActiveZones.Count > 1 ? ActiveZones[1] : null;
        ElementZoneData c = ActiveZones.Count > 2 ? ActiveZones[2] : null;

        if (a == null || b == null) return;

        ElementReactionType reaction = ElementReactionResolver.Resolve(a, b, c);
        if (reaction != _predictReaction[curReactionIndex].Reaction) return;

        // 直接伤害计算（无需演出）
        DamageCalculate.ApplyElementReactionDamageSimulate(reaction, a, b, c,pseudoBullet,targets);

        int consumeCount = ElementReactionResolver.GetReactionCount(reaction);
        for (int i = consumeCount - 1; i >= 0; i--)
            ActiveZones.RemoveAt(i);

        curReactionIndex++;
    }
    
    IEnumerator TriggerReactionAsync(
        ElementReactionType reaction,
        ElementZoneData a, ElementZoneData b, ElementZoneData c,
        BulletData pseudoBullet, int consumeCount)
    {
        // 1. 演出开始通知
        OnReaction?.Invoke(reaction, consumeCount);

        // 2. 创建战报记录
        var report = BattleManager.Instance.battleData.CurWarReport.GetCurBattleInfo();
        var record = report.GetOrCreateBulletRecord(pseudoBullet);

        // 3. 协程逐击执行伤害
        yield return DamageCalculate.ApplyElementReactionDamageAsync(
            reaction, a, b, c, pseudoBullet,
            (result, target) =>
            {
                // 可选演出
                /*if (target is Enemy e)
                    e.Controller.ShowReactionHitText(result.TotalDamage);
                if (target is Shield s)
                    s.Controller.PlayReactionEffect();*/

                // 战报记录
                int hitIndex = record.Hits.Count;
                var onceHit = new BattleOnceHit(
                    pseudoBullet.FinalDamage, pseudoBullet.FinalPiercing, pseudoBullet.FinalResonance,
                    hitIndex: hitIndex,
                    shieldIndex: result.TargetIndex >= 0 ? result.TargetIndex : -1,
                    enemyIndex: result.TargetIndex < 0 ? 0 : -1,
                    effectiveDamage: result.EffectiveDamage,
                    overflowDamage: result.OverflowDamage,
                    damage: result.TotalDamage,
                    isDestroyed: result.IsDestroyed
                );
                record.RecordHit(onceHit);
            });

        // 4. 清除消耗的场域
        for (int i = consumeCount - 1; i >= 0; i--)
            ActiveZones.RemoveAt(i);

        // 解锁战斗流程
        GM.Root.BattleMgr.battleFlowLock.UnlockReaction();
        
        // 5. UI刷新
        OnZoneChanged?.Invoke();
    }
    
    //预测全部子弹的元素反应
    public List<ElementReactionInfo> PredictReactions()
    {
        List<ElementReactionInfo> results = new();
        int i = 0;

        while (i < bulletInners.Count)
        {
            BulletData first = bulletInners[i].controller.Data;
            BulletData second = (i + 1 < bulletInners.Count) ? bulletInners[i + 1].controller.Data : null;
            BulletData third = (i + 2 < bulletInners.Count) ? bulletInners[i + 2].controller.Data : null;

            if (second == null)
            {
                i++;
                continue;
            }

            ElementZoneData firstZone = new ElementZoneData(first.ElementalType, first.ElementalInfusionValue);
            ElementZoneData secondZone = new ElementZoneData(second.ElementalType, second.ElementalInfusionValue);
            ElementZoneData thirdZone = third != null
                ? new ElementZoneData(third.ElementalType, third.ElementalInfusionValue)
                : null;

            ElementReactionType reaction = ElementReactionResolver.Resolve(firstZone, secondZone, thirdZone);
            ElementReactionInfo reactionInfo = new ElementReactionInfo();
            reactionInfo.Reaction = reaction;
            if (third == null)
            {
                reactionInfo.ReactionBullets = new List<BulletInner>
                    { bulletInners[i], bulletInners[i + 1]};
            }
            else
            {
                reactionInfo.ReactionBullets = new List<BulletInner>
                    { bulletInners[i], bulletInners[i + 1],bulletInners[i + 2]};
            }

            results.Add(reactionInfo);

            // 根据反应类型判断消耗多少颗子弹
            int consumeCount = ElementReactionResolver.GetReactionCount(reaction);
            i += consumeCount > 0 ? consumeCount : 1;
        }

        return results;
    }
}

public class ElementReactionInfo
{
    public ElementReactionType Reaction;
    public List<BulletInner> ReactionBullets;
}

public enum ElementReactionType
{
    Non = 0,
    Explosion = 1,
    Overload = 2,
    Superconduct = 3,
    CryoExplosion = 4,
    Collapse = 5,
    Shift = 6,
    Thunderburst = 7,
    EchoingThunder = 8,
    BlazingTrail = 9
}