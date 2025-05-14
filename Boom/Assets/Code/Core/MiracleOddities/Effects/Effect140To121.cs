using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Effect_MisspelledSpellbook : IMiracleOddityEffect
{
    public int Id => 129; //写满错字的魔法书 Misspelled Spellbook
    public MiracleOddityTriggerTiming TriggerCash => MiracleOddityTriggerTiming.OnAlltimes;
    public MiracleOddityTriggerTiming TriggerTiming => MiracleOddityTriggerTiming.OnBulletFire;
    public void ApplyCash(BattleContext ctx) {}
    public void Apply(BattleContext ctx)
    {
        if (ctx.AllBullets.Count < 3) return; // 小于三颗子弹不触发
        BulletData bullet = ctx.AllBullets[2];
        
        float roll = Random.value;
        if (roll < 0.3f)
            bullet.GemEffectOverride = GemEffectOverrideState.Ignore;
        else if (roll < 0.8f)
            bullet.GemEffectOverride = GemEffectOverrideState.Double;
        else
            bullet.GemEffectOverride = GemEffectOverrideState.None;
        bullet.SyncFinalAttributes();
        //用于调试
        //Debug.Log($"[写满错字的魔法书] 触发{bullet.GemEffectOverride.ToString()}");
    }
    public void RemoveEffect(){}
    public string GetDescription() => "第三颗子弹，50%概率宝石效果翻倍，30%概率宝石效果失效";
}

public class Effect_GoldenKey : IMiracleOddityEffect
{
    public int Id => 128; //胆小鬼的黄金钥匙 Coward's Golden Key
    public MiracleOddityTriggerTiming TriggerCash => MiracleOddityTriggerTiming.None;
    public MiracleOddityTriggerTiming TriggerTiming => MiracleOddityTriggerTiming.OnEnterRoom;
    public void ApplyCash(BattleContext ctx) {}
    public void Apply(BattleContext ctx)
    {
        if (!ctx.IsFirstEnterRoom) return; // 不是新房间，不触发！
        Vector3 pos = GM.Root.PlayerMgr.RoleInMapGO.transform.position + Vector3.up *2.8f;
        if (Random.value < 0.5f)
        {
            GM.Root.PlayerMgr._PlayerData.ModifyCoins(4);
            FloatingTextFactory.CreateWorldText("胆小鬼的黄金钥匙 +4 硬币",pos,
                FloatingTextType.MapHint, Color.yellow, 4f);
        }
        else
        {
            GM.Root.PlayerMgr._PlayerData.ModifyCoins(-2);
            FloatingTextFactory.CreateWorldText("胆小鬼的黄金钥匙 -2 硬币",pos,
                FloatingTextType.MapHint,  Color.red, 4f);
        }
    }
    public void RemoveEffect(){}
    public string GetDescription() => "每进入一个新房间：50%得4金币，50%失2金币。";
}

public class Effect_TransmutationPracticeQuill : IMiracleOddityEffect
{
    public int Id => 127; //变形术练习笔 Transmutation Practice Quill
    public MiracleOddityTriggerTiming TriggerCash => MiracleOddityTriggerTiming.None;
    public MiracleOddityTriggerTiming TriggerTiming => MiracleOddityTriggerTiming.OnBattleStart;
    public void ApplyCash(BattleContext ctx) {}
    public void Apply(BattleContext ctx)
    {
        ChangeGemTypeBuff tempBuff = new ChangeGemTypeBuff(
            BuffSource.Item, Id);
        GM.Root.BattleMgr.battleData.BattleTempBuffMgr.Add(tempBuff);
        
        //用于调试
        //Debug.Log($"[变形术练习笔] 触发");
    }
    public void RemoveEffect(){}
    public string GetDescription() => "战斗开始时，你镶嵌的宝石中，随机一个类型变为“共振”";
}


#region 残响魔链
//残响魔链Cash类
public class ResonantMagicChainCache
{
    public bool HasTriggered = false;
    public int BlockedBulletOrder = -1; // 被护盾挡住的子弹编号
}

public class Effect_ResonantMagicChain : IMiracleOddityEffect
{
    public int Id => 123; //残响魔链 Resonant Magic Chain
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
#endregion