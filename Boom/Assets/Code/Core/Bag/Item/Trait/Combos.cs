using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Trait_MagicInstability : IItemSynergies
{
    public int Id => 1;
    public string Name => "施法失衡";
    public string Description => "你所有子弹的伤害会随机波动（-3~2）";

    public bool Match(List<ItemData> equippedItems) =>
        equippedItems.Any(i => i.ID == 1) &&
        equippedItems.Any(i => i.ID == 2) &&
        equippedItems.Any(i => i.ID == 300);

    public void ApplyEffect(BattleContext ctx)
    {
        /*if (ctx.AllBullets.Count > 0)
            ctx.AllBullets[0].FinalDamage += 2;
        if (ctx.AllBullets.Count > 1)
            ctx.AllBullets[1].FinalPiercing -= 1;*/
    }
    
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnBulletFire;
    TraitData _data;
    public TraitData Data => _data ??= new TraitData(Id); // 懒加载
}

public class Trait_TeachingDisaster : IItemSynergies
{
    public int Id => 2;
    public string Name => "教学事故";
    public string Description => "第一颗子弹伤害-6，最后一颗子弹伤害+6";

    public bool Match(List<ItemData> equippedItems) =>
        equippedItems.Any(i => i.ID == 8) &&
        equippedItems.Any(i => i.ID == 9) &&
        equippedItems.Any(i => i.ID == 10);

    public void ApplyEffect(BattleContext ctx)
    {
        if (ctx.AllBullets.Count > 0)
            ctx.AllBullets[0].FinalDamage -= 6;

        if (ctx.AllBullets.Count > 4)
            ctx.AllBullets[4].FinalDamage += 6;
    }

    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnBulletFire;
    TraitData _data;
    public TraitData Data => _data ??= new TraitData(Id);
}

public class Trait_ChaosTriangle : IItemSynergies
{
    public int Id => 3;
    public string Name => "混沌三角";
    public string Description => "所有子弹宝石类型变为共振，效果+1（本轮）";

    public bool Match(List<ItemData> equippedItems) =>
        equippedItems.Any(i => i.ID == 1) &&
        equippedItems.Any(i => i.ID == 8) &&
        equippedItems.Any(i => i.ID == 9);

    public void ApplyEffect(BattleContext ctx)
    {
        // TODO: implement logic
    }

    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnBulletFire;
    TraitData _data;
    public TraitData Data => _data ??= new TraitData(Id);
}

public class Trait_ProgressiveSpell : IItemSynergies
{
    public int Id => 4;
    public string Name => "渐强式魔咒";
    public string Description => "第三颗子弹伤害+1，第四颗子弹伤害+2";

    public bool Match(List<ItemData> equippedItems) =>
        equippedItems.Any(i => i.ID == 2) &&
        equippedItems.Any(i => i.ID == 6) &&
        equippedItems.Any(i => i.ID == 7);

    public void ApplyEffect(BattleContext ctx)
    {
        if (ctx.AllBullets.Count > 2)
            ctx.AllBullets[2].FinalDamage += 1;
        if (ctx.AllBullets.Count > 3)
            ctx.AllBullets[3].FinalDamage += 2;
    }

    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnBulletFire;
    TraitData _data;
    public TraitData Data => _data ??= new TraitData(Id);
}

public class Trait_ThirdResonanceChaos : IItemSynergies
{
    public int Id => 5;
    public string Name => "三阶混念术";
    public string Description => "你的所有子弹的共振效果+1";
    
    public bool Match(List<ItemData> equippedItems) =>
        equippedItems.Any(i => i.ID == 9990) &&
        equippedItems.Any(i => i.ID == 9991) &&
        equippedItems.Any(i => i.ID == 3);

    public void ApplyEffect(BattleContext ctx)
    {
        /*foreach (var bullet in ctx.AllBullets)
        {
            if (bullet.HasResonance)
                bullet.ResonanceBonus += 1;
        }*/
    }

    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnBulletFire;
    TraitData _data;
    public TraitData Data => _data ??= new TraitData(Id);
}

public class Trait_UnlimitedCoin : IItemSynergies
{
    public int Id => 6;
    public string Name => "无限金币术";
    public string Description => "你每翻找一次物品时，额外获得#Yellow(2~10)#金币。";

    public bool Match(List<ItemData> equippedItems) =>
        equippedItems.Any(i => i.ID == 4) &&  // 猫头鹰雕像
        equippedItems.Any(i => i.ID == 5) &&  // 黄金钥匙
        equippedItems.Any(i => i.ID == 7);    // 幸运靴

    public void ApplyEffect(BattleContext ctx)
    {
        // 留空：此效果应挂接到“翻找事件”逻辑中，由事件系统调用
    }

    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.Passive;
    TraitData _data;
    public TraitData Data => _data ??= new TraitData(Id);
}

public class Trait_TrashExplosion : IItemSynergies
{
    public int Id => 7;
    public string Name => "垃圾之光";
    public string Description => "如果你有至少两颗子弹未镶嵌宝石，则最后一颗子弹伤害+4，穿透+1";

    public bool Match(List<ItemData> equippedItems) =>
        equippedItems.Any(i => i.ID == 1) &&  // 蹩脚的魔术师帽子
        equippedItems.Any(i => i.ID == 9) &&  // 碎裂的玩具魔杖 
        equippedItems.Any(i => i.ID == 10);    // 发霉的训练日志

    public void ApplyEffect(BattleContext ctx)
    {
        /*int emptyCount = ctx.AllBullets.Count(b => b.Sockets.All(s => s.IsEmpty));
        if (emptyCount >= 2 && ctx.AllBullets.Count > 4)
        {
            ctx.AllBullets[4].FinalDamage += 4;
            ctx.AllBullets[4].FinalPiercing += 1;
        }*/
    }

    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnBulletFire;
    TraitData _data;
    public TraitData Data => _data ??= new TraitData(Id);
}

public class Trait_ScreamingOwl : IItemSynergies
{
    public int Id => 8;
    public string Name => "尖叫猫头鹰";

    public string Description => "战斗开始时，敌人护盾血量减少1~2。";

    public bool Match(List<ItemData> equippedItems) =>
        equippedItems.Any(i => i.ID == 2) &&   // 尖叫陶罐
        equippedItems.Any(i => i.ID == 4) &&   // 晕头转向的猫头鹰雕像
        equippedItems.Any(i => i.ID == 204);   // 翻找术手册

    public void ApplyEffect(BattleContext ctx)
    {
    }

    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnBattleStart;

    TraitData _data;
    public TraitData Data => _data ??= new TraitData(Id);
}


public class Trait_RainbowResonantTrap : IItemSynergies
{
    public int Id => 9;
    public string Name => "虹彩混响陷阱";

    public string Description => "你所有子弹在战斗开始时，其宝石类型将统一变为随机的一种（共振 / 穿透 / 伤害）";

    public bool Match(List<ItemData> equippedItems) =>
        equippedItems.Any(i => i.ID == 200) &&  // 写满错字的魔法书
        equippedItems.Any(i => i.ID == 300) &&  // 黏糊糊的巫师手套
        equippedItems.Any(i => i.ID == 400);    // 虹彩的魔法书

    public void ApplyEffect(BattleContext ctx)
    {
       
    }

    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnBattleStart;

    TraitData _data;
    public TraitData Data => _data ??= new TraitData(Id);
}

