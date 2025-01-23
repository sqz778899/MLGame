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
    public SlotType CurGemInSlotType;//此时宝石所在的Slot类型
    
    //
    int preID;
    internal override void Start()
    {
        base.Start();
        SyncData();
    }
    void Update()
    {
        if (ID != preID)
            SyncData();
    }
    
    internal override void VOnDrop()
    {
        //同步新的Slot信息
        _dragIns.transform.position = _curSlot.transform.position;
        _curSlot.MainID = ID;
        _curSlot.InstanceID = InstanceID;
        CurGemInSlotType = _curSlot.SlotType;//GO层同步
        //清除旧的Slot信息
        //ItemManager.ClearBagSlotByID(SlotID);
        //SlotID = _curSlot.SlotID;
        //数据层同步
        //SetItemData();
        MainRoleManager.Instance.RefreshAllItems();
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
