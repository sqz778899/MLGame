﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Effect_IridescentGrimoire : IMiracleOddityEffect
{
    #region 数据
    public int Id => 401; //虹彩的魔法书 Iridescent Grimoire
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

public class Effect_ArrogantKingCrown : IMiracleOddityEffect
{
    #region 数据
    public int Id => 402; //自大的国王王冠 Arrogant King's Crown
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

public class Effect_EchoingEdict : IMiracleOddityEffect
{
    #region 数据
    public int Id => 403; //永响之谕 Echoing Edict
    string cacheKey => $"永响之谕-{Id}";
    public MiracleOddityTriggerTiming TriggerCash => MiracleOddityTriggerTiming.None;
    public MiracleOddityTriggerTiming TriggerTiming => MiracleOddityTriggerTiming.OnAlltimes;
    List<BulletData> bullets => GM.Root.InventoryMgr._BulletInvData.EquipBullets;
    #endregion
    
    #region 核心实现
    public void ApplyCash(BattleContext ctx) {}
    public void Apply(BattleContext ctx)
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            if(i == bullets.Count - 1)
                bullets[i].ModifierDamageAdditionDict[cacheKey] = 5;
            else
                bullets[i].ModifierDamageAdditionDict[cacheKey] = 0;
            bullets[i].SyncFinalAttributes();
        }
        //Debug.Log($"[永响之谕] 触发");
    }
    public void RemoveEffect()
    {
        foreach (var each in bullets)
        {
            each.ModifierDamageAdditionDict.Remove(cacheKey);
            each.SyncFinalAttributes();
        }
    }
    #endregion
    public string GetDescription() => "最后一颗子弹伤害#Red(+5)#";
}

//Echoing Edict   永响之谕