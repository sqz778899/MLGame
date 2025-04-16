using UnityEngine;

public static class FloatingTextFactory
{
    // 配置路径（根据你资源管理方式调整）
    static readonly string WorldTextPrefabPath = PathConfig.TxtFloatingPB;     // 世界用 TextMeshPro
    static readonly string UITextPrefabPath = PathConfig.TxtFloatingUIPB;

    /// <summary>
    /// 创建一个世界坐标系中的浮动文字
    /// </summary>
    public static void CreateWorldText(string content, Vector3 worldPos, 
        Color? color = null, float fontSize = 4f, Transform parent = null)
    {
        GameObject prefab = ResManager.instance.CreatInstance(WorldTextPrefabPath);
        prefab.transform.position = worldPos;
        if (parent != null)
            prefab.transform.SetParent(parent, true);

        FloatingText floating = prefab.GetComponent<FloatingText>();
        floating.textRenderer.sortingLayerName = "FloatingText";
        floating.Animate(content, color ?? Color.white, fontSize);
    }

    /// <summary>
    /// 创建一个 UI 坐标系中的浮动文字
    /// </summary>
    public static void CreateUIText(string content, Transform uiParent, 
        Color? color = null, float fontSize = 28f)
    {
        GameObject prefab = ResManager.instance.CreatInstance(UITextPrefabPath);
        prefab.transform.SetParent(uiParent, false);

        FloatingText floating = prefab.GetComponent<FloatingText>();
        floating.textRenderer.sortingLayerName = "FloatingText";
        floating.Animate(content, color ?? Color.white, fontSize);
    }
}