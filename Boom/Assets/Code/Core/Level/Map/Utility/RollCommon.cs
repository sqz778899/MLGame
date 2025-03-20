using System;
using System.Collections.Generic;
using UnityEngine;

public class HaveID
{
    public int ID;
}

[Serializable]
public class RollPR
{
    public int ID;
    public float Probability; //基础概率（真实概率，如10%写为10f）
    [NonSerialized] public int FailCount; //当前连续失败次数（不序列化）

    public RollPR(int id, float prob)
    {
        ID = id;
        Probability = prob;
        FailCount = 1;
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