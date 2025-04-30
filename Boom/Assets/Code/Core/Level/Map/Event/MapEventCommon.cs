using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public abstract class MapEventRuntimeData {}
[Serializable]
public abstract class MapEventConfigData
{
    public abstract MapEventRuntimeData ToRuntimeData();
}

#region 事件具体数据结构分别实现
#region 奖励型
//金币堆事件
[Serializable]
public class CoinsPileRuntimeData : MapEventRuntimeData
{
    public int MinGold;
    public int MaxGold;
}
//宝箱事件
[Serializable]
public class TreasureBoxRuntimeData : MapEventRuntimeData
{
    public DropedRarity Rarity;
    public int MinLootCount;
    public int MaxLootCount;
    public List<DropedObjEntry> DropTable;
}
//子弹事件
[Serializable]
public class BulletEventRuntimeData : MapEventRuntimeData
{
    public int BulletID;
    public string DialogueName;
}
//钥匙事件
[Serializable]
public class RoomKeyRuntimeData : MapEventRuntimeData
{
    public int RoomKeysNum = 1;
}
#endregion

#region 赌博型
[Serializable]
public class BasicGamblingRuntimeData : MapEventRuntimeData
{
    public int EmptyChance;
    public int KeyChance; //钥匙事件
    public int TempBuffChance;
    public int TempDebuffChance;
    public int NormalLootChance;
    public int MetaResourceChance;
    public int RareLootChance;
    //多重掉落支持
    public int MinDropCount = 1; // 最少掉几个
    public int MaxDropCount = 1; // 最多掉几个
}
#endregion

#region Lore探索型
[Serializable]
public class StoneTabletRuntimeData : MapEventRuntimeData
{
    public List<string> LoreTexts;
}
#endregion

#region 奇异互动型
[Serializable]
public class WigglingBoxRuntimeData : MapEventRuntimeData
{
    public int RunAwayChance;
    public int TalkChance;
    public int ExplosionChance;
    public int LootChance;
}
//商店事件
[Serializable]
public class ShopEventRuntimeData : MapEventRuntimeData
{
    public ShopType ShopType;
    public List<RollPR> RollPRs;
    public int ShopCost;
    
    // 缓存实例化的 Shop 对象
    [NonSerialized] public GameObject ShopInstance;
    [NonSerialized] public bool IsFirstOpen = true;
}
#endregion

#region 房间箭头功能类
[Serializable]
public class RoomArrowRuntimeData : MapEventRuntimeData
{
    public RoomArrowType ArrowType;
    public int TargetRoomID;
    public EnemyConfigData BattleConfig;
}
#endregion
#endregion

#region 事件具体ConfigData分别实现
#region 奖励型
//金币堆事件
[Serializable]
public class GoldPileConfigData : MapEventConfigData
{
    public int MinGold = 2;
    public int MaxGold = 4;

    public override MapEventRuntimeData ToRuntimeData() =>
        new CoinsPileRuntimeData { MinGold = MinGold, MaxGold = MaxGold };
}
//宝箱事件
[Serializable]
public class TreasureBoxConfigData : MapEventConfigData
{
    public DropedRarity Rarity;
    public int MinLootCount;
    public int MaxLootCount;
    public List<DropedObjEntry> DropTable = new();
    
    public override MapEventRuntimeData ToRuntimeData() => new TreasureBoxRuntimeData
    {
        Rarity = Rarity,
        MinLootCount = MinLootCount,
        MaxLootCount = MaxLootCount,
        DropTable = DropTable
    };
}
//子弹事件
[Serializable]
public class BulletEventConfigData : MapEventConfigData
{
    public int BulletID;
    public string DialogueName;

    public override MapEventRuntimeData ToRuntimeData()
    {
        return new BulletEventRuntimeData
        {
            BulletID = this.BulletID,
            DialogueName = this.DialogueName
        };
    }
}
//钥匙事件
[Serializable]
public class RoomKeyConfigData : MapEventConfigData
{
    public int RoomKeysNum = 1;

    public override MapEventRuntimeData ToRuntimeData() =>
        new RoomKeyRuntimeData { RoomKeysNum = RoomKeysNum };
}
#endregion

#region 赌博型
[Serializable]
public class BasicGamblingConfigData : MapEventConfigData
{
    [Range(0, 100)] public int EmptyChance = 25;
    [Range(0, 100)] public int KeyChance = 25;
    [Range(0, 100)] public int TempBuffChance = 20;
    [Range(0, 100)] public int TempDebuffChance = 30;
    [Range(0, 100)] public int NormalLootChance = 10;
    [Range(0, 100)] public int MetaResourceChance = 10;
    [Range(0, 100)] public int RareLootChance = 5;
    //多重掉落支持
    public int MinDropCount = 1; // 最少掉几个
    public int MaxDropCount = 1; // 最多掉几个
    public override MapEventRuntimeData ToRuntimeData() => new BasicGamblingRuntimeData
    {
        EmptyChance = EmptyChance,
        KeyChance = KeyChance,
        TempBuffChance = TempBuffChance,
        TempDebuffChance = TempDebuffChance,
        NormalLootChance = NormalLootChance,
        MetaResourceChance = MetaResourceChance,
        RareLootChance = RareLootChance,
        //ClutterTags = ClutterTags,
        MinDropCount = MinDropCount,
        MaxDropCount = MaxDropCount,
    };
}
#endregion

#region Lore探索型
[Serializable]
public class StoneTabletConfigData : MapEventConfigData
{
    public List<string> LoreTexts = new()
    {
        "魔法之灾起源于无名塔。",
        "禁咒只在最深处的图书馆中记载。",
        "他们封印了通往旧纪元的门。"
    };

    public override MapEventRuntimeData ToRuntimeData()
    {
        return new StoneTabletRuntimeData { LoreTexts = LoreTexts };
    }
}
#endregion

#region 奇异互动型
[Serializable]
public class WigglingBoxConfigData : MapEventConfigData
{
    public int RunAwayChance = 30;
    public int TalkChance = 30;
    public int ExplosionChance = 20;
    public int LootChance = 20;

    public override MapEventRuntimeData ToRuntimeData()
    {
        return new WigglingBoxRuntimeData
        {
            RunAwayChance = RunAwayChance,
            TalkChance = TalkChance,
            ExplosionChance = ExplosionChance,
            LootChance = LootChance
        };
    }
}
//商店事件
[Serializable]
public class ShopEventConfigData : MapEventConfigData
{
    public ShopType ShopType;
    public List<RollPR> RollPRs;
    public int ShopCost;

    public override MapEventRuntimeData ToRuntimeData()
    {
        return new ShopEventRuntimeData
        {
            ShopType = ShopType,
            RollPRs = new List<RollPR>(RollPRs),
            ShopCost = ShopCost
        };
    }
}

#endregion

#region 房间箭头功能类
[Serializable]
public class RoomArrowConfigData : MapEventConfigData
{
    public RoomArrowType ArrowType;
    public int TargetRoomID;
    public EnemyConfigData BattleConfig;

    public override MapEventRuntimeData ToRuntimeData()
    {
        return new RoomArrowRuntimeData
        {
            ArrowType = ArrowType,
            TargetRoomID = TargetRoomID,
            BattleConfig = BattleConfig
        };
    }
}
#endregion
#endregion

#region 一些枚举定义
//主类别
public enum MapEventType
{
    None = 0,
    CoinsPile = 1,
    BasicGambling = 2,
    StoneTablet = 4,
    MysticalInteraction = 5,
    TreasureBox = 6,
    Bullet = 7,
    RoomKey = 8,
    Enemy,
    Shop,
    Event,
    Boss,
    RoomArrow = 100,
}

//EventType规则定义（可重复触发判断）
public static class EventTypeRules
{
    public static bool IsRepeatable(MapEventType type)
    {
        return type switch
        {
            MapEventType.RoomArrow => true,
            MapEventType.Shop => true,
            MapEventType.Event => true,
            _ => false
        };
    }
}

public class MapNodeData
{
    public int ID;
    public List<string> ClutterTags;
    public MapEventType EventType;
    
    public MapEventRuntimeData EventData;

    public bool IsLocked;
    public bool IsTriggered;

    public MapNodeData(int id, List<string> _clutterTags,
        MapEventType eventType,MapEventRuntimeData eventData)
    {
        ID = id;
        ClutterTags = _clutterTags;
        EventType = eventType;
        IsLocked = false;
        IsTriggered = false;
        EventData = eventData;
    }
}
#endregion