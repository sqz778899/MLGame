using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Loc
{
    public static event Action OnLanguageChanged;
    
    public static LanguageType CurrentLanguage = LanguageType.ChineseSimplified;

    static LocDataJson table = new();

    public static void InitData() =>
        table = JsonConvert.DeserializeObject<LocDataJson>(File.ReadAllText(PathConfig.LocalizationConfigJson));
    
    public static void Load(LanguageType lang)
    {
        CurrentLanguage = lang;
        OnLanguageChanged?.Invoke();//语言改变事件
    }

    static Dictionary<LocTableType,string> TableNameDict = new()
    {
        {LocTableType.mo,"奇迹物件"},
        {LocTableType.ui,"UI"},
        {LocTableType.item,"Item"},
        {LocTableType.bullet,"Bullet"},
        {LocTableType.gem,"Gem"},
        {LocTableType.battle,"Battle"},
        {LocTableType.npc,"NPC"},
        {LocTableType.dia,"Dia"},
    };
    public static string Get(string key)
    {
        if (string.IsNullOrEmpty(key)) return "#NULL#";
        
        string[] tmp = key.Split(".");
        LocTableType locType = (LocTableType)Enum.Parse(typeof(LocTableType),tmp[0]);
        if(!TableNameDict.TryGetValue(locType, out string tableName)) return $"MissingTable:{tmp[0]}";//表名不存在
        
        LocTable locTable = table.LocTableDict[tableName];
        MultipleLanguage loc = locTable.LocDict[key];
        string val =  CurrentLanguage switch
        {
            LanguageType.ChineseSimplified => loc.zh,
            LanguageType.English => loc.en,
            LanguageType.Japanese => loc.ja,
            _ => loc.zh
        };
        return string.IsNullOrEmpty(val) ? $"#Empty:{key}#" : val;
    }

    //离线把表数据根据类型推断Key
    public static GuessLocKey GuessKey(int ID,LocTableType locType) => new GuessLocKey(ID,locType);
  
    public static string Get(string key, params object[] args) => string.Format(Get(key), args);

    public static string ToHex(Color color) => $"#{ColorUtility.ToHtmlStringRGB(color)}";
    public static string ToRichText(string content, Color color) => $"<color={ToHex(color)}>{content}</color>";
}

public class GuessLocKey
{
    public string NameKey;
    public string DescKey;
    public string FlavorKey;

    public string ssss;

    public GuessLocKey(int ID,LocTableType locType)
    {
        NameKey = $"{locType.ToString()}.{ID}.name";
        DescKey = $"{locType.ToString()}.{ID}.desc";
        FlavorKey = $"{locType.ToString()}.{ID}.flavor";
    }
}

public enum LanguageType
{
    ChineseSimplified = 0,
    English = 1,
    Japanese = 2,
    // Add more...
}

public enum LocTableType
{
    non = 0,
    mo = 1,
    ui = 2,
    item = 3,
    bullet = 4,
    gem = 5,
    battle = 6,
    npc = 7,
    dia = 8,
}

// 示例用法：
// Loc.Get("ui.start")
// Loc.Get("combat.damage", 10)
// Loc.ToRichText(Loc.Get("buff.fire"), Color.red) 

// 在游戏启动时调用 Loc.Load(LanguageType.English);
//CSV 示例格式
//lang_en.csv
//ui.start,Start Game
//combat.damage,Deals {0} damage!
//buff.fire,Ignited

//lang_zh.csv
//ui.start,开始游戏
//combat.damage,造成 {0} 点伤害！
//buff.fire,燃烧
