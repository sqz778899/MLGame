using System;
using System.Collections.Generic;
using UnityEngine;

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
    public int MapRandomSeed; //用于控制本地图内的伪随机性

    public MapSate()
    {
        ExplorePercent = 0;
        CurRoomID = 1;
        TargetRoomID = 1;
        IsFinishedRooms = new List<int>();
        IsFinishedRooms.Add(1);
        MapRandomSeed = GenerateNewSeed();
    }
    
    public void SetCurRoomID(int id)
    {
        CurRoomID = id;
        if (!IsFinishedRooms.Contains(CurRoomID))
            IsFinishedRooms.Add(CurRoomID);
    }

    public void FinishAndToNextRoom()
    {
        CurRoomID = TargetRoomID;
        if (!IsFinishedRooms.Contains(CurRoomID))
            IsFinishedRooms.Add(CurRoomID);
        //计算探索进度
        float s = IsFinishedRooms.Count;
        ExplorePercent = (int)(s / AllRoomCount * 100);
    }
    
    int GenerateNewSeed() => Guid.NewGuid().GetHashCode();
}