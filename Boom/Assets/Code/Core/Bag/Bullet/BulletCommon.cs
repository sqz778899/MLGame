using System;
using UnityEngine;

#region 一些枚举类
public enum MutMode
{
    Add = 1,
    Sub = 2,
}

public enum BulletInsMode
{
    Inner = 1,
    EditA = 2,
    EditB = 3,
    Spawner = 4,
    Standby = 6,
    Thumbnail = 7,
    Icon = 8,
    Mat = 9
}

public enum RollBulletMatType
{
    Mat = 1,
    Score = 2
}
#endregion

#region 主要的子弹类
public enum ElementalTypes
{
    Non = 1,
    Ice = 2,
    Fire = 3,
    Electric = 4
}

[Serializable]
public class StandbyData:HaveID
{
    public int SlotID;
    public int InstanceID;

    public StandbyData(int slotID = 0,int instanceID = 0)
    {
        SlotID = slotID;
        InstanceID = instanceID;
    }
}
#endregion


//在场内子弹的状态
public enum BulletInnerState
{
    Common,
    AttackBegin,
    Attacking,
    Dead
}

public class BulletTooltipInfo
{
    public Sprite bulletImage;
    public string name;
    public string description;
}
