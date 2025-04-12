using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.Serialization;


[Serializable]
public struct Award
{
    public int BaseScore;
    public int Coin;
    public List<int> SupremeCharms;
    public List<int> Items;

    public Award(int baseScore = 0,int _coin = 0,List<int> _supremeCharms = null,List<int> _items = null)
    {
        BaseScore = baseScore;
        Coin = _coin;
        SupremeCharms = _supremeCharms ?? new List<int>();
        Items = _items ?? new List<int>();
    }
}


#region 战斗相关
public struct BattleOnceHit
{
    public int HitIndex;
    public int ShieldIndex;
    public int EnemyIndex;
    public int EffectiveDamage;
    public int OverflowDamage;
    public int Damage;
    public bool IsDestroyed;

    public BattleOnceHit(int hitIndex = -1, int shieldIndex = -1, int enemyIndex = -1,
        int effectiveDamage = 0, int overflowDamage = 0, int damage = 0, bool isDestroyed = false)
    {
        HitIndex = hitIndex;
        ShieldIndex = shieldIndex;
        EnemyIndex = enemyIndex;
        EffectiveDamage = effectiveDamage;
        OverflowDamage = overflowDamage;
        Damage = damage;
        IsDestroyed = isDestroyed;
    }
}

// 单个子弹在一场战斗中的全部表现
public class BulletAttackRecord
{
    public BulletData BulletData;                // 攻击时使用的子弹数据
    public int BulletSlotID;                     // 子弹槽位索引
    public List<BattleOnceHit> Hits;             // 每次Hit记录

    public BulletAttackRecord(BulletData bulletData, int bulletSlotID)
    {
        BulletData = bulletData;
        BulletSlotID = bulletSlotID;
        Hits = new List<BattleOnceHit>();
    }
    public void RecordHit(BattleOnceHit hit) => Hits.Add(hit);
}

// 单场战斗信息（一个战斗回合）
public class SingleBattleReport
{
    public List<BulletAttackRecord> BulletAttackRecords = new List<BulletAttackRecord>();

    // 快速查找当前子弹的记录（按SlotID）
    public BulletAttackRecord GetOrCreateBulletRecord(BulletData bulletData)
    {
        int slotID = bulletData.CurSlotController.SlotID;
        var record = BulletAttackRecords.FirstOrDefault(r => r.BulletSlotID == slotID);
        if (record == null)
        {
            record = new BulletAttackRecord(bulletData, slotID);
            BulletAttackRecords.Add(record);
        }
        return record;
    }
}

public class WarReport
{
    public int CurWarIndex;
    public bool IsWin;
    public int TotalDamage;
    public int EffectiveDamage;
    public int OverFlowDamage;
    public Dictionary<int,SingelBattleInfoOld> WarIndexToBattleInfoOld;
    public Dictionary<int, SingleBattleReport> WarIndexToBattleInfo;

    public WarReport()
    {
        CurWarIndex = 0;
        IsWin = false;
        WarIndexToBattleInfoOld = new Dictionary<int, SingelBattleInfoOld>();
        WarIndexToBattleInfo = new Dictionary<int, SingleBattleReport>();
    }
    
    public SingleBattleReport GetCurBattleInfo()
    {
        if (!WarIndexToBattleInfo.TryGetValue(CurWarIndex, out var report))
        {
            report = new SingleBattleReport();
            WarIndexToBattleInfo[CurWarIndex] = report;
        }
        return report;
    }
    
    public SingelBattleInfoOld GetCurBattleInfoOld()
    {
        return WarIndexToBattleInfoOld[CurWarIndex];
    }
}

public class SingelBattleInfoOld
{
    public Dictionary<int, KeyValuePair<BulletData, List<BattleOnceHit>>> InfoDict;
    public SingelBattleInfoOld()
    {
        InfoDict = new Dictionary<int, KeyValuePair<BulletData, List<BattleOnceHit>>>();
    }
}
#endregion

#region 多语言相关
public enum MultiLaEN
{
    English = 0,
    ZH_Simplified = 1,
    ZH_Traditional = 2,
    Japanese = 3,
    Korean = 4,
}

public class MStr
{
    public string Str;
    public float FondSize;
    public TMP_FontAsset FondAsset;

    public MStr(string _str,float _fondSize,TMP_FontAsset _fondAsset)
    {
        Str = _str;
        FondSize = _fondSize;
        FondAsset = _fondAsset;
    }
}

public class MultiLaJson
{
    public List<string> English;
    public Dictionary<string, string> ZH_Simplified;
    public Dictionary<string, string> ZH_Traditional;
    public Dictionary<string, string> Japanese;
    public Dictionary<string, string> Korean;

    public MultiLaJson()
    {
        English = new List<string>();
        ZH_Simplified = new Dictionary<string, string>();
        ZH_Traditional = new Dictionary<string, string>();
        Japanese = new Dictionary<string, string>();
        Korean = new Dictionary<string, string>();
    }
}
#endregion

#region PlayerSetting
public enum ScreenRes
{
    Set3840_2160 = 0,
    Set2560_1440 = 1,
    Set1920_1080 = 2,
    Set1366_768 = 3
}

#endregion
//score
//gold
//insignias

public enum TalentType
{
    Normal = 0,
    Resource = 1,
    Gem = 2,
    Bullet = 3,
}
public enum ShopType
{
    All = 0,
    GemShop = 1,
    BulletShop = 2
}

public enum CreateItemType
{
    ShopGem = 0,
    MiniBagGem = 1,
    TempGem = 2,
}


public enum UILockedState
{
    isNormal = 0,
    isLocked = 1,
    isSelected = 2
}

public class ElementState
{
    //元素均衡
    public bool ELBalance;
    public int ELBalenceLV;
    //元素沸腾
    public bool ELEbullitionWater;
    public bool ELEbullitionWaterLV;
    //元素精纯
    public bool ELPureWater;
    public bool ELPureWaterLV;
}

[Serializable]
public class SupremeCharm
{
    public int ID;
    public string name;
    
    public int damage;
    public ElementalTypes elementalTypes;

    public void GetSupremeCharmByID()
    {
        
    }

    public SupremeCharm(int _id)
    {
        ID = _id;
    }
}

public enum SceneState
{
    StartGame = 0,
    MainEnv = 1,
    MapScene = 2,
    LoadingScene = 3
}