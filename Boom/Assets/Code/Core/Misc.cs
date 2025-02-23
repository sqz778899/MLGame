﻿using System;
using System.Collections.Generic;
using TMPro;

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

public enum ShopType
{
    All = 0,
    GemShop = 1,
    BulletShop = 2
}

public enum UILockedState
{
    isNormal = 0,
    isLocked = 1,
    isSelected = 2
}

[Serializable]
public class Award
{
    public int score;
    public int gold;
    public SupremeCharm supremeCharm;
    public List<int> Items;
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
    BagSlot = 1,
    BulletSlot = 2,
    ElementSlot = 3,
    GemBagSlot = 4,
    GemInlaySlot = 5,
    CurBulletSlot = 6
}