using System;
using System.Collections.Generic;
using UnityEngine;

public enum ElementalTypes
{
    NonElemental = 1,
    Ice = 2,
    Fire = 3,
    Electric = 4
}

public enum ItemTypes
{
    Bullet = 1
}

//score
//gold
//insignias

[Serializable]
public class Award
{
    public int score;
    public int gold;
    public SupremeCharm supremeCharm;
}

[Serializable]
public class SupremeCharm
{
    public int ID;
    public string name;
    
    public int damage;
    public ElementalTypes elementalTypes;

    public void GetSupremeCharmByID()
    {
        
    }

    public SupremeCharm(int _id)
    {
        ID = _id;
    }
}

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