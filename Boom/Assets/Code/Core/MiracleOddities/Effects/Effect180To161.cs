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
    public int Id => 163; //黏糊糊的巫师手套 Sticky Wizard Gloves
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