using System.Collections.Generic;
using UnityEngine;

public class Effect_FlickeringCandle : IMiracleOddityEffect
{
    public int Id => 117; //忽明忽暗的蜡烛 Flickering Candle
    public MiracleOddityTriggerTiming TriggerCash => MiracleOddityTriggerTiming.OnAlltimes;
    public MiracleOddityTriggerTiming TriggerTiming => MiracleOddityTriggerTiming.OnBattleStart;
    public void ApplyCash(BattleContext ctx) {}
    public void Apply(BattleContext ctx)
    {
        StatBuff buff = new StatBuff(
            BulletStatType.Damage,
            new List<KeyValuePair<int, int>> { new(1, 1),new(3, 1),new(5, 1) },
            BuffSource.Item, Id);
        
        GM.Root.BattleMgr.battleData.BattleTempBuffMgr.Add(buff);
        //用于调试
        //Debug.Log($"[忽明忽暗的蜡烛] 触发");
    }
    public void RemoveEffect(){}
    public string GetDescription() => "你的第一、第三、第五颗子弹伤害+1";
}

public class Effect_MoldyTrainingLog : IMiracleOddityEffect
{
    public int Id => 116; //发霉的训练日志 Moldy Training Log
    string cacheKey => $"MoldyTrainingLog-{Id}";
    public MiracleOddityTriggerTiming TriggerCash => MiracleOddityTriggerTiming.None;
    public MiracleOddityTriggerTiming TriggerTiming => MiracleOddityTriggerTiming.OnAlltimes;
    List<BulletData> bullets => GM.Root.InventoryMgr._BulletInvData.EquipBullets;
    public void ApplyCash(BattleContext ctx) {}
    public void Apply(BattleContext ctx)
    {
        if (ctx.AllBullets.Count == 0) return;
        
        BulletData firstBullet = ctx.AllBullets[0];
        firstBullet.ModifierDamageAdditionDict[cacheKey] = -1;
        firstBullet.SyncFinalAttributes();
        //用于调试
        //Debug.Log($"[发霉的训练日志] 触发");
    }

    public void RemoveEffect()
    {
        foreach (var each in bullets)
        {
            each.ModifierDamageAdditionDict[cacheKey] = 0;
            each.SyncFinalAttributes();
        }
    }
    public string GetDescription() => "你的第一颗子弹伤害-1";
}