using System.Collections.Generic;

public struct TooltipsInfo
{
    public string Name;
    public int Level;
    public List<ToolTipsAttriSingleInfo> AttriInfos;

    public TooltipsInfo(string name = "", int level = 0,
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
