﻿using System;
using System.Collections.Generic;


public interface ISaveable
{
    public ItemBaseSaveData ToSaveData();
}

#region 存档结构
[Serializable]
public class SaveFileJson
{
    public int MaxHP;
    public int HP;
    public int Score;                //分数
    public int Coins;                //硬币数量
    public int RoomKeys;             //钥匙数量
    public List<BulletBaseSaveData> UserBulletSpawner;  //子弹孵化器
    public List<BulletBaseSaveData> UserCurBullets;     //当前子弹的状态
    public List<StandbyData> UserStandbyBullet; //子弹材料的状态
    public Dictionary<int, bool> UserBulletSlotLockedState;  //用户子弹槽的锁定状态
    
    public List<ItemSaveData> UserItems;          //用户道具
    public List<GemBaseSaveData> UserGems;        //用户宝石
    public List<MapSate> UserMapSate;         //地图状态
    public List<QuestSaveData> UserQuests;          //用户任务

    public SaveFileJson()
    {
        MaxHP = 3;
        HP = 3;
        Score = 0;
        Coins = 0;
        RoomKeys = 0;
        UserBulletSpawner = new List<BulletBaseSaveData>();
        UserCurBullets = new List<BulletBaseSaveData>();
        UserStandbyBullet = new List<StandbyData>();
        UserBulletSlotLockedState = new Dictionary<int, bool>();
        UserItems = new List<ItemSaveData>();
        UserGems = new List<GemBaseSaveData>();
        UserMapSate = new List<MapSate>();
        UserQuests = new List<QuestSaveData>();
    }
}

public class UserConfig
{
    public int UserLanguage;
    public int UserScreenResolution;
    public int UserScreenMode;

    public UserConfig()
    {
        UserLanguage = 0;
        UserScreenResolution = 2;
        UserScreenMode = 1;
    }
}
#endregion

#region 存储策划数据结构
// 统一 Json 数据基类
public abstract class ItemJsonBase
{
    public int ID;
    public string Name;
    public int Price;
}

//子弹的策划数据存储结构
[Serializable]
public class BulletJson : ItemJsonBase
{
    public int Level;
    public int ElementalType;
    
    //基础属性
    public int Damage;
    public int Piercing;
    public int Resonance;
    //资产相关
    public string HitEffectName;

    public BulletJson(int _id = -1, string _name = "", int _level = 1,int _elementalType = -1,
        int _damage = 0, int _piercing = 0, int _resonance = 0,string _hitEffectName = "")
    {
        ID = _id;
        Name = _name;
        Level = _level;
        ElementalType = _elementalType;
        Damage = _damage;
        Piercing = _piercing;
        Resonance = _resonance;
        Price = 0;
        HitEffectName = _hitEffectName;
    }
}


[Serializable]
public class GemJson : ItemJsonBase
{
    public int Level;
    public string ImageName;
    
    public int Damage;
    public int Piercing;
    public int Resonance;
    
    public GemJson(int id = -1,
        string name = "", int _level = 1,string imageName = "")
    {
        ID = id;
        Name = name;
        Level = _level;
        ImageName = imageName;
        Price = 0;
    }
}

[Serializable]
public class ItemJson:ItemJsonBase
{
    public int Rare;
    public string ImageName;
    public ItemAttribute Attribute;
    
    public ItemJson(int _id = -1, int _instanceID = -1,
        int _rare = -1, string _name = "", 
        string _imageName = "", ItemAttribute _attribute = null,
        int _SlotID = 0, int _SlotType = 0)
    {
        ID = _id;
        Rare = _rare;
        Name = _name;
        ImageName = _imageName;
        if (_attribute == null)
            _attribute = new ItemAttribute();
        Attribute = _attribute; ;
        Price = 0;
    }
}

[Serializable]
public class QuestJson
{
    public int ID;                   // 唯一ID
    public string Name;              // 任务名称
    public int Level;                // 任务等级
    public string Description;       // 任务描述

    public QuestJson()
    {
        ID = -1;
        Name = "";
        Level = -1;
        Description = "";
    }
}
#endregion

#region 游戏内存档数据结构
[Serializable]
public class ItemBaseSaveData
{
    public int ID;          // 配表ID
    public int SlotID;      // 槽位ID
    public SlotType SlotType;

    public ItemBaseSaveData() {}// 让无参构造也保留，以免 JsonUtility/序列化报错
    public ItemBaseSaveData(ItemData item)
    {
        ID = item.ID;
        SlotID = item.CurSlot.SlotID;
        SlotType = item.CurSlot.SlotType;
    }
}

[Serializable]
public class BulletBaseSaveData:ItemBaseSaveData
{
    public int SpawnerCount;
    public BulletBaseSaveData(BulletData data)
    {
        ID = data.ID; //有了ID，其他静态数据通过配表索引出来
        SlotID = data.CurSlot.SlotID;
        SlotType = data.CurSlot.SlotType;
        SpawnerCount = data.SpawnerCount;
    }
    public BulletBaseSaveData(int _id = -1, int _slotID = -1, 
        SlotType _slotType = SlotType.CurBulletSlot, int _spawnerCount = 0)
    {
        ID = _id;
        SlotID = _slotID;
        SlotType = _slotType;
        SpawnerCount = _spawnerCount;
    }
    public BulletBaseSaveData() {}// 让无参构造也保留，以免 JsonUtility/序列化报错
}

[Serializable]
public class GemBaseSaveData:ItemBaseSaveData
{
    public GemBaseSaveData(GemData data)
    {
        ID = data.ID; //有了ID，其他静态数据通过配表索引出来
        SlotID = data.CurSlot.SlotID;
        SlotType = data.CurSlot.SlotType;
    }
    public GemBaseSaveData() {}// 让无参构造也保留，以免 JsonUtility/序列化报错
}

[Serializable]
public class ItemSaveData:ItemBaseSaveData
{
    public ItemSaveData(ItemData data)
    {
        ID = data.ID; //有了ID，其他静态数据通过配表索引出来
        SlotID = data.CurSlot.SlotID;
        SlotType = data.CurSlot.SlotType;
    }
    public ItemSaveData() {}// 让无参构造也保留，以免 JsonUtility/序列化报错
}

[Serializable]
public class QuestSaveData
{
    public int ID;                   // 唯一ID
    public QuestState State;         // 任务状态
    public int DifficultyLevel;      // 难度等级（用于调整怪物或地图状态）
    public QuestSaveData(Quest quest)
    {
        ID = quest.ID;
        State = quest.State;
        DifficultyLevel = quest.DifficultyLevel;
    }
    public QuestSaveData() {}// 让无参构造也保留，以免 JsonUtility/序列化报错
}
#endregion