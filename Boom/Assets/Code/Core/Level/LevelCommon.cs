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
    public List<BulletSaveData> UserBulletSpawner;  //子弹孵化器
    public List<BulletSaveData> UserCurBullets;     //当前子弹的状态
    public List<StandbyData> UserStandbyBullet; //子弹材料的状态
    public Dictionary<int, bool> UserBulletSlotLockedState;  //用户子弹槽的锁定状态
    
    public List<ItemJson> UserItems;          //用户道具
    public List<GemJson> UserGems;            //用户宝石
    public List<MapSate> UserMapSate;         //地图状态

    public SaveFileJson()
    {
        MaxHP = 3;
        HP = 3;
        Score = 0;
        Coins = 0;
        RoomKeys = 0;
        UserBulletSpawner = new List<BulletSaveData>();
        UserCurBullets = new List<BulletSaveData>();
        UserStandbyBullet = new List<StandbyData>();
        UserBulletSlotLockedState = new Dictionary<int, bool>();
        UserItems = new List<ItemJson>();
        UserGems = new List<GemJson>();
        UserMapSate = new List<MapSate>();
    }
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