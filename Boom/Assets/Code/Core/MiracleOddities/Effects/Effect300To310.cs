using System.Collections.Generic;
using UnityEngine;


//黏糊糊的巫师手套的缓存数据类型
public class StickyGloveCache
{
    public bool HasTriggered;
    public int BreakShieldOrder;
}

public class Effect_StickyWizardGloves : IMiracleOddityEffect
{
    public int Id => 300; //黏糊糊的巫师手套 Sticky Wizard Gloves
    string cacheKey => $"StickyGlove-{Id}";
    public MiracleOddityTriggerTiming TriggerCash => MiracleOddityTriggerTiming.OnBulletHitAfter;
    public MiracleOddityTriggerTiming TriggerTiming => MiracleOddityTriggerTiming.OnBulletHitBefore;
    BattleTempState battleTempState => GM.Root.BattleMgr.battleData.BattleStateCash;
    
    public void ApplyCash(BattleContext ctx)
    {
        if (ctx.TargetEnemy.IsDead && ctx.TargetEnemy is Shield)
        {
            StickyGloveCache state = battleTempState.Get<StickyGloveCache>(cacheKey)
                                     ?? new StickyGloveCache();
            if (!state.HasTriggered)
            {
                state.HasTriggered = true;
                state.BreakShieldOrder = ctx.CurBullet.OrderInRound;
                battleTempState.Set(cacheKey, state);
                //Debug.Log($"[黏糊糊的巫师手套] 记录第 {state.BreakShieldOrder} 发子弹击破护盾");
            }
        }
    }

    public void Apply(BattleContext ctx)
    {
        StickyGloveCache state = battleTempState.Get<StickyGloveCache>(cacheKey);
        if (state != null && state.HasTriggered && ctx.CurBullet.OrderInRound == state.BreakShieldOrder + 1)
            ctx.CurBullet.FinalDamage += 3;
       // Debug.Log($"[黏糊糊的巫师手套] 触发");
    }
    public void RemoveEffect(){}
    public string GetDescription() => "首次击穿护盾后，下一发子弹伤害+3";
}

public class Effect_BloodhoundRing : IMiracleOddityEffect
{
    public int Id => 301; //溅血猎犬的指环 Bloodhound's Ring
    string cacheKey => $"BloodhoundRing-{Id}";
    public MiracleOddityTriggerTiming TriggerCash => MiracleOddityTriggerTiming.None;
    public MiracleOddityTriggerTiming TriggerTiming => MiracleOddityTriggerTiming.OnAlltimes;
    List<BulletData> bullets => GM.Root.InventoryMgr._BulletInvData.EquipBullets;
    public void ApplyCash(BattleContext ctx) {}

    public void Apply(BattleContext ctx)
    {
        if (ctx.AllBullets.Count == 0) return;
        
        BulletData firstBullet = ctx.AllBullets[0];
        int lostHearts = GM.Root.PlayerMgr._PlayerData.LostHPs;
        firstBullet.ModifierDamageAdditionDict[cacheKey] = lostHearts;
        firstBullet.SyncFinalAttributes();
        //Debug.Log($"[溅血猎犬的指环] 首颗子弹伤害 +{lostHearts}");
    }

    public void RemoveEffect()
    {
        foreach (var each in bullets)
        {
            each.ModifierDamageAdditionDict[cacheKey] = 0;
            each.SyncFinalAttributes();
        }
    }
    public string GetDescription() => "你在本局游戏中每失去一颗心，首颗子弹获得一次伤害+1";
}

//残响魔链Cash类
public class ResonantMagicChainCache
{
    public bool HasTriggered = false;
    public int BlockedBulletOrder = -1; // 被护盾挡住的子弹编号
}

public class Effect_ResonantMagicChain : IMiracleOddityEffect
{
    public int Id => 302; //残响魔链 Resonant Magic Chain
    string cacheKey => $"ResonantMagicChain-{Id}";
    public MiracleOddityTriggerTiming TriggerCash => MiracleOddityTriggerTiming.OnBulletHitAfter;
    public MiracleOddityTriggerTiming TriggerTiming => MiracleOddityTriggerTiming.OnBulletHitBefore;
    BattleTempState battleTempState => GM.Root.BattleMgr.battleData.BattleStateCash;
    public void ApplyCash(BattleContext ctx)
    {
        // 条件：子弹未击破护盾，被挡住了（护盾未死）
        if (!ctx.TargetEnemy.IsDead && (ctx.TargetEnemy is Shield || ctx.TargetEnemy is ShieldData))
        {
            ResonantMagicChainCache state = battleTempState.Get<ResonantMagicChainCache>(cacheKey)
                                            ?? new ResonantMagicChainCache();
            if (!state.HasTriggered)
            {
                state.BlockedBulletOrder = ctx.CurBullet.OrderInRound;
                battleTempState.Set(cacheKey, state);
            }
        }
    }

    public void Apply(BattleContext ctx)
    {
        ResonantMagicChainCache state = battleTempState.Get<ResonantMagicChainCache>(cacheKey);
        if (state != null && !state.HasTriggered && 
            ctx.CurBullet.OrderInRound == state.BlockedBulletOrder + 1)
        {
            state.HasTriggered = true;
            ctx.CurBullet.FinalDamage += 2;
            ctx.CurBullet.FinalPiercing += 1;
            Debug.Log($"[残响魔链] 对第 {ctx.CurBullet.OrderInRound} 发子弹生效：+伤害+2 +穿透+1");
        }
    }
    public void RemoveEffect(){}
    public string GetDescription() => "若一颗子弹被护盾挡住，下一颗子弹获得穿透+1 伤害+2，仅生效一次";
}
//Resonant Magic Chain