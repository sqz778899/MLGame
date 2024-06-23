using System;
using System.Collections.Generic;
using UnityEngine;

public class HaveID
{
    public int ID;
}

[Serializable]
public class RollPR:HaveID
{
    public float Probability;
}

[Serializable]
public class RollPREvent:HaveID
{
    public string Title;
    public string EDescription;
    public Dictionary<int, float> AddPRDict;
    public Dictionary<int, float> SubPRDict;
}