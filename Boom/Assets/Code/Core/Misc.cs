using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.Serialization;

public enum ShopType
{
    All = 0,
    GemShop = 1,
    BulletShop = 2
}

#region 多语言相关
public enum MultiLaEN
{
    English = 0,
    ZH_Simplified = 1,
    ZH_Traditional = 2,
    Japanese = 3,
    Korean = 4,
}

public class MStr
{
    public string Str;
    public float FondSize;
    public TMP_FontAsset FondAsset;

    public MStr(string _str,float _fondSize,TMP_FontAsset _fondAsset)
    {
        Str = _str;
        FondSize = _fondSize;
        FondAsset = _fondAsset;
    }
}

public class MultiLaJson
{
    public List<string> English;
    public Dictionary<string, string> ZH_Simplified;
    public Dictionary<string, string> ZH_Traditional;
    public Dictionary<string, string> Japanese;
    public Dictionary<string, string> Korean;

    public MultiLaJson()
    {
        English = new List<string>();
        ZH_Simplified = new Dictionary<string, string>();
        ZH_Traditional = new Dictionary<string, string>();
        Japanese = new Dictionary<string, string>();
        Korean = new Dictionary<string, string>();
    }
}
#endregion

//score
//gold
//insignias

public enum TalentType
{
    Normal = 0,
    Resource = 1,
    Gem = 2,
    Bullet = 3,
}

public enum CreateItemType
{
    ShopGem = 0,
    MiniBagGem = 1,
    TempGem = 2,
}

public enum UILockedState
{
    isNormal = 0,
    isLocked = 1,
    isSelected = 2
}

public enum SceneState
{
    StartGame = 0,
    MainEnv = 1,
    MapScene = 2,
    LoadingScene = 3
}