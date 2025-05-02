using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Effect_ShabbyHat: IItemEffect
{
    public int Id => 1;//蹩脚的魔术师帽子 Shabby Magician's Hat
    public ItemTriggerTiming TriggerCash => ItemTriggerTiming.None;
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnBulletFire;
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

public class Effect_ScreamingClayJar : IItemEffect
{
    public int Id => 2; //尖叫陶罐 Screaming Clay Jar
    public ItemTriggerTiming TriggerCash => ItemTriggerTiming.None;
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnBulletHitBefore;
    public void ApplyCash(BattleContext ctx) {}
    public void Apply(BattleContext ctx)
    {
        if (ctx.CurBullet.OrderInRound == 2 && ctx.TargetEnemy is Shield)
            ctx.CurBullet.FinalDamage += 1;
    }
    public void RemoveEffect(){}
    public string GetDescription() => "你的第二颗子弹若命中敌人的护盾，则该子弹伤害+1";
}

public class Effect_MismatchedTacticalNotes : IItemEffect
{
    public int Id => 3; //错页的战术笔记 Mismatched Tactical Notes
    public ItemTriggerTiming TriggerCash => ItemTriggerTiming.None;
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnBulletFire;
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

public class Effect_DizzyOwlFigurine : IItemEffect
{
    public int Id => 4; //错晕头转向的猫头鹰雕像 Dizzy Owl Figurine
    public ItemTriggerTiming TriggerCash => ItemTriggerTiming.None;
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnBattleStart;
    public void ApplyCash(BattleContext ctx) {}
    public void Apply(BattleContext ctx)
    {
        if (ctx.CurEnemy == null || ctx.CurEnemy.Shields == null) return;
        
        List<TraitData> activeTraits = GM.Root.InventoryMgr._ItemEffectMrg.GetCurrentSynergiesInfos();
        if (activeTraits.Any(t => t.ID == 8)) // Trait ID 8 = 尖叫猫头鹰
        {
            //Debug.Log("[猫头鹰雕像] 被特质“尖叫猫头鹰”覆盖，无效化");
            return;
        }
        
        List<ShieldData> shieldData = ctx.CurEnemy.Shields;
        foreach (var shield in shieldData)
        {
            int randomChange = Random.Range(0, 2) == 0 ? -1 : 1; // ±1
            shield.ModifyHP(randomChange);
           // Debug.Log($"错晕头转向的猫头鹰雕像 {randomChange}");
        }
    }
    public void RemoveEffect(){}
    public string GetDescription() => "战斗开始时，敌方护盾血量随机±1";
}

public class Effect_GoldenKey : IItemEffect
{
    public int Id => 5; //胆小鬼的黄金钥匙 Coward's Golden Key
    public ItemTriggerTiming TriggerCash => ItemTriggerTiming.None;
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnEnterRoom;
    public void ApplyCash(BattleContext ctx) {}
    public void Apply(BattleContext ctx)
    {
        if (!ctx.IsFirstEnterRoom) return; // 不是新房间，不触发！
        Vector3 pos = GM.Root.PlayerMgr.RoleInMapGO.transform.position + Vector3.up *2.8f;
        if (Random.value < 0.5f)
        {
            PlayerManager.Instance._PlayerData.ModifyCoins(4);
            FloatingTextFactory.CreateWorldText("+4 Gold",pos,
                FloatingTextType.MapHint, Color.yellow, 4f);
        }
        else
        {
            PlayerManager.Instance._PlayerData.ModifyCoins(-2);
            FloatingTextFactory.CreateWorldText("-2 Gold",pos,
                FloatingTextType.MapHint,  Color.red, 4f);
        }
    }
    public void RemoveEffect(){}
    public string GetDescription() => "每进入一个新房间：50%得4金币，50%失2金币。";
}

public class Effect_DraftyFlyingBroom : IItemEffect
{
    public int Id => 6; //漏风的飞天扫帚 Drafty Flying Broom
    public ItemTriggerTiming TriggerCash => ItemTriggerTiming.None;
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnBulletHitBefore;
    public void ApplyCash(BattleContext ctx) {}
    public void Apply(BattleContext ctx)
    {
        if (ctx.CurBullet.OrderInRound == 2 &&
            ctx.TargetEnemy is Shield &&
            ctx.CurBullet.JumpHitCount == 0)
        {
            ctx.CurBullet.JumpHitCount += 1;
            ctx.ShieldSkipCount = true;
        }
    }
    public void RemoveEffect(){}
    public string GetDescription() => "你的第二颗子弹无视敌人的一层护盾";
}

public class Effect_LeftFootOfFortune : IItemEffect
{
    public int Id => 7; //左脚的幸运靴 Left Foot of Fortune
    public ItemTriggerTiming TriggerCash => ItemTriggerTiming.None;
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnBattleStart;
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
    public string GetDescription() => "战斗开始时，你的第1~3颗子弹中随机一颗获得穿透+1";
}

public class Effect_TransmutationPracticeQuill : IItemEffect
{
    public int Id => 8; //变形术练习笔 Transmutation Practice Quill
    public ItemTriggerTiming TriggerCash => ItemTriggerTiming.None;
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnBattleStart;
    public void ApplyCash(BattleContext ctx) {}
    public void Apply(BattleContext ctx)
    {
        ChangeGemTypeBuff tempBuff = new ChangeGemTypeBuff(
            BuffSource.Item, Id);
        GM.Root.BattleMgr.battleData.BattleTempBuffMgr.Add(tempBuff);
        
        //用于调试
        //Debug.Log($"[变形术练习笔] 触发");
    }
    public void RemoveEffect(){}
    public string GetDescription() => "战斗开始时，你镶嵌的宝石中，随机一个类型变为“共振”";
}

public class Effect_CrackedToyWand : IItemEffect
{
    public int Id => 9; //碎裂的玩具魔杖 Cracked Toy Wand
    public ItemTriggerTiming TriggerCash => ItemTriggerTiming.None;
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnBattleStart;
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
    public string GetDescription() => "你的最后一颗子弹本轮宝石全部失效";
}

public class Effect_MoldyTrainingLog : IItemEffect
{
    public int Id => 10; //发霉的训练日志 Moldy Training Log
    string cacheKey => $"MoldyTrainingLog-{Id}";
    public ItemTriggerTiming TriggerCash => ItemTriggerTiming.None;
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnAlltimes;
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