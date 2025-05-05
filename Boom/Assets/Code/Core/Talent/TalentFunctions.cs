using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TalentFunctions
{
    static readonly Dictionary<TalentEffectType, ITalentUnlockStrategy> talentUnlockStrategies = new Dictionary<TalentEffectType, ITalentUnlockStrategy>
    {
        {TalentEffectType.UnlockBulletSlot, new UnlockBulletSlotStrategy()},
        {TalentEffectType.CarryGem, new CarryGemTalentStrategy() },
        {TalentEffectType.CarryBullet, new CarryBulletTalentStrategy() },
        {TalentEffectType.CarryRoomKey, new CarryRoomKeyTalentStrategy() },
        {TalentEffectType.GemBonus, new AddGemBonusStrategy() },
        {TalentEffectType.ScoreToDustBonus, new ScoreToDustBonusStrategy() },
        {TalentEffectType.CoinToDustBonus, new CoinToDustBonusStrategy() },
        {TalentEffectType.ResBonus, new ResBonusStrategy() },
        {TalentEffectType.BulletResBonus, new BulletResBonusStrategy() },
        {TalentEffectType.UnlockGem, new UnlockGemStrategy() },
        {TalentEffectType.CarryGemLevelBonus, new CarryGemLevelBonusStrategy() },
    };

    public static void LearnTalent(int talentID)
    {
        TalentJson tjson = TrunkManager.Instance.GetTalentJson(talentID);
        if (talentUnlockStrategies.TryGetValue(tjson.TalentType, out var strategy))
        {
            strategy.Learn(tjson);
        }
    }
}

#region 具体的天赋实现
// 解锁子弹槽
public class UnlockBulletSlotStrategy : ITalentUnlockStrategy
{
    public void Learn(TalentJson talent) =>
        GM.Root.PlayerMgr._PlayerData.SetBulletSlotLockedState(talent.EffectID,true);
}
// 携带宝石
public class CarryGemTalentStrategy : ITalentUnlockStrategy
{
    public void Learn(TalentJson talent) =>
        GM.Root.InventoryMgr.AddGemToBag(talent.EffectID);
}
// 携带子弹
public class CarryBulletTalentStrategy : ITalentUnlockStrategy
{
    public void Learn(TalentJson talent)=>
        GM.Root.InventoryMgr._BulletInvData.AddSpawner(talent.EffectID);
}
// 携带钥匙
public class CarryRoomKeyTalentStrategy : ITalentUnlockStrategy
{
    public void Learn(TalentJson talent) => 
        GM.Root.PlayerMgr._PlayerData.ModifyRoomKeys(talent.EffectValue);
}
// 宝石加成
public class AddGemBonusStrategy : ITalentUnlockStrategy
{
    public void Learn(TalentJson talent) =>
        GM.Root.PlayerMgr._PlayerData.TalentGemBonuses
            .Add(new TalentGemBonus(talent.EffectID, talent.EffectValue));
}
//局内资源加成
public class ResBonusStrategy : ITalentUnlockStrategy
{
    public void Learn(TalentJson talent) =>
        GM.Root.PlayerMgr._PlayerData.CoinAdd += talent.EffectValue;
}
//分数转魔尘能力
public class ScoreToDustBonusStrategy : ITalentUnlockStrategy
{
    public void Learn(TalentJson talent) =>
        GM.Root.PlayerMgr._PlayerData.ScoreToDustRate += (float)talent.EffectValue/100;
}
//分数转魔尘能力
public class CoinToDustBonusStrategy : ITalentUnlockStrategy
{
    public void Learn(TalentJson talent) =>
        GM.Root.PlayerMgr._PlayerData.CoinToDustRate += talent.EffectValue;
}
// 子弹资源加成
public class BulletResBonusStrategy : ITalentUnlockStrategy
{
    public void Learn(TalentJson talent) {}
}
// 解锁宝石
public class UnlockGemStrategy : ITalentUnlockStrategy
{
    public void Learn(TalentJson talent) {}
}
//携带宝石等级加成
public class CarryGemLevelBonusStrategy : ITalentUnlockStrategy
{
    public void Learn(TalentJson talent) {}
}
#endregion