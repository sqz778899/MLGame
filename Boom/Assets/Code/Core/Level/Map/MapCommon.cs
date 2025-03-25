using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;

public enum   MapRoomState
{
    IsLocked = 1,
    Unlocked = 2
}

[Serializable]
public class MapSate
{
    public int CurRoomID;
    public int TargetRoomID;
    //
    public int AllRoomCount;
    public int ExplorePercent;
    //
    public List<int> IsFinishedRooms;//已经完成的RoomID

    public MapSate()
    {
        ExplorePercent = 0;
        CurRoomID = 1;
        TargetRoomID = 1;
        IsFinishedRooms = new List<int>();
        IsFinishedRooms.Add(1);
    }
    
    public void SetCurRoomID(int id)
    {
        CurRoomID = id;
        if (!IsFinishedRooms.Contains(CurRoomID))
            IsFinishedRooms.Add(CurRoomID);
        Debug.Log($"IsFinishedRooms:{IsFinishedRooms.Count}  AllRoomCount:{AllRoomCount}");
    }

    public void FinishAndToNextRoom()
    {
        CurRoomID = TargetRoomID;
        if (!IsFinishedRooms.Contains(CurRoomID))
            IsFinishedRooms.Add(CurRoomID);
        //计算探索进度
        float s = IsFinishedRooms.Count;
        Debug.Log($"IsFinishedRooms:{IsFinishedRooms.Count}  AllRoomCount:{AllRoomCount}");
        ExplorePercent = (int)(s / AllRoomCount * 100);
    }
}