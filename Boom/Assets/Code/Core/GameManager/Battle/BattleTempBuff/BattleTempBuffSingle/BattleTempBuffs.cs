using System.Collections.Generic;
using UnityEngine;

//改变宝石类型的Buff
public class ChangeGemTypeBuff : IBattleTempBuff
{
    public BuffSource Source { get; private set; }
    public int SourceID { get; private set; }
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

//忽略宝石修改的Buff
public class IgnoreGemBuff : IBattleTempBuff
{
    public BuffSource Source { get; private set; }
    public int SourceID { get; private set; }
    List<BulletData> bullets => GM.Root.InventoryMgr._BulletInvData.EquipBullets;

    public IgnoreGemBuff(BuffSource source, int id)
    {
        Source = source;
        SourceID = id;
    }

    public void Apply(BulletData bullet) {}

    public void ApplyMacro()
    {
        int lastIndex = bullets.Count - 1;
        for (int i = 0; i < bullets.Count; i++)
        {
            if (i!=lastIndex)
                bullets[i].IgnoreGemModifier = false;
            else
                bullets[i].IgnoreGemModifier = true;
        }
    }
    
    public void RemoveBuff() => bullets.ForEach(b => b.IgnoreGemModifier = false);
    
    public string GetUniqueKey() => $"{Source}-{SourceID}-IgnoreGem";
}

//最后一颗子弹伤害加成
public class LastBulletDamageBuff : IBattleTempBuff
{
    public BuffSource Source { get; private set; }
    public int SourceID { get; private set; }

    List<BulletData> bullets => GM.Root.InventoryMgr._BulletInvData.EquipBullets;

    public LastBulletDamageBuff(BuffSource source, int id)
    {
        Source = source;
        SourceID = id;
    }

    public void Apply(BulletData bullet) {}

    public void ApplyMacro()
    {
        if (bullets.Count == 0) return;

        BulletData last = bullets[^1]; // C# 的 ^1 是倒数第一个
        Debug.Log(last.OrderInRound);
        StatBuff realBuff = new StatBuff(
            BulletStatType.Damage,
            new List<KeyValuePair<int, int>> { new(last.OrderInRound, 2) },
            Source, SourceID
        );
        // 注意这里是 Add 而不是 Apply！
        GM.Root.BattleMgr.battleData.BattleTempBuffMgr.Add(realBuff);
    }

    public void RemoveBuff() { }

    public string GetUniqueKey() => $"{Source}-{SourceID}-LastBulletDmg";
}


#region 专门处理伤害，穿透，共振增减的Buff
public class StatBuff : IBattleTempBuff
{
    public BuffSource Source { get; private set; }
    public int SourceID { get; private set; }
    public BulletStatType StatType { get; private set; }

    private List<KeyValuePair<int, int>> targetList;
    private List<BulletData> bullets => GM.Root.InventoryMgr._BulletInvData.EquipBullets;

    public StatBuff(BulletStatType statType, IEnumerable<KeyValuePair<int, int>> targets, BuffSource source, int id)
    {
        StatType = statType;
        Source = source;
        SourceID = id;
        targetList = new List<KeyValuePair<int, int>>(targets);
    }

    public void Apply(BulletData bullet)
    {
        foreach (var kvp in targetList)
        {
            if (bullet.OrderInRound == kvp.Key)
            {
                switch (StatType)
                {
                    case BulletStatType.Damage:
                        bullet.FinalDamage += kvp.Value;
                        break;
                    case BulletStatType.Piercing:
                        bullet.FinalPiercing += kvp.Value;
                        break;
                    case BulletStatType.Resonance:
                        bullet.FinalResonance += kvp.Value;
                        break;
                }
            }
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

