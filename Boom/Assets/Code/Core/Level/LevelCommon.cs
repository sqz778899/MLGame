#region 输赢条件相关

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
    public BagDataJson BagData;
    public int Score;
    public int Gold;
    public List<StandbyData> UserStandbyBullet;
   
    public List<int> SupremeCharms;
    public List<MapSate> UserMapSate;
    public List<RollProbability> UserProbabilitys;
}

public class StandbyData
{
    public int BulletID;
    public int SlotID;

    public StandbyData(int bulletID = 0,int slotID = 0)
    {
        BulletID = bulletID;
        SlotID = slotID;
    }
}
#endregion