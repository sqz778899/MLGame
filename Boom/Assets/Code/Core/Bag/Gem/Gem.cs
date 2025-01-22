using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gem : DragBase
{
    public int ID;
    public int InstanceID;
    public GemAttribute Attribute;
    
    //游戏内相关属性
    public int slotID;
    public SlotType slotType;
    
    //
    int preID;
    void Start()
    {
        SyncData();
    }
    void Update()
    {
        if (ID != preID)
            SyncData();
    }

    public void SyncData()
    {
        preID = ID;
        Attribute.SyncAttribute(ID);
        Debug.Log(PathConfig.GetGemPath(Attribute.ImageName));
        InstanceID = gameObject.GetInstanceID();
        GetComponent<Image>().sprite = ResManager.instance.GetAssetCache<Sprite>(
            PathConfig.GetGemPath(Attribute.ImageName));
    }
}
