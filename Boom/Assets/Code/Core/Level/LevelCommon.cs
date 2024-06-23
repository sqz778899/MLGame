using System;
using System.Collections.Generic;

#region 输赢条件相关
public enum WinOrFail
{
    InLevel = 1,
    Win = 2,
    Fail = 3
}
#endregion

#region 存档相关
public class SaveFileJson
{
    public int Score;
    public int Gold;
    public List<BulletSpawner> UserBulletSpawner;
    public List<BulletReady> UserCurBullets;
    public List<StandbyData> UserStandbyBullet;
    public List<int> UserCurBuff;
    
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