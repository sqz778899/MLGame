#region 输赢条件相关

using System;
using System.Collections.Generic;

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
    
    public List<int> SupremeCharms;
    public List<MapSate> UserMapSate;
    public List<RollProbability> UserProbabilitys;
}

#endregion