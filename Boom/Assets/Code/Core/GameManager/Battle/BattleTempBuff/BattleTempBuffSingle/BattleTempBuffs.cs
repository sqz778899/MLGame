using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//改变宝石类型的Buff
public class ChangeGemTypeBuff : IBattleTempBuff
{
    public BuffSource Source { get; private set; }
    public int SourceID { get; private set; }
    public BuffBehavior SepicialBehavior { get; }
    List<GemData> curGems =>  GM.Root.InventoryMgr._InventoryData.EquipGems;
    public ChangeGemTypeBuff(BuffSource source, int id)
    {
        Source = source;
        SourceID = id;
    }
    public void Apply(BulletData bullet) {}

    public void ApplyMacro()
    {
        List<GemData> filterGemDatas = new List<GemData>();
        string buffUniqueKey = GetUniqueKey();
        //如果已经触发过则不再触发
        foreach (GemData gem in curGems)
        {
            if(gem.IsTriggered(buffUniqueKey)) return;
        }
        // 过滤掉共振宝石
        foreach (GemData gem in curGems)
        {
            if (gem.CurGemType == GemType.Resonance) continue;
            filterGemDatas.Add(gem);
        }
        if (filterGemDatas.Count == 0) return;
        // 随机选择一个宝石
        GemData selGem = filterGemDatas[Random.Range(0, filterGemDatas.Count)];
        selGem.ChangeGemType(GemType.Resonance,buffUniqueKey);
        
        GM.Root.InventoryMgr._BulletInvData.RefreshModifiers();
    }

    public void RemoveBuff() => curGems.ForEach(g=> g.ReturnGemType());
    public string GetUniqueKey() => $"{Source}-{SourceID}-ChangeGemType";
}

public class ChangeGemTypeAllBuff : IBattleTempBuff
{
    public BuffSource Source { get; private set; }
    public int SourceID { get; private set; }
    public BuffBehavior SepicialBehavior { get; }
    List<GemData> curGems =>  GM.Root.InventoryMgr._InventoryData.EquipGems;
    public ChangeGemTypeAllBuff(BuffSource source, int id)
    {
        Source = source;
        SourceID = id;
    }
    public void Apply(BulletData bullet) {}

    public void ApplyMacro()
    {
        List<GemData> filterGemDatas = new List<GemData>();
        string buffUniqueKey = GetUniqueKey();
        // 过滤掉共振宝石
        foreach (GemData gem in curGems)
        {
            if (gem.CurGemType == GemType.Resonance) continue;
            if(gem.IsTriggered(buffUniqueKey)) continue;
            filterGemDatas.Add(gem);
        }
        if (filterGemDatas.Count == 0) return;
        // 随机选择一个宝石
        foreach (var each in filterGemDatas)
            each.ChangeGemType(GemType.Resonance,buffUniqueKey);
        
        GM.Root.InventoryMgr._BulletInvData.RefreshModifiers();
    }

    public void RemoveBuff() => curGems.ForEach(g=> g.ReturnGemType());
    public string GetUniqueKey() => $"{Source}-{SourceID}-ChangeGemTypeAll";
}


//忽略宝石修改的Buff
public class IgnoreGemBuff : IBattleTempBuff
{
    public BuffSource Source { get; private set; }
    public int SourceID { get; private set; }
    
    public BuffBehavior SepicialBehavior { get; }
    List<BulletData> bullets => GM.Root.InventoryMgr._BulletInvData.EquipBullets;

    public IgnoreGemBuff(BuffSource source, int id)
    {
        Source = source;
        SourceID = id;
    }

    public void Apply(BulletData bullet) {}

    public void ApplyMacro()
    {
        foreach (BulletData each in bullets)
        {
            if (each.IsLastBullet == false)
                each.GemEffectOverride = GemEffectOverrideState.None;
            else
                each.GemEffectOverride = GemEffectOverrideState.Ignore;
        }
    }
    
    public void RemoveBuff() => bullets.ForEach(b => b.GemEffectOverride = GemEffectOverrideState.None);
    
    public string GetUniqueKey() => $"{Source}-{SourceID}-IgnoreGem";
}


#region 专门处理伤害，穿透，共振增减的Buff
public class StatBuff : IBattleTempBuff
{
    public BuffSource Source { get; private set; }
    public int SourceID { get; private set; }
    
    public BuffBehavior SepicialBehavior { get; }
    public BulletStatType StatType { get; private set; }

    private List<KeyValuePair<int, int>> targetList;
    private List<BulletData> bullets => GM.Root.InventoryMgr._BulletInvData.EquipBullets;

    public StatBuff(BulletStatType statType, IEnumerable<KeyValuePair<int, int>> targets,
        BuffSource source, int id,BuffBehavior _sepicialBehavior = BuffBehavior.None)
    {
        StatType = statType;
        Source = source;
        SourceID = id;
        SepicialBehavior = _sepicialBehavior;
        targetList = new List<KeyValuePair<int, int>>(targets);
    }

    public void Apply(BulletData bullet)
    {
        // 特殊行为：最后一颗子弹
        if (SepicialBehavior == BuffBehavior.LastBullet && bullet.IsLastBullet)
        {
            ApplyToBullet(bullet, targetList.FirstOrDefault().Value);
            return;
        }

        // 常规按索引处理
        foreach (var kvp in targetList)
        {
            if (bullet.OrderInRound == kvp.Key)
                ApplyToBullet(bullet, kvp.Value);
        }
    }
    
    void ApplyToBullet(BulletData bullet, int value)
    {
        switch (StatType)
        {
            case BulletStatType.Damage: bullet.FinalDamage += value; break;
            case BulletStatType.Piercing: bullet.FinalPiercing += value; break;
            case BulletStatType.Resonance: bullet.FinalResonance += value; break;
        }
    }

    public void ApplyMacro() { }

    public void RemoveBuff() => bullets.ForEach(b => b.ClearTempBuffs());

    public string GetUniqueKey() => $"{Source}-{SourceID}-Stat-{StatType}";
}

public enum BulletStatType
{
    Damage,
    Piercing,
    Resonance
}
#endregion

