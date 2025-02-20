using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine.Serialization;

public enum   MapRoomState
{
    IsLocked = 1,
    Unlocked = 2
}

[Serializable]
public class MapSate
{
    //public int CurMapID;
    public int CurLevelID;
    public int CurRoomID;
    public int TargetRoomID;
    //
    public List<int> IsFinishedMaps;//已经完成的Map
    public List<int> IsFinishedLevels;//已经完成的Map
    public List<int> IsFinishedRooms;//已经完成的RoomID

    public MapSate()
    {
        //CurMapID = 1;
        CurLevelID = 1;
        CurRoomID = 1;
        TargetRoomID = 1;
        IsFinishedMaps = new List<int>();
        IsFinishedLevels = new List<int>();
        IsFinishedRooms = new List<int>();
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