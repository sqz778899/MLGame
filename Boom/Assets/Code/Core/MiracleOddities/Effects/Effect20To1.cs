using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Effect_IridescentGrimoire : IMiracleOddityEffect
{
    #region 数据
    public int Id => 15; //虹彩的魔法书 Iridescent Grimoire
    string cacheKey => $"IridescentGrimoire-{Id}";
    public MiracleOddityTriggerTiming TriggerCash => MiracleOddityTriggerTiming.None;
    public MiracleOddityTriggerTiming TriggerTiming => MiracleOddityTriggerTiming.OnAlltimes;
    List<BulletData> bullets => GM.Root.InventoryMgr._BulletInvData.EquipBullets;
    #endregion
    
    #region 核心实现
    public void ApplyCash(BattleContext ctx) {}

    public void Apply(BattleContext ctx)
    {
        foreach (BulletData each in ctx.AllBullets)
        {
            HashSet<string> gemTypes = new();
            foreach (var modifier in each.Modifiers)
            {
                if (modifier is BulletModifierGem gModifier)
                    gemTypes.Add(gModifier.gem.CurGemType.ToString());
            }
            if (gemTypes.Count == 3)
                each.ModifierGemAdditionDict[cacheKey] = 1;
            else
                each.ModifierGemAdditionDict[cacheKey] = 0;
            //Debug.Log($"[虹彩的魔法书] {gemTypes.Count}");
            each.SyncFinalAttributes();
        }
        //Debug.Log($"[虹彩的魔法书] 触发");
    }

    public void RemoveEffect()
    {
        foreach (var each in bullets)
        {
            each.ModifierGemAdditionDict[cacheKey] = 0;
            each.SyncFinalAttributes();
        }
    }
    #endregion
    public string GetDescription() => "若一颗子弹镶嵌的宝石类型不重复，则宝石效果+1";
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
