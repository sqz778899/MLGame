using System;
using System.Collections.Generic;

public enum MapNodeState
{
    UnLocked = 1,
    Locked = 2,
    IsFinish = 3,
}

[Serializable]
public class MapSate
{
    public int CurMapID;
    public int MapID;
    public int LevelID;
    public List<int> IsFinishedLevels;
}