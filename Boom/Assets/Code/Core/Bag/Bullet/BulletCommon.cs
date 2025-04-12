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
    EditInner = 0,
    Inner = 1,
    EditA = 2,
    EditB = 3,
    Spawner = 4,
    Standby = 6,
    Thumbnail = 7,
    Icon = 8,
    Mat = 9,
    SpawnerInner = 10,
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
#endregion


//在场内子弹的状态
public enum BulletInnerState
{
    Idle = 0,
    AttackBegin = 1,
    Attacking = 2,
    Dead = 3,
    Edit = 4,
    Moving = 5
}
