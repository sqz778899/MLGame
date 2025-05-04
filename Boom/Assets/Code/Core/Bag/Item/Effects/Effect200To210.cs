using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Effect_MisspelledSpellbook : IItemEffect
{
    public int Id => 200; //写满错字的魔法书 Misspelled Spellbook
    public ItemTriggerTiming TriggerCash => ItemTriggerTiming.OnAlltimes;
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnBulletFire;
    public void ApplyCash(BattleContext ctx) {}
    public void Apply(BattleContext ctx)
    {
        if (ctx.AllBullets.Count < 3) return; // 小于三颗子弹不触发
        BulletData bullet = ctx.AllBullets[2];
        
        float roll = Random.value;
        if (roll < 0.3f)
            bullet.GemEffectOverride = GemEffectOverrideState.Ignore;
        else if (roll < 0.8f)
            bullet.GemEffectOverride = GemEffectOverrideState.Double;
        else
            bullet.GemEffectOverride = GemEffectOverrideState.None;
        bullet.SyncFinalAttributes();
        //用于调试
        //Debug.Log($"[写满错字的魔法书] 触发{bullet.GemEffectOverride.ToString()}");
    }
    public void RemoveEffect(){}
    public string GetDescription() => "第三颗子弹，50%概率宝石效果翻倍，30%概率宝石效果失效";
}

public class Effect_FlickeringCandle : IItemEffect
{
    public int Id => 201; //忽明忽暗的蜡烛 Flickering Candle
    public ItemTriggerTiming TriggerCash => ItemTriggerTiming.OnAlltimes;
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnBattleStart;
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

public class Effect_GrudgeTangledScarf : IItemEffect
{
    public int Id => 202; //怨念缠绕的围巾 Grudge-Tangled Scarf
    string cacheKey => $"怨念缠绕的围巾-{Id}";
    public ItemTriggerTiming TriggerCash => ItemTriggerTiming.None;
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnAlltimes;
    List<BulletData> bullets => GM.Root.InventoryMgr._BulletInvData.EquipBullets;
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
    public string GetDescription() => "最后一颗子弹伤害+2";
}

public class Effect_SilentWatcherBlindfold : IItemEffect
{
    public int Id => 203; //沉默注视者的眼罩 Silent Watcher's Blindfold
    public ItemTriggerTiming TriggerCash => ItemTriggerTiming.OnAlltimes;
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnBattleStart;
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
    public string GetDescription() => "若敌人战斗护盾≥2层，第1颗子弹穿透+1,伤害+1";
}

public class Effect_ScavengerHandbook : IItemEffect
{
    public int Id => 204; //翻找术手册 Scavenger's Handbook
    public ItemTriggerTiming TriggerCash => ItemTriggerTiming.OnAlltimes;
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnAlltimes;
    public void ApplyCash(BattleContext ctx) {}
    public void Apply(BattleContext ctx)
    {
        //直接写在ItemEffectManager中
    }
    public void RemoveEffect(){}
    public string GetDescription() => "有50%概率可以额外翻找一次";
}