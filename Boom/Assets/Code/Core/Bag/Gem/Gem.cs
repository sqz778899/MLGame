using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Gem : DragBase
{
    public int Level;
    public GemAttribute Attribute;
    //是否镶嵌
    public bool IsInLay;
    public int BulletSlotIndex;
    public Image ItemSprite;
    
    internal override void Start()
    {
        base.Start();
        SyncData();
    }
    
    //落下空槽逻辑
    public override void OnDropEmptySlot()
    {
        switch (CurSlot.SlotType)
        {
            case SlotType.GemBagSlot:
                IsInLay = false;
                BulletSlotIndex = -1;
                CurSlot.SOnDrop(_dragIns);
                ToolTipsOffset = new Vector3(1.01f, -0.5f, 0);
                break;
            case SlotType.GemInlaySlot:
                IsInLay = true;
                GemSlot gemSlot = (GemSlot)CurSlot;
                BulletSlotIndex = gemSlot.BulletSlotIndex;
                CurSlot.SOnDrop(_dragIns);
                ToolTipsOffset = new Vector3(-0.92f, -0.52f, 0);
                break;
            default:
                break;
        }
        MainRoleManager.Instance.RefreshAllItems();
    }
    //落下交换逻辑
    internal override void OnDropFillSlot(SlotBase targetSlot)
    {
        //先把目标槽位的物品拿出来
        GameObject tagetChildIns = targetSlot.ChildIns;
        //tagetChildIns.GetComponent<Gem>().SlotType = CurSlot.SlotType;
        CurSlot.SOnDrop(tagetChildIns);
        //再把自己放进去
        targetSlot.SOnDrop(_dragIns);
        //SlotType = targetSlot.SlotType;
        MainRoleManager.Instance.RefreshAllItems();
    }
    //双击逻辑
    internal override void DoubleClick()
    {
        GemSlot curEmptySlot = null;
        if (CurSlot.SlotType == SlotType.GemBagSlot)
            curEmptySlot = MainRoleManager.Instance.GetEmptyGemSlot();//在角色栏找到一个空的GemSlot
        else if(CurSlot.SlotType == SlotType.GemInlaySlot)
            curEmptySlot = MainRoleManager.Instance.GetEmptyBagGemSlot();//在角色栏找到一个空的GemSlot
        
        if (!curEmptySlot) return;
        
        SlotManager.ClearBagSlotByID(SlotID,CurSlot.SlotType);//清除旧的Slot信息
        CurSlot = curEmptySlot;//再换Slot信息
        OnDropEmptySlot();
    }
    //右击逻辑
    internal override void RightClick()
    {
        if (CurSlot.SlotType == SlotType.GemBagSlot)
            DisplayRightClickMenu(_eventData);
        else if (CurSlot.SlotType == SlotType.GemInlaySlot)//如果是镶嵌的宝石.右键直接卸下
            DoubleClick();
    }
    
    protected  void OnIDChanged() =>  SyncData();

    #region ToolTips相关
    internal override void SetTooltipInfo()
    {
        ToolTipsInfo curToolTipsInfo = new ToolTipsInfo(Name,Level);
        if (Attribute.Damage != 0)
        {
            ToolTipsAttriSingleInfo curInfo = new ToolTipsAttriSingleInfo(
                ToolTipsAttriType.Damage, Attribute.Damage);
            curToolTipsInfo.AttriInfos.Add(curInfo);
        }
        if (Attribute.Piercing != 0)
        {
            ToolTipsAttriSingleInfo curInfo = new ToolTipsAttriSingleInfo(
                ToolTipsAttriType.Piercing, Attribute.Piercing);
            curToolTipsInfo.AttriInfos.Add(curInfo);
        }
        if (Attribute.Resonance != 0)
        {
            ToolTipsAttriSingleInfo curInfo = new ToolTipsAttriSingleInfo(
                ToolTipsAttriType.Resonance, Attribute.Resonance);
            curToolTipsInfo.AttriInfos.Add(curInfo);
        }
        CurTooltipsSC.SetInfo(curToolTipsInfo);
    }
    #endregion
    
    #region 同步各种数据
    //数据同步
    public override void SyncData()
    {
        InstanceID = gameObject.GetInstanceID();
        GemJson gemDesignData = ToJosn();
        Level = gemDesignData.Level;
        Name = gemDesignData.Name;
        Attribute = gemDesignData.Attribute;
        if (ItemSprite == null) ItemSprite = GetComponent<Image>();
        ItemSprite.sprite = ResManager.instance.GetAssetCache<Sprite>(
            PathConfig.GetGemPath(gemDesignData.ImageName));
    }
    
    public GemJson ToJosn()
    {
        GemJson gemJson = new GemJson();
        GemJson gemDesignJson = TrunkManager.Instance.GemDesignJsons
            .FirstOrDefault(each => each.ID == ID) ?? new GemJson();

        gemJson.CopyFrom(gemDesignJson);
        //同步在游戏内的变量
        gemJson.InstanceID = InstanceID;
        gemJson.SlotID = SlotID;
        gemJson.SlotType = (int)SlotType;
        gemJson.BulletSlotIndex = BulletSlotIndex;
        gemJson.IsInLay = IsInLay;
        return gemJson;
    }
    #endregion
}
