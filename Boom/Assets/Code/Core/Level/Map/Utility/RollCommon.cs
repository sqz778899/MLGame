using System;
using System.Collections.Generic;
using UnityEngine;

public class HaveID
{
    public int ID;
}

[Serializable]
public struct RollPR
{
    public int ID;
    public float Probability;

    public RollPR(int ID = -1, float Probability = -1)
    {
        this.ID = ID;
        this.Probability = Probability;
    }
}

[Serializable]
public class RollPREvent:HaveID
{
    public string Title;
    public string EDescription;
    public Dictionary<int, float> AddPRDict;
    public Dictionary<int, float> SubPRDict;
}