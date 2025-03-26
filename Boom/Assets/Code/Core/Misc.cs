using System;
using System.Collections.Generic;
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

#region ToolTips相
public struct ToolTipsInfo
{
    public string Name;
    public int Level;
    public List<ToolTipsAttriSingleInfo> AttriInfos;

    public ToolTipsInfo(string name = "", int level = 0,
        List<ToolTipsAttriSingleInfo> attriInfos = null)
    {
        Name = name;
        Level = level;
        AttriInfos = attriInfos ?? new List<ToolTipsAttriSingleInfo>();
    }
}

public struct ToolTipsAttriSingleInfo
{
    public ToolTipsAttriType Type;
    public int OriginValue;
    public int AddedValue;
    public ElementalTypes ElementType;
    public ToolTipsAttriSingleInfo(ToolTipsAttriType type = default, 
        int originValue = 0, int addedValue = 0,ElementalTypes elementType = default)
    {
        Type = type;
        OriginValue = originValue;
        AddedValue = addedValue;
        ElementType = elementType;
    }
}

public enum ToolTipsAttriType
{
    Damage = 0,
    Piercing = 1,
    Resonance = 2,
    Element = 10,
}

public enum ToolTipsMenuState
{
    Normal = 0,
    RightClick = 1,
    Locked = 2
}
#endregion

#region 战斗相关
public struct BattleOnceHit
{
    public int HitIndex;
    public int ShieldIndex;
    public int EnmyIndex;
    public int EffectiveDamage;
    public int OverflowDamage;
    public int Damage;
    public bool IsDestroyed;

    public BattleOnceHit(int _hitIndex = -1,int _shieldIndex = -1,
        int _enmyIndex = -1,int _effectiveDamage = 0,
        int _overflowDamage = 0,int _damage = 0,bool _isDestroyed = false)
    {
        HitIndex = _hitIndex;
        ShieldIndex = _shieldIndex;
        EnmyIndex = _enmyIndex;
        EffectiveDamage = _effectiveDamage;
        OverflowDamage = _overflowDamage;
        Damage = _damage;
        IsDestroyed = _isDestroyed;
    }
}

public class WarReport
{
    public int CurWarIndex;
    public bool IsWin;
    public int TotalDamage;
    public int EffectiveDamage;
    public int OverFlowDamage;
    public Dictionary<int,SingelBattleInfo> WarIndexToBattleInfo;

    public WarReport()
    {
        CurWarIndex = 0;
        IsWin = false;
        WarIndexToBattleInfo = new Dictionary<int, SingelBattleInfo>();
    }
    
    public SingelBattleInfo GetCurBattleInfo()
    {
        return WarIndexToBattleInfo[CurWarIndex];
    }
}

public class SingelBattleInfo
{
    public Dictionary<int, KeyValuePair<BulletData, List<BattleOnceHit>>> InfoDict;
    public SingelBattleInfo()
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

//Slot的类型
public enum SlotType
{
    SpawnnerSlot = 0,
    BagItemSlot = 1,
    BulletSlot = 2,
    BagEquipSlot = 3,
    GemBagSlot = 4,
    GemInlaySlot = 5,
    CurBulletSlot = 6,
    BulletInnerSlot = 7
}

public enum SceneState
{
    StartGame = 0,
    MainEnv = 1,
    MapScene = 2,
    LoadingScene = 3
}