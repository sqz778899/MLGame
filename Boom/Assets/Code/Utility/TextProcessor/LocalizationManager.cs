using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class LocalizationManager
{
    static Dictionary<string, string> _langData = new();
    public static SystemLanguage CurrentLanguage = Application.systemLanguage;

    public static void LoadLanguage(SystemLanguage lang)
    {
        // 示例：从JSON加载
        string path = $"Localization/{lang.ToString()}";
        TextAsset json = Resources.Load<TextAsset>(path);
        _langData = JsonUtility.FromJson<LocalizationData>(json.text).ToDictionary();
    }

    public static string Get(string key)
    {
        if (_langData.TryGetValue(key, out string value))
            return value;
        return $"[Missing:{key}]";
    }
}

#region Localization 类
[Serializable]
public class LocalizationData
{
    public List<LangEntry> entries;
    public Dictionary<string, string> ToDictionary() =>
        entries.ToDictionary(e => e.key, e => e.value);
}

[Serializable]
public class LangEntry
{
    public string key;
    public string value;
}
#endregion