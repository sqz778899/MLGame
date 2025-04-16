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
[Serializable]
public class CoinsPileRuntimeData : MapEventRuntimeData
{
    public int MinGold;
    public int MaxGold;
}
#endregion

#region 赌博型
[Serializable]
public class WeaponRackRuntimeData : MapEventRuntimeData
{
    public int EmptyChance;
    public int TempBuffChance;
    public int TempDebuffChance;
    public int NormalLootChance;
    public int MetaResourceChance;
    public int RareLootChance;
}

[Serializable]
public class SkeletonRuntimeData : MapEventRuntimeData
{
    public int EmptyChance;
    public int NoteChance;
    public int ItemChance;
    public int DebuffChance;
    public int KeyChance;
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
#endregion
#endregion


#region 事件具体ConfigData分别实现
#region 奖励型
[Serializable]
public class GoldPileConfigData : MapEventConfigData
{
    public int MinGold = 2;
    public int MaxGold = 4;

    public override MapEventRuntimeData ToRuntimeData() =>
        new CoinsPileRuntimeData { MinGold = MinGold, MaxGold = MaxGold };
}
#endregion

#region 赌博型
[Serializable]
public class WeaponRackConfigData : MapEventConfigData
{
    [Range(0, 100)] public int EmptyChance = 25;
    [Range(0, 100)] public int TempBuffChance = 20;
    [Range(0, 100)] public int TempDebuffChance = 30;
    [Range(0, 100)] public int NormalLootChance = 10;
    [Range(0, 100)] public int MetaResourceChance = 10;
    [Range(0, 100)] public int RareLootChance = 5;

    public override MapEventRuntimeData ToRuntimeData() => new WeaponRackRuntimeData
    {
        EmptyChance = EmptyChance,
        TempBuffChance = TempBuffChance,
        TempDebuffChance = TempDebuffChance,
        NormalLootChance = NormalLootChance,
        MetaResourceChance = MetaResourceChance,
        RareLootChance = RareLootChance
    };
}

[Serializable]
public class SkeletonConfigData : MapEventConfigData
{
    public List<string> LoreNotes = new()
    {
        "别信墙上的门", 
        "是我先发现这个宝藏的", 
        "钥匙……藏在壁炉……咳……"
    };

    public int EmptyChance = 30;
    public int NoteChance = 30;
    public int ItemChance = 20;
    public int DebuffChance = 15;
    public int KeyChance = 5;
    
    public override MapEventRuntimeData ToRuntimeData() => new SkeletonRuntimeData
    {
        EmptyChance = EmptyChance,
        NoteChance = NoteChance,
        ItemChance = ItemChance,
        DebuffChance = DebuffChance,
        KeyChance = KeyChance,
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
#endregion
#endregion