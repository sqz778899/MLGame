using System;
using UnityEngine;

[Serializable]
public class RollProbability
{
    public int ID;
    public float Probability;
}

public static class RollLayout
{
    public static readonly Vector3 InsOriginPos = new Vector3(-755f,11f,0);
    public const float xOffset = 376;
}