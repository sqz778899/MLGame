using System.Collections.Generic;

public static class RichTextColorConfig
{
    public static Dictionary<RichTextColorType, string> ColorHexMap = new()
    {
        { RichTextColorType.Green, "#66ff66" },
        { RichTextColorType.Red, "#ff5555" },
        { RichTextColorType.Blue, "#55ccff" },
        { RichTextColorType.Yellow, "#ffff88" },
        { RichTextColorType.Rarity_Common, "#FFFFFF" },
        { RichTextColorType.Rarity_Rare, "#4aa2ff" },
        { RichTextColorType.Rarity_Epic, "#b47aff" },
        { RichTextColorType.Rarity_Legendary, "#ffb400" }
    };

    public static string GetColorHex(RichTextColorType type)
    {
        return ColorHexMap.TryGetValue(type, out var hex) ? hex : "#ffffff";
    }
}

public enum RichTextColorType
{
    Green,
    Red,
    Blue,
    Yellow,
    Rarity_Common,
    Rarity_Rare,
    Rarity_Epic,
    Rarity_Legendary
}

