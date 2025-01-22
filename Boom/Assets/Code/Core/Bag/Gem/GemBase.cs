using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GemAttribute
{
    public int Damage;
    public int Piercing;
    public int Resonance;
    public string ImageName;
    
    public GemAttribute(int damage = 0, int piercing = 0, int resonance = 0)
    {
        Damage = damage;
        Piercing = piercing;
        Resonance = resonance;
        ImageName = "";
    }

    public void SyncAttribute(int ID)
    {
        foreach (var each in TrunkManager.Instance.GemDesignJsons)
        {
            if(each.ID == ID)
            {
                Damage = each.Attribute.Damage;
                Piercing = each.Attribute.Piercing;
                Resonance = each.Attribute.Resonance;
                ImageName = each.ImageName;
                break;
            }
        }
    }
}

[Serializable]
public class GemJson
{
    public int ID;
    public int InstanceID;
    public string Name;
    public GemAttribute Attribute;
    public string ImageName;
    
    //游戏内相关属性
    public int slotID;
    public int slotType;
    
    public GemJson()
    {
        ID = -1;
        InstanceID = -1;
        Name = "";
        Attribute = new GemAttribute();
        ImageName = "";
        slotID = -1;
        slotType = -1;
    }
}