using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Trait_MagicInstability : IItemSynergies
{
    public string Name => "施法失衡";
    public string Description => "你每轮首次攻击的子弹+2伤害，下一发穿透-1";

    public bool Match(List<ItemData> equippedItems) =>
        equippedItems.Any(i => i.ID == 1) &&
        equippedItems.Any(i => i.ID == 2) &&
        equippedItems.Any(i => i.ID == 300);

    public void ApplyEffect(BattleContext ctx)
    {
        if (ctx.AllBullets.Count > 0)
            ctx.AllBullets[0].FinalDamage += 2;
        if (ctx.AllBullets.Count > 1)
            ctx.AllBullets[1].FinalPiercing -= 1;
    }
    
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnBulletFire;
    public Sprite GetIcon() => null;
}

// 特质：垃圾三宝
public class Trait_JunkSet : IItemSynergies
{
    public string Name => "垃圾三宝";
    public string Description => "战斗开始时获得1金币，但子弹伤害-1";

    public bool Match(List<ItemData> equippedItems) =>
        HasID(equippedItems, 5) &&
        HasID(equippedItems, 7) &&
        HasID(equippedItems, 200);

    public void ApplyEffect(BattleContext ctx)
    {
        PlayerManager.Instance._PlayerData.ModifyCoins(1);
        foreach (var bullet in ctx.AllBullets)
            bullet.FinalDamage -= 1;
    }

    bool HasID(List<ItemData> items, int id) => items.Any(i => i.ID == id);
    
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnBattleStart;
    public Sprite GetIcon() => null;
}


// 特质：纯洁之辉
public class Trait_PurityGlory : IItemSynergies
{
    public string Name => "纯洁之辉";
    public string Description => "若三件道具稀有度相同，所有宝石效果+1";

    public bool Match(List<ItemData> equippedItems)
    {
        if (equippedItems.Count != 3) return false;
        DropedRarity rare = equippedItems[0].Rarity;
        return equippedItems.All(item => item.Rarity == rare);
    }

    public void ApplyEffect(BattleContext ctx)
    {
        /*foreach (var bullet in ctx.AllBullets)
            bullet.Modifiers.ForEach(m => m.BonusEffectValue += 1); // 假设每个 modifier 有这个字段*/
    }
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnAlltimes;
    public Sprite GetIcon()=> null;
}