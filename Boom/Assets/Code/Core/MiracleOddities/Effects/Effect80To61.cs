using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Effect_SilentWatcherBlindfold : IMiracleOddityEffect
{
    #region 数据
    public int Id => 75; //沉默注视者的眼罩 Silent Watcher's Blindfold
    public MiracleOddityTriggerTiming TriggerCash => MiracleOddityTriggerTiming.OnAlltimes;
    public MiracleOddityTriggerTiming TriggerTiming => MiracleOddityTriggerTiming.OnBattleStart;
    #endregion
    
    #region 核心实现
    public void ApplyCash(BattleContext ctx) {}
    public void Apply(BattleContext ctx)
    {
        // 判断敌人护盾数量
        int shieldCount = ctx.CurEnemy?.Shields?.Count ?? 0;
        if (shieldCount < 2) return;

        // 第1颗子弹，伤害+1，穿透+1
        var additions = new List<KeyValuePair<int, int>> { new(1, 1) };

        StatBuff dmgBuff = new StatBuff(BulletStatType.Damage, additions, BuffSource.Item, Id);
        StatBuff pierceBuff = new StatBuff(BulletStatType.Piercing, additions, BuffSource.Item, Id + 10000); // 避免冲突

        GM.Root.BattleMgr.battleData.BattleTempBuffMgr.Add(dmgBuff);
        GM.Root.BattleMgr.battleData.BattleTempBuffMgr.Add(pierceBuff);
        //用于调试
        //Debug.Log($"[沉默注视者的眼罩] 触发");
    }
    public void RemoveEffect(){}
    #endregion
    public string GetDescription() => "若敌人战斗护盾≥2层，第1颗子弹穿透+1,伤害+1";
}

public class Effect_ArrogantKingCrown : IMiracleOddityEffect
{
    #region 数据
    public int Id => 74; //自大的国王王冠 Arrogant King's Crown
    string cacheKey => $"ArrogantKingCrown-{Id}";
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

            if (gemTypes.Count == 1)
                each.ModifierGemAdditionDict[cacheKey] = 1;
            else
                each.ModifierGemAdditionDict[cacheKey] = 0;
            //Debug.Log($"[自大的国王王冠] {gemTypes.Count}");
            each.SyncFinalAttributes();
        }
        //Debug.Log($"[自大的国王王冠] 触发");
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
    public string GetDescription() => "若一颗子弹镶嵌一种类型宝石，则宝石效果+1";
}


public class Effect_CrackedToyWand : IMiracleOddityEffect
{
    #region 数据
    public int Id => 76; //碎裂的玩具魔杖 Cracked Toy Wand
    public MiracleOddityTriggerTiming TriggerCash => MiracleOddityTriggerTiming.None;
    public MiracleOddityTriggerTiming TriggerTiming => MiracleOddityTriggerTiming.OnBattleStart;
    #endregion
    
    #region 核心实现
    public void ApplyCash(BattleContext ctx) {}
    public void Apply(BattleContext ctx)
    {
        IgnoreGemBuff tempBuff = new IgnoreGemBuff(
            BuffSource.Item, Id);
        GM.Root.BattleMgr.battleData.BattleTempBuffMgr.Add(tempBuff);
        //用于调试
        //Debug.Log($"[碎裂的玩具魔杖] 触发");
    }
    public void RemoveEffect(){}
    #endregion
    public string GetDescription() => "你的最后一颗子弹本轮宝石全部失效";
}