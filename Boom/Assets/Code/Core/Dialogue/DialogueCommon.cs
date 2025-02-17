using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DiaSingle
{
    public int ID;
    public string Sign;
    public string Name;
    public int IsLeft;
    public int NextIdex;
    public string Content;

    public DiaSingle(int _id, string _sign, string _name, int _isLeft, int _nextIdex, string _content)
    {
        ID = _id;
        Sign = _sign;
        Name = _name;
        IsLeft = _isLeft;
        NextIdex = _nextIdex;
        Content = _content;
    }
}