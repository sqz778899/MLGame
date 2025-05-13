using UnityEngine;

[CreateAssetMenu(fileName = "GlobalColorPalette", menuName = "Configs/GlobalColorPalette")]
public class GlobalColorPalette : ScriptableObject
{
    [Header("稀有度颜色")]
    public Color commonColor = Color.white;
    public Color rareColor = new Color(0.2f, 0.6f, 1f);
    public Color epicColor = new Color(0.7f, 0.3f, 1f);
    public Color legendaryColor = new Color(1f, 0.8f, 0.2f);

    [Header("关键词颜色")]
    public Color NormalTextColor = new Color(0.752f, 0.752f, 0.752f);
    public Color DamageValueColor= new Color(0.58f, 0.35f, 0.35f);
    public Color PiercingValueColor = new Color(0.385f, 0.52f, 0.634f);
    public Color ResonanceValueColor = new Color(0.68f, 0.54f, 0.34f);
    public Color ElementalInfusionValue = Color.magenta;

    public Color GetRarityColor(DropedRarity type)
    {
        return type switch
        {
            DropedRarity.Common => commonColor,
            DropedRarity.Rare => rareColor,
            DropedRarity.Epic => epicColor,
            DropedRarity.Legendary => legendaryColor,
            _ => Color.white
        };
    }

    /*public Color GetKeywordColor(KeywordColorType type)
    {
        return type switch
        {
            KeywordColorType.Buff => buffColor,
            KeywordColorType.Debuff => debuffColor,
            KeywordColorType.Special => specialColor,
            KeywordColorType.Warning => warningColor,
            _ => Color.white
        };
    }*/
}
