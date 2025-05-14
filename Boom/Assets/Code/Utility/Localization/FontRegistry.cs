using TMPro;

public static class FontRegistry
{
    public static TMP_FontAsset Font_zh;
    public static TMP_FontAsset Font_en;
    public static TMP_FontAsset Font_ja;
    
    public static void InitData()
    {
        Font_zh = ResManager.instance.GetAssetCache<TMP_FontAsset>(PathConfig.Font_zh);
        Font_en = ResManager.instance.GetAssetCache<TMP_FontAsset>(PathConfig.Font_en);
        Font_ja = ResManager.instance.GetAssetCache<TMP_FontAsset>(PathConfig.Font_ja);
    }
    
    public static TMP_FontAsset GetCurrentFont() => Loc.CurrentLanguage switch
    {
        LanguageType.ChineseSimplified => Font_zh,
        LanguageType.English => Font_en,
        LanguageType.Japanese => Font_ja,
        _ => Font_en
    };
}