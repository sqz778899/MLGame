using TMPro;
using UnityEngine;

public static class FloatingTextFactory
{
    // 配置路径
    static readonly string WorldTextPrefabPath = PathConfig.TxtFloatingPB;     // 世界用 TextMeshPro
    static readonly string UITextPrefabPath = PathConfig.TxtFloatingUIPB;
    
    // 不同跳字对应不同字体
    public static TMP_FontAsset DamageFontAsset;
    public static TMP_FontAsset MapHintFontAsset;

    public static Material DamageFontMaterial;
    
    /// <summary>
    /// 创建一个世界坐标系中的浮动文字
    /// </summary>
    public static void CreateWorldText(string content, Vector3 worldPos, 
        FloatingTextType type = FloatingTextType.MapHint,
        Color? color = null, float fontSize = 4f, Transform parent = null)
    {
        GameObject prefab = ResManager.instance.CreatInstance(WorldTextPrefabPath);
        prefab.transform.position = worldPos;
        if (parent != null)
            prefab.transform.SetParent(parent, false);

        FloatingText floating = prefab.GetComponent<FloatingText>();
        TextMeshPro curText = prefab.GetComponent<TextMeshPro>();
        floating.textRenderer.sortingLayerName = "FloatingText";
        
        // 应用材质和字体
        switch (type)
        {
            case FloatingTextType.Damage:
                curText.font = DamageFontAsset;
                curText.fontMaterial = DamageFontMaterial;
                break;
            case FloatingTextType.MapHint:
                curText.font = MapHintFontAsset;
                break;
        }
        
        floating.Animate(content, color ?? Color.white, fontSize);
    }

    /// <summary>
    /// 创建一个 UI 坐标系中的浮动文字
    /// </summary>
    public static void CreateUIText(string content,Vector3 UIPos, Color? color = null, float fontSize = 28f)
    {
        GameObject prefab = ResManager.instance.CreatInstance(UITextPrefabPath);
        prefab.transform.SetParent(EternalCavans.Instance.FloatingTextRoot.transform, false);
        
        FloatingText floating = prefab.GetComponent<FloatingText>();
        prefab.GetComponent<RectTransform>().anchoredPosition = UIPos;
        floating.Animate(content, color ?? Color.white, fontSize);
    }
}

public enum FloatingTextType
{
    Damage,
    MapHint
}
