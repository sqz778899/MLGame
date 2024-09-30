using System;
using System.Collections.Generic;
using System.Numerics;

public enum MapNodeState
{
    UnLocked = 1,
    Locked = 2,
    IsFinish = 3,
}

public enum MapRoomState
{
    UnFinish = 1,
    IsFinish = 2,
}

[Serializable]
public class MapSate
{
    public int CurMapID;
    public int MapID;
    public int LevelID;
    public List<int> IsFinishedLevels;
}

public enum MapNodeType
{
    Main = 1,
    Shop = 2,
    Event = 3,
    TreasureBox = 4,
    GoldPile = 5
}


public enum MapEventType
{
    Non = 0,
    PREvent = 1
}