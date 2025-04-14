using System.Collections.Generic;

public struct ToolTipsInfo
{
    public string Name;
    public int Level;
    public int Rarity;
    public string Description;
    public ToolTipsType CurToolTipsType;
    public List<ToolTipsAttriSingleInfo> AttriInfos;

    public ToolTipsInfo(string name = "", int level = 0,string desc = "",
        ToolTipsType type = ToolTipsType.None,int _rarity = 0,
        List<ToolTipsAttriSingleInfo> attriInfos = null)
    {
        Name = name;
        Level = level;
        Description = desc;
        AttriInfos = attriInfos ?? new List<ToolTipsAttriSingleInfo>();
        CurToolTipsType = type;
        Rarity = _rarity;
    }
}

public enum ToolTipsType
{
    None = 0,
    Item = 1,
    Bullet = 2,
    Gem = 3
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
