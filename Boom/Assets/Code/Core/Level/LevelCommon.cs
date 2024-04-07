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
    public List<int> SupremeCharms;
    public List<MapSate> UserMapSate;
    public List<RollProbability> UserProbabilitys;
}
#endregion