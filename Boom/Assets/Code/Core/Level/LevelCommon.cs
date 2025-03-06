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

public class LevelBuff
{
    public int LevelID;
    public List<RollPR> CurBuffProb;
}