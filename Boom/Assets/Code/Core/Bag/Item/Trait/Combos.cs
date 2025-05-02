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
        if (ctx?.CurBullet == null) return;
      
        int delta = Random.Range(-3, 3); // 上限不包含 3 => [-3, 2]
        ctx.CurBullet.FinalDamage += delta;
        ctx.CurBullet.FinalDamage = Mathf.Max(ctx.CurBullet.FinalDamage, 0); // 确保伤害不为负数
        // 记录日志用于调试
        //Debug.Log($"[特质·施法失衡] 第{ctx.CurBullet.OrderInRound}颗子弹伤害波动：{delta}");
    }
    
    public void RemoveEffect() {}
    public void OnClutterSearchResolved(){}
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnBulletHitBefore;
    TraitData _data;
    public TraitData Data => _data ??= new TraitData(Id); // 懒加载
}

//教学事故
public class Trait_TeachingDisaster : IItemSynergies
{
    public int Id => 2;
    public string Name => "教学事故";
    string cacheKey => $"教学事故-{Id}";
    public string Description => "第一颗子弹伤害-6，最后一颗子弹伤害+6";
    List<BulletData> _bullets => GM.Root.InventoryMgr._BulletInvData.EquipBullets;

    public bool Match(List<ItemData> equippedItems) =>
        equippedItems.Any(i => i.ID == 8) &&
        equippedItems.Any(i => i.ID == 9) &&
        equippedItems.Any(i => i.ID == 10);

    public void ApplyEffect(BattleContext ctx)
    {
        for(int i = 0; i < _bullets.Count; i++)
        {
            if (i == 0)
                _bullets[i].ModifierDamageAdditionDict[cacheKey] = -6;
            else if (_bullets[i].IsLastBullet)
                _bullets[i].ModifierDamageAdditionDict[cacheKey] = 6;
            else
                _bullets[i].ModifierDamageAdditionDict[cacheKey] = 0;
            
            _bullets[i].SyncFinalAttributes();
        }
        //Debug.Log($"[教学事故] 触发");
    }

    public void RemoveEffect()
    {
        foreach (var each in _bullets)
        {
            each.ModifierDamageAdditionDict.Remove(cacheKey);
            each.SyncFinalAttributes();
        }
    }
    public void OnClutterSearchResolved(){}
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnAlltimes;
    TraitData _data;
    public TraitData Data => _data ??= new TraitData(Id);
}

//混沌三角
public class Trait_ChaosTriangle : IItemSynergies
{
    public int Id => 3;
    public string Name => "混沌三角";
    public string Description => "战斗开始时,所有子弹宝石类型变为共振，效果+1";

    public bool Match(List<ItemData> equippedItems) =>
        equippedItems.Any(i => i.ID == 1) &&
        equippedItems.Any(i => i.ID == 8) &&
        equippedItems.Any(i => i.ID == 9);

    public void ApplyEffect(BattleContext ctx)
    {
        Debug.Log("混沌三角");
        
        ChangeGemTypeAllBuff tempBuff = new ChangeGemTypeAllBuff(
            BuffSource.Trait, Id);
        GM.Root.BattleMgr.battleData.BattleTempBuffMgr.Add(tempBuff);
    }
    
    public void RemoveEffect() {}
    public void OnClutterSearchResolved(){}
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnBattleStart;
    TraitData _data;
    public TraitData Data => _data ??= new TraitData(Id);
}

//渐强式魔咒
public class Trait_ProgressiveSpell : IItemSynergies
{
    public int Id => 4;
    public string Name => "渐强式魔咒";
    string cacheKey => $"渐强式魔咒-{Id}";
    public string Description => "第三颗子弹伤害+1，第四颗子弹伤害+2";
    List<BulletData> _bullets => GM.Root.InventoryMgr._BulletInvData.EquipBullets;

    public bool Match(List<ItemData> equippedItems) =>
        equippedItems.Any(i => i.ID == 2) &&
        equippedItems.Any(i => i.ID == 6) &&
        equippedItems.Any(i => i.ID == 7);

    public void ApplyEffect(BattleContext ctx)
    {
        for(int i = 0; i < _bullets.Count; i++)
        {
            if (i == 2)
                _bullets[i].ModifierDamageAdditionDict[cacheKey] = 1;
            else if (i == 3)
                _bullets[i].ModifierDamageAdditionDict[cacheKey] = 2;
            else
                _bullets[i].ModifierDamageAdditionDict[cacheKey] = 0;
            
            _bullets[i].SyncFinalAttributes();
        }
    }

    public void RemoveEffect()
    {
        foreach (var each in _bullets)
        {
            each.ModifierDamageAdditionDict.Remove(cacheKey);
            each.SyncFinalAttributes();
        }
    }
    public void OnClutterSearchResolved(){}
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnAlltimes;
    TraitData _data;
    public TraitData Data => _data ??= new TraitData(Id);
}

//三阶混念术
public class Trait_ThirdResonanceChaos : IItemSynergies
{
    public int Id => 5;
    public string Name => "三阶混念术";
    string cacheKey => $"三阶混念术-{Id}";
    public string Description => "你的所有子弹的共振效果+1";
    List<BulletData> _bullets => GM.Root.InventoryMgr._BulletInvData.EquipBullets;
    
    public bool Match(List<ItemData> equippedItems) =>
        equippedItems.Any(i => i.ID == 4) && //晕头转向的猫头鹰雕像
        equippedItems.Any(i => i.ID == 8) && //变形术练习笔
        equippedItems.Any(i => i.ID == 10);  //发霉训练日志

    public void ApplyEffect(BattleContext ctx)
    {
        foreach (BulletData bullet in _bullets)
        {
            bullet.ModifierGemResonanceAdditionDict[cacheKey] = 1;
            bullet.SyncFinalAttributes();
        }
        //Debug.Log("[三阶混念术] 所有子弹共振+1 已应用");
    }

    public void RemoveEffect()
    {
        foreach (var bullet in _bullets)
        {
            bullet.ModifierGemResonanceAdditionDict.Remove(cacheKey);
            bullet.SyncFinalAttributes();
        }
        //Debug.Log("[三阶混念术] 移除效果");
    }
    public void OnClutterSearchResolved(){}
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnAlltimes;
    TraitData _data;
    public TraitData Data => _data ??= new TraitData(Id);
}

//无限金币术
public class Trait_UnlimitedCoin : IItemSynergies
{
    public int Id => 6;
    public string Name => "无限金币术"; 
    public string Description => "你每翻找一次物品时，额外获得#Yellow(2~10)#金币。";
    public bool Match(List<ItemData> equippedItems) =>
        equippedItems.Any(i => i.ID == 4) &&  // 猫头鹰雕像
        equippedItems.Any(i => i.ID == 5) &&  // 黄金钥匙
        equippedItems.Any(i => i.ID == 7);    // 幸运靴

    public void ApplyEffect(BattleContext ctx) {}
    
    //全图事件在这里处理
    public void OnClutterSearchResolved()
    {
        int coinGain = Random.Range(2, 11); // [2,10] inclusive
        PlayerManager.Instance._PlayerData.ModifyCoins(coinGain);
    
        FloatingTextFactory.CreateWorldText($"无限金币术 +{coinGain}",
            default, FloatingTextType.MapHint, Color.yellow, 2.5f);
    }
    public void RemoveEffect() {}
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.Passive;
    TraitData _data;
    public TraitData Data => _data ??= new TraitData(Id);
}

public class Trait_TrashExplosion : IItemSynergies
{
    #region 特质信息
    public int Id => 7;
    public string Name => "垃圾之光";
    string cacheKey => $"垃圾之光-{Id}";
    public string Description => "如果你有至少两颗子弹未镶嵌宝石，则最后一颗子弹伤害+4，穿透+1";
    List<BulletData> _bullets => GM.Root.InventoryMgr._BulletInvData.EquipBullets;
    #endregion

    #region 核心实现
    public void ApplyEffect(BattleContext ctx)
    {
        if (_bullets == null || _bullets.Count == 0)
            return;

        // 统计未镶嵌宝石的子弹
        int emptyCount = _bullets.Count(b => b.Modifiers.All(m => m is not BulletModifierGem));

        foreach (var bullet in _bullets)
        {
            bullet.ModifierDamageAdditionDict[cacheKey] = 0; // 清理老状态
            bullet.ModifierPiercingAdditionDict[cacheKey] = 0; // 清理老状态
            bullet.SyncFinalAttributes();
        }

        if (emptyCount >= 2)
        {
            BulletData lastBullet = _bullets.Last();
            lastBullet.ModifierDamageAdditionDict[cacheKey] = 4; // 伤害+4
            lastBullet.ModifierPiercingAdditionDict[cacheKey] = 1; // 穿透+1
            lastBullet.SyncFinalAttributes();
        }
    }
    public void RemoveEffect()
    {
        foreach (var bullet in _bullets)
        {
            bullet.ModifierDamageAdditionDict.Remove(cacheKey); 
            bullet.ModifierGemPiercingAdditionDict.Remove(cacheKey);
            bullet.SyncFinalAttributes();
        }
    }
    public void OnClutterSearchResolved(){}
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnAlltimes;
    #endregion

    #region 绑定信息
    public bool Match(List<ItemData> equippedItems) =>
        equippedItems.Any(i => i.ID == 1) &&  // 蹩脚的魔术师帽子
        equippedItems.Any(i => i.ID == 9) &&  // 碎裂的玩具魔杖 
        equippedItems.Any(i => i.ID == 10);    // 发霉的训练日志
    TraitData _data;
    public TraitData Data => _data ??= new TraitData(Id);
    #endregion
}

public class Trait_ScreamingOwl : IItemSynergies
{
    #region 特质信息
    public int Id => 8;
    public string Name => "尖叫猫头鹰";
    public string Description => "战斗开始时，敌人护盾血量减少1~2。";
    #endregion
    
    #region 核心实现
    public void ApplyEffect(BattleContext ctx)
    {
        if (ctx.CurEnemy == null || ctx.CurEnemy.Shields == null) return;
        
        List<ShieldData> shieldData = ctx.CurEnemy.Shields;
        foreach (var shield in shieldData)  
        {
            int randomChange = Random.Range(1, 3); // [1,2]
            shield.ModifyHP(-randomChange);
            //Debug.Log($"[尖叫猫头鹰] 触发，护盾血量减少{randomChange}");
        }
    }
    public void RemoveEffect() {}
    public void OnClutterSearchResolved(){}
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnBattleStart;
    #endregion
    
    #region 绑定信息
    public bool Match(List<ItemData> equippedItems) =>
        equippedItems.Any(i => i.ID == 2) &&   // 尖叫陶罐
        equippedItems.Any(i => i.ID == 4) &&   // 晕头转向的猫头鹰雕像
        equippedItems.Any(i => i.ID == 204);   // 翻找术手册
    TraitData _data;
    public TraitData Data => _data ??= new TraitData(Id);
    #endregion
}


public class Trait_RainbowResonantTrap : IItemSynergies
{
    #region 特质信息
    public int Id => 9;
    public string Name => "虹彩混响陷阱";
    string cacheKey => $"虹彩混响陷阱-{Id}";
    public string Description => "所有子弹伤害-1";
    List<BulletData> _bullets => GM.Root.InventoryMgr._BulletInvData.EquipBullets;
    public ItemTriggerTiming TriggerTiming => ItemTriggerTiming.OnAlltimes;
    #endregion

    #region 核心实现
    public void ApplyEffect(BattleContext ctx)
    {
        foreach (var bullet in _bullets)
        {
            bullet.ModifierDamageAdditionDict[cacheKey] = -1; // 伤害-1
            bullet.SyncFinalAttributes();
        }
    }

    public void RemoveEffect()
    {
        foreach (var bullet in _bullets)
        {
            bullet.ModifierDamageAdditionDict[cacheKey] = 0; // 清理老状态
            bullet.SyncFinalAttributes();
        }
    }
    public void OnClutterSearchResolved(){}
    #endregion
    
    #region 绑定信息
    public bool Match(List<ItemData> equippedItems) =>
        equippedItems.Any(i => i.ID == 200) &&  // 写满错字的魔法书
        equippedItems.Any(i => i.ID == 300) &&  // 黏糊糊的巫师手套
        equippedItems.Any(i => i.ID == 401);    // 虹彩的魔法书
    TraitData _data;
    public TraitData Data => _data ??= new TraitData(Id);
    #endregion
}

