using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaRole
{
    public string Name;
    public string NameKey;
}

public struct DiaSingle
{
    public int ID;
    public string Sign;
    public string NameKey;
    public int IsLeft;
    public int NextIdex;
    public string ContentKey;

    public DiaSingle(int _id, string _sign, string nameKey, int _isLeft, int _nextIdex, string contentKey)
    {
        ID = _id;
        Sign = _sign;
        NameKey = nameKey;
        IsLeft = _isLeft;
        NextIdex = _nextIdex;
        ContentKey = contentKey;
    }
}

public enum DiaState
{
    Start = 1,
    Clossed = 2
}