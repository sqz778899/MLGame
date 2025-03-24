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
    public int CurLevelID;
    public int CurRoomID;
    public int TargetRoomID;
    //
    public List<int> IsFinishedRooms;//已经完成的RoomID

    public MapSate()
    {
        CurLevelID = 1;
        CurRoomID = 1;
        TargetRoomID = 1;
        IsFinishedRooms = new List<int>();
    }

    public void FinishAndToNextRoom()
    {
        IsFinishedRooms.Add(CurRoomID);
        CurRoomID = TargetRoomID;
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