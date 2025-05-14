using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Effect_ScavengerHandbook : IMiracleOddityEffect
{
    #region 数据
    public int Id => 38; //翻找术手册 Scavenger's Handbook
    public MiracleOddityTriggerTiming TriggerCash => MiracleOddityTriggerTiming.OnAlltimes;
    public MiracleOddityTriggerTiming TriggerTiming => MiracleOddityTriggerTiming.OnAlltimes;
    #endregion
    
    #region 核心实现
    public void ApplyCash(BattleContext ctx) {}
    public void Apply(BattleContext ctx)
    {
        //直接写在ItemEffectManager中
    }
    public void RemoveEffect(){}
    #endregion
    public string GetDescription() => "有50%概率可以额外翻找一次";
}

public class Effect_DraftyFlyingBroom : IMiracleOddityEffect
{
    #region 数据
    public int Id => 37; //漏风的飞天扫帚 Drafty Flying Broom
    public MiracleOddityTriggerTiming TriggerCash => MiracleOddityTriggerTiming.None;
    public MiracleOddityTriggerTiming TriggerTiming => MiracleOddityTriggerTiming.OnBulletHitBefore;
    #endregion
    
    #region 核心实现
    public void ApplyCash(BattleContext ctx) {}
    public void Apply(BattleContext ctx)
    {
        if (ctx.CurBullet.OrderInRound == 2 &&
            (ctx.TargetEnemy is Shield || ctx.TargetEnemy is ShieldData)&&
            ctx.CurBullet.JumpHitCount == 0)
        {
            ctx.CurBullet.JumpHitCount += 1;
            ctx.ShieldSkipCount = true;
        }
    }
    public void RemoveEffect(){}
    #endregion
    public string GetDescription() => "你的第二颗子弹无视敌人的一层护盾";
}

public class Effect_GrudgeTangledScarf : IMiracleOddityEffect
{
    #region 数据
    public int Id => 36; //怨念缠绕的围巾 Grudge-Tangled Scarf
    string cacheKey => $"怨念缠绕的围巾-{Id}";
    public MiracleOddityTriggerTiming TriggerCash => MiracleOddityTriggerTiming.None;
    public MiracleOddityTriggerTiming TriggerTiming => MiracleOddityTriggerTiming.OnAlltimes;
    List<BulletData> bullets => GM.Root.InventoryMgr._BulletInvData.EquipBullets;
    #endregion
    
    #region 核心实现
    public void ApplyCash(BattleContext ctx) {}
    public void Apply(BattleContext ctx)
    {
        foreach (BulletData each in bullets)
        {
            if (each.IsLastBullet)
                each.ModifierDamageAdditionDict[cacheKey] = 2;
            else
                each.ModifierDamageAdditionDict[cacheKey] = 0;
            each.SyncFinalAttributes();
        }
        //用于调试
        //Debug.Log($"[怨念缠绕的围巾] 触发");
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
    public string GetDescription() => "最后一颗子弹伤害+2";
}

public class Effect_LeftFootOfFortune : IMiracleOddityEffect
{
    #region 数据
    public int Id => 26; //左脚的幸运靴 Left Foot of Fortune
    public MiracleOddityTriggerTiming TriggerCash => MiracleOddityTriggerTiming.None;
    public MiracleOddityTriggerTiming TriggerTiming => MiracleOddityTriggerTiming.OnBattleStart;
    #endregion
    
    #region 核心实现
    public void ApplyCash(BattleContext ctx) {}
    public void Apply(BattleContext ctx)
    {
        if (ctx.AllBullets == null || ctx.AllBullets.Count == 0)
            return;

        int count = Mathf.Min(ctx.AllBullets.Count, 3);
        int randomIndex = Random.Range(0, count);

        StatBuff tempBuff = new StatBuff(BulletStatType.Piercing,
            new List<KeyValuePair<int, int>> { new(randomIndex + 1, 1) }
            , BuffSource.Item, Id);
        GM.Root.BattleMgr.battleData.BattleTempBuffMgr.Add(tempBuff);
        
        //用于调试
        //Debug.Log($"[幸运靴] 子弹 {randomIndex + 1} 获得穿透+1");
    }
    public void RemoveEffect(){}
    #endregion
    public string GetDescription() => "战斗开始时，你的第1~3颗子弹中随机一颗获得穿透+1";
}