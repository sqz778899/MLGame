using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine.Serialization;

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
    public int CurRoomID;
    public int CurMapNodeID;
    //
    public List<int> IsFinishedMaps;//已经完成的Map
    public List<int> IsFinishedRooms;//已经完成的RoomID
    public List<int> IsFinishedMapNodes;//已经完成的MapNodeID
    
    public MapSate()
    {
        CurMapID = 1;
        CurRoomID = 1;
        CurMapNodeID = 0;
        IsFinishedMaps = new List<int>();
        IsFinishedRooms = new List<int>();
        IsFinishedMapNodes = new List<int>();
    }
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