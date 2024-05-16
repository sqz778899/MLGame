using System;
using System.Collections.Generic;
using System.Numerics;

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

[Serializable]
public class NodeOpenFog
{
    public Vector3 position = Vector3.Zero;
    public float openFogRadius = 10;
    public float openFogFadeRange = 10;
}