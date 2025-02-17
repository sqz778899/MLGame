﻿using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

#region 输赢条件相关
public enum WinOrFail
{
    InLevel = 1,
    Win = 2,
    Fail = 3
}
#endregion

#region 存档相关
[Serializable]
public class SaveFileJson
{
    public int Score;
    public int Coins;
    public int RoomKeys;
    public List<BulletJson> UserBulletSpawner;
    public List<BulletJson> UserCurBullets;
    public List<BulletEntry> UserBulletEntries;
    public List<StandbyData> UserStandbyBullet;
    public List<int> UserCurBuff;
    
    public List<ItemJson> UserItems;
    public List<GemJson> UserGems;
    public List<int> SupremeCharms;
    public List<MapSate> UserMapSate;

    public List<int> CurRollPREveIDs;
}

public class UserConfig
{
    public int UserLanguage;
    public int UserScreenResolution;
    public int UserScreenMode;

    public UserConfig()
    {
        UserLanguage = 0;
        UserScreenResolution = 2;
        UserScreenMode = 1;
    }
}
#endregion

public class LevelBuff
{
    public int LevelID;
    public List<RollPR> CurBuffProb;
}