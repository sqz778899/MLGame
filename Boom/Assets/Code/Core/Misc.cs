using System;
using System.Collections.Generic;
using TMPro;

public enum TipTypes
{
    Bullet = 1,
    Item = 2,
}

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
public class ItemAttribute
{
    public int waterElement;
    public int fireElement;
    public int thunderElement;
    public int lightElement;
    public int darkElement;

    public int extraWaterDamage;
    public int extraFireDamage;
    public int extraThunderDamage;
    public int extraLightDamage;
    public int extraDarkDamage;
    
    public int maxDamage;

    public ItemAttribute()
    {
        waterElement = 0;
        fireElement = 0;
        thunderElement = 0;
        lightElement = 0;
        darkElement = 0;
        
        //
        extraWaterDamage = 0;
        extraFireDamage = 0;
        extraThunderDamage = 0;
        extraLightDamage = 0;
        extraDarkDamage = 0;
        maxDamage = 0;
    }
}

[Serializable]
public class Item
{
    public int ID;
    public int rare;
    public string name;
    public string resAllPath;

    public ItemAttribute attribute;
    //游戏内相关属性
    public int slotID;
    public int slotType;
    
    void InitItemDataByID()
    {
        attribute = new ItemAttribute();
        List<ItemJson> itemDesignJsons = TrunkManager.Instance.ItemDesignJsons;
        foreach (var each in itemDesignJsons)
        {
            if (each.ID == ID)
            {
                rare = each.rare;
                name = each.name;
                resAllPath = PathConfig.ItemImageDir + each.resName + ".png";
                attribute.waterElement = each.attribute.waterElement;
                attribute.fireElement = each.attribute.fireElement;
                attribute.thunderElement = each.attribute.thunderElement;
                attribute.lightElement = each.attribute.lightElement;
                attribute.darkElement = each.attribute.darkElement;
                //
                attribute.extraWaterDamage = each.attribute.extraWaterDamage;
                attribute.extraFireDamage = each.attribute.extraFireDamage;
                attribute.extraThunderDamage = each.attribute.extraThunderDamage;
                attribute.extraLightDamage = each.attribute.extraLightDamage;
                attribute.extraDarkDamage = each.attribute.extraDarkDamage;
                attribute.maxDamage = each.attribute.maxDamage;
            }
        }
    }
    public Item(int _id)
    {
        ID = _id;
        InitItemDataByID();
    }
}

public class ItemJson
{
    public int ID;
    public int rare;
    public string name;
    public string resName;
    
    public ItemAttribute attribute;
    
    public ItemJson()
    {
        ID = -1;
        rare = -1;
        name = "";
        resName = "";
        attribute = new ItemAttribute();
    }
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
}