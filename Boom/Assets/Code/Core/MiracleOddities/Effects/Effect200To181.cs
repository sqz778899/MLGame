using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Effect_ShabbyHat: IMiracleOddityEffect
{
    public int Id => 198;//蹩脚的魔术师帽子 Shabby Magician's Hat
    public MiracleOddityTriggerTiming TriggerCash => MiracleOddityTriggerTiming.None;
    public MiracleOddityTriggerTiming TriggerTiming => MiracleOddityTriggerTiming.OnBulletFire;
    public void ApplyCash(BattleContext ctx) {}
    public void Apply(BattleContext ctx)
    {
        if (ctx.AllBullets.Count > 0)
        {
            BulletData b = ctx.AllBullets[0];
            b.FinalDamage = Random.Range(0, 4);
        }
    }
    public void RemoveEffect(){}
    public string GetDescription() => "你第一个子弹的伤害随机变化（0～3），发射前无法预知。";
}

public class Effect_ScreamingClayJar : IMiracleOddityEffect
{
    public int Id => 197; //尖叫陶罐 Screaming Clay Jar
    public MiracleOddityTriggerTiming TriggerCash => MiracleOddityTriggerTiming.None;
    public MiracleOddityTriggerTiming TriggerTiming => MiracleOddityTriggerTiming.OnBulletHitBefore;
    public void ApplyCash(BattleContext ctx) {}
    public void Apply(BattleContext ctx)
    {
        if (ctx.CurBullet.OrderInRound == 2 && (ctx.TargetEnemy is Shield || ctx.TargetEnemy is ShieldData ))
            ctx.CurBullet.FinalDamage += 1;
    }
    public void RemoveEffect(){}
    public string GetDescription() => "你的第二颗子弹若命中敌人的护盾，则该子弹伤害+1";
}

public class Effect_MismatchedTacticalNotes : IMiracleOddityEffect
{
    public int Id => 196; //错页的战术笔记 Mismatched Tactical Notes
    public MiracleOddityTriggerTiming TriggerCash => MiracleOddityTriggerTiming.None;
    public MiracleOddityTriggerTiming TriggerTiming => MiracleOddityTriggerTiming.OnBulletFire;
    public void ApplyCash(BattleContext ctx) {}
    public void Apply(BattleContext ctx)
    {
        var bullets = GM.Root.InventoryMgr.CurBulletsInFight;

        for (int i = 0; i < bullets.Count; i++)
        {
            int rnd = Random.Range(i, bullets.Count);
            (bullets[i], bullets[rnd]) = (bullets[rnd], bullets[i]);
        }
        //Debug.Log("[错页的战术笔记] 当前战斗子弹顺序打乱成功");
    }
    public void RemoveEffect(){}
    public string GetDescription() => "开火时，你的子弹的顺序会被随机打乱";
}