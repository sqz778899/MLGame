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

#region 新地图事件框架
public interface IMapInteractable
{
    void Interact();
    string GetHint();
}

public abstract class MapInteractEvent : MonoBehaviour, IMapInteractable
{
    [Header("基础设定")]
    public string HintText = "调查";
    public bool IsLocked = false;
    public bool IsTutorialOnly = false;

    [Header("表现组件")]
    public SpriteRenderer SpriteRenderer;
    public Sprite OpenedSprite;

    public virtual void Interact()
    {
        if (IsLocked || (UIManager.Instance.IsLockedClick && !IsTutorialOnly)) return;
        ExecuteEventLogic();
    }

    public string GetHint() => HintText;

    protected abstract void ExecuteEventLogic();
}

public enum MapEventType
{
    None = 0,
    CoinsPile = 1,
    WeaponRack = 2,
    Skeleton = 3,
    StoneTablet = 4,
    MysticalInteraction = 5,
    Treasure,
    Enemy,
    Shop,
    Event,
    Boss,
}

public class MapNodeData
{
    public int ID;
    public string Name;
    public string Desc;
    public MapEventType EventType;
    
    public MapEventRuntimeData EventData;

    public bool IsLocked;
    public bool IsTriggered;

    public MapNodeData(int id, string name, string desc, 
        MapEventType eventType,MapEventRuntimeData eventData)
    {
        ID = id;
        Name = name;
        Desc = desc;
        EventType = eventType;
        IsLocked = false;
        IsTriggered = false;
        EventData = eventData;
    }
}
#endregion