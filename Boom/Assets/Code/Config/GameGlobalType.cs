


//全部有关分类的枚举都在这里


//稀有度类型细则划分
public enum DropedRarity
{
    None = 0, // 无
    Common = 1,     // 白色
    Rare = 2,       // 蓝色
    Epic = 3,       // 紫色
    Legendary = 4   // 橙色
}

#region 最大的物品类划分细则
//全局物品划分
public enum DropedCategory
{
    Gem = 0,
    Item = 1,
    Key = 2,
    Buff = 3,
    MiracleOddity = 4
}

//Item类划分
public enum ItemCategory
{
    Equipable = 0,       // 原有的可装备型道具
    Persistent = 1,      // 新增：可保留、无效果型道具
}

//Item的Persistent类型划分
public enum PersistentItemType
{
    Resource = 0,
    QuestItem = 1,
}


//Gem类划分
public enum GemType
{
    Damage = 0,       // 攻击型宝石
    Piercing = 1,      // 穿透型宝石
    Resonance = 2
}
#endregion

//Buff来源分类
public enum BuffSource
{
    Item = 0,
    Trait = 1,
    MapEvent = 2
}

public enum BattleBuffType
{
    Addition = 0,
    ChangeType = 1,
}

//敌人分类
public enum EnemyType
{
    None = 0,
    Normal = 1,
    Elite = 2,
    Boss = 3,
}