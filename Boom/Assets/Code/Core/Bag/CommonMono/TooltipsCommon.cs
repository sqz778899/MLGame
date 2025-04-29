using System.Collections.Generic;

public struct ToolTipsInfo
{
    public string Name;
    public int Level;
    public DropedRarity Rarity;
    public string Description;
    public ToolTipsType CurToolTipsType;
    public ItemCategory Category;
    public PersistentItemType PersistentType;
    
    public List<ToolTipsAttriSingleInfo> AttriInfos;

    public ToolTipsInfo(string name = "", int level = 0,string desc = "",
        ToolTipsType type = ToolTipsType.None,DropedRarity _rarity = 0,
        ItemCategory _category = ItemCategory.Equipable,
        PersistentItemType _persistentType = PersistentItemType.QuestItem,
        List<ToolTipsAttriSingleInfo> attriInfos = null)
    {
        Name = name;
        Level = level;
        Description = desc;
        AttriInfos = attriInfos ?? new List<ToolTipsAttriSingleInfo>();
        Category = _category;
        PersistentType = _persistentType;
        CurToolTipsType = type;
        Rarity = _rarity;
    }
}

public enum ToolTipsType
{
    None = 0,
    Item = 1,
    Bullet = 2,
    Gem = 3,
    Buff = 4,
    Trait = 5,
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
