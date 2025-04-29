using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Effect_ShabbyHat: IItemEffect
{
    public int Id => 1;//蹩脚的魔术师帽子 Shabby Magician's Hat
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnBulletFire;

    public void Apply(BattleContext ctx)
    {
        if (ctx.AllBullets.Count > 0)
        {
            BulletData b = ctx.AllBullets[0];
            b.FinalDamage = Random.Range(0, 4);
        }
    }
    public string GetDescription() => "你第一个子弹的伤害随机变化（0～3），发射前无法预知。";
}

public class Effect_ScreamingClayJar : IItemEffect
{
    public int Id => 2; //尖叫陶罐 Screaming Clay Jar
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnBulletHit;

    public void Apply(BattleContext ctx)
    {
        if (ctx.OrderInRound == 2 && ctx.TargetEnemy is Shield)
            ctx.CurBullet.FinalDamage += 1;
    }
    public string GetDescription() => "你的第二颗子弹若命中敌人的护盾，则该子弹伤害+1";
}

public class Effect_MismatchedTacticalNotes : IItemEffect
{
    public int Id => 3; //错页的战术笔记 Mismatched Tactical Notes
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnBulletFire;

    public void Apply(BattleContext ctx)
    {
        var bullets = GM.Root.InventoryMgr.CurBulletsInFight;

        for (int i = 0; i < bullets.Count; i++)
        {
            int rnd = Random.Range(i, bullets.Count);
            (bullets[i], bullets[rnd]) = (bullets[rnd], bullets[i]);
        }

        Debug.Log("[错页的战术笔记] 当前战斗子弹顺序打乱成功");
    }
    public string GetDescription() => "开火时，你的子弹的顺序会被随机打乱";
}

public class Effect_DizzyOwlFigurine : IItemEffect
{
    public int Id => 4; //错晕头转向的猫头鹰雕像 Dizzy Owl Figurine
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnBattleStart;

    public void Apply(BattleContext ctx)
    {
        if (ctx.CurEnemy == null || ctx.CurEnemy.Shields == null) return;
        
        List<ShieldData> shieldData = ctx.CurEnemy.Shields;
        foreach (var shield in shieldData)
        {
            int randomChange = Random.Range(0, 2) == 0 ? -1 : 1; // ±1
            shield.ModifyHP(randomChange);
        }
    }
    public string GetDescription() => "战斗开始时，敌方护盾血量随机±1";
}

public class Effect_GoldenKey : IItemEffect
{
    public int Id => 5; //胆小鬼的黄金钥匙 Coward's Golden Key
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnEnterRoom;

    public void Apply(BattleContext ctx)
    {
        Debug.Log("胆小鬼的黄金钥匙");
        if (!ctx.IsFirstEnterRoom) return; // 不是新房间，不触发！
        
        if (Random.value < 0.5f)
        {
            PlayerManager.Instance._PlayerData.ModifyCoins(4);
            FloatingTextFactory.CreateUIText("+4 Gold", Color.yellow, 32f);
        }
        else
        {
            PlayerManager.Instance._PlayerData.ModifyCoins(-2);
            FloatingTextFactory.CreateUIText("-2 Gold",  Color.red, 32f);
        }

        Debug.Log("每进入一个新房间");
    }
    public string GetDescription() => "每进入一个新房间：50%得4金币，50%失2金币。";
}
//Drafty Flying Broom