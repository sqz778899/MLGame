using System;
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
    public int MaxHP;
    public int HP;
    public int Score;                //分数
    public int Coins;                //硬币数量
    public int RoomKeys;             //钥匙数量
    public List<BulletJson> UserBulletSpawner;  //子弹孵化器
    public List<BulletJson> UserCurBullets;     //当前子弹的状态
    public List<StandbyData> UserStandbyBullet; //子弹材料的状态
    public Dictionary<int, bool> UserBulletSlotLockedState;  //用户子弹槽的锁定状态
    
    public List<ItemJson> UserItems;          //用户道具
    public List<GemJson> UserGems;            //用户宝石
    public List<int> SupremeCharms;           //用户饰品
    public List<MapSate> UserMapSate;         //地图状态

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