using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

public interface ISaveable
{
    public ItemBaseSaveData ToSaveData();
}

[Serializable]
public class TutorialCompletionStatus
{
    public bool L1;
    public bool L1Step1;
    public bool L1Step2;
    public bool L1Step3;
    public bool L1Step4;
    public bool L1Step5;
    
    public bool L2;
    public bool L2Step1;
    
    public bool L3;
    public bool L3Step1;
    
    public TutorialCompletionStatus()
    {
        L1 = false;
        L1Step1 = false;
        L1Step2 = false;
        L1Step3 = false;
        L1Step4 = false;
        L1Step5 = false;
        L2 = false;
        L2Step1 = false;
        L3 = false;
        L3Step1 = false;
    }
}

[Serializable]
public class StorylineNodeStateData
{
    public int ID;                          // 节点ID
    public StorylineState State;            // 当前状态（Inactive、Active、Completed）
    public string CustomDataJson;           // 可选：剧情中用到的自定义状态
}

#region 存档结构
[Serializable]
public class SaveFileJson
{
    //单局会清理的数据
    public int MaxHP;
    public int HP;
    public int Score;                //分数
    public int Coins;                //硬币数量
    public int RoomKeys;             //钥匙数量
    //局外数据
    public int MagicDust;//魔尘
    
    public List<BulletBaseSaveData> UserBulletSpawner;  //子弹孵化器
    public List<BulletBaseSaveData> UserCurBullets;     //当前子弹的状态
    //public List<StandbyData> UserStandbyBullet; //子弹材料的状态
    public Dictionary<int, bool> UserBulletSlotLockedState;  //用户子弹槽的锁定状态
    
    public List<ItemSaveData> UserItems;           //用户道具
    public List<GemBaseSaveData> UserGems;         //用户宝石
    //public List<MapSate> UserMapSate;            //地图状态

    #region 任务剧情新手教程等
    public int UserMainStoryProgress;              //主线剧情进度
    public List<StorylineNodeStateData> UserStorylineNodesState;  //剧情节点状态
    public List<QuestSaveData> UserQuests;         //用户任务完成情况
    public TutorialCompletionStatus UserTutorial;   //用户新手教程完成度
    #endregion
    
    
    public List<TalentData> UserTalents; //用户天赋

    public SaveFileJson()
    {
        MaxHP = 3;
        HP = 3;
        Score = 0;
        Coins = 0;
        RoomKeys = 0;
        MagicDust = 0;
        UserBulletSpawner = new List<BulletBaseSaveData>();
        UserCurBullets = new List<BulletBaseSaveData>();
        UserBulletSlotLockedState = new Dictionary<int, bool>();
        UserItems = new List<ItemSaveData>();
        UserGems = new List<GemBaseSaveData>();

        UserMainStoryProgress = 0;
        UserStorylineNodesState = new List<StorylineNodeStateData>();
        UserQuests = new List<QuestSaveData>();
        UserTutorial = new TutorialCompletionStatus();
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
    public int Rarity;
    public string ResName;
    public string Desc;
    public ItemCategory Category;
    public PersistentItemType PersistentType;
    
    public ItemJson(int _id = -1, string _name = "",int rarity = -1,
        string _desc = "",int _price = 0, string resName = "")
    {
        ID = _id;
        Rarity = rarity;
        Name = _name;
        Desc = _desc;
        ResName = resName;
        Price = _price;
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

[Serializable]
public class TalentJson
{
    public int ID;                   // 唯一ID
    public string Name;              // 天赋名称
    public int Level;                // 天赋等级
    public List<int> DependTalents;  // 依赖的天赋ID
    public List<int> UnlockTalents;  // 解锁的天赋ID
    public int Price;               // 价格
    public TalentEffectType TalentType; //天赋效果类型
    public int EffectID; //受影响的ID
    public int EffectValue; //天赋效果值
    public TalentJson()
    {
        ID = -1;
        Name = "";
        Level = -1;
        DependTalents = new List<int>();
        UnlockTalents = new List<int>();
        Price = -1;
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
        SlotID = item.CurSlotController.SlotID;
        SlotType = item.CurSlotController.SlotType;
    }
}

[Serializable]
public class BulletBaseSaveData:ItemBaseSaveData
{
    public int SpawnerCount;
    public BulletBaseSaveData(BulletData data)
    {
        ID = data.ID; //有了ID，其他静态数据通过配表索引出来
        SlotID = data.CurSlotController.SlotID;
        SlotType = data.CurSlotController.SlotType;
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
        SlotID = data.CurSlotController.SlotID;
        SlotType = data.CurSlotController.SlotType;
    }
    public GemBaseSaveData() {}// 让无参构造也保留，以免 JsonUtility/序列化报错
}

[Serializable]
public class ItemSaveData:ItemBaseSaveData
{
    public ItemSaveData(ItemData data)
    {
        ID = data.ID; //有了ID，其他静态数据通过配表索引出来
        SlotID = data.CurSlotController.SlotID;
        SlotType = data.CurSlotController.SlotType;
    }
    public ItemSaveData() {}// 让无参构造也保留，以免 JsonUtility/序列化报错
}

[Serializable]
public class QuestSaveData
{
    public int ID;                   // 唯一ID
    public bool IsCompleted;         // 是否完成过
    public QuestState State;         // 任务此时状态（可以重复刷所以与上述不冲突）
    public int DifficultyLevel;      // 难度等级（用于调整怪物或地图状态）
    public int TotalScore;             //历史最高总分
    public int TotalLoopCount;         //历史最高循环次数
    public int ExplorationPercent;   //探索进度
    public QuestSaveData(Quest quest)
    {
        ID = quest.ID;
        State = quest.State;
        IsCompleted = quest.IsCompleted;
        DifficultyLevel = quest.DifficultyLevel;
        TotalScore = quest.TotalScore;
        TotalLoopCount = quest.TotalLoopCount;
        ExplorationPercent = quest.ExplorationPercent;
    }
    public QuestSaveData() {}// 让无参构造也保留，以免 JsonUtility/序列化报错
}
#endregion