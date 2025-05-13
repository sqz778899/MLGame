using UnityEngine;

public static class ColorPalette
{
    static GlobalColorPalette _palette;
    public static void Init(GlobalColorPalette palette) => _palette = palette;
    public static Color Rarity(DropedRarity type) => _palette.GetRarityColor(type);
    
    //常用字体颜色
    public static Color NormalTextColor => _palette.NormalTextColor;
    public static Color DamageValueColor => _palette.DamageValueColor;
    public static Color PiercingValueColor => _palette.PiercingValueColor;
    public static Color ResonanceValueColor => _palette.ResonanceValueColor;
    public static Color ElementalInfusionValue => _palette.ElementalInfusionValue;

    public static string ToHex(Color color) =>
        $"#{ColorUtility.ToHtmlStringRGB(color)}";

    public static string ToRichText(string content, Color color) =>
        $"<color={ToHex(color)}>{content}</color>";
}