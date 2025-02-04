using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Gem : DragBase
{
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
    
    internal override void VOnDrop()
    {
        //同步新的Slot信息
        _curSlot.MainID = ID;
        _curSlot.InstanceID = InstanceID;
        SlotType = _curSlot.SlotType;//GO层同步
        SlotID = _curSlot.SlotID;
        //
        switch (_curSlot.SlotType)
        {
            case SlotType.GemBagSlot:
                IsInLay = false;
                BulletSlotIndex = -1;
                SlotManager.ClearBagSlotByID(SlotID,SlotType.GemBagSlot);//清除旧的Slot信息
                break;
            case SlotType.GemInlaySlot:
                IsInLay = true;
                GemSlot gemSlot = (GemSlot)_curSlot;
                BulletSlotIndex = gemSlot.BulletSlotIndex;
                SlotManager.ClearBagSlotByID(SlotID,SlotType.GemInlaySlot);
                break;
        }
        //数据层同步
        //MainRoleManager.Instance.MoveGem(this);
        //SetItemData();
        MainRoleManager.Instance.RefreshAllItems();
    }
    
    protected override void OnIDChanged()
    {
        SyncData();
    }

    #region ToolTips相关
    internal override void SetTooltipInfo()
    {
        CommonTooltip curTip = TooltipsGO.GetComponentInChildren<CommonTooltip>();
        string des = GetItemAttriInfo();
        curTip.txtTitle.text = Name;
        curTip.txtDescription.text = des;
        curTip.ImgThumbnail.sprite = ItemSprite.sprite;
    }

    string GetItemAttriInfo()
    {
        List<string> attributes = new List<string>();
        if (Attribute.Damage != 0) attributes.Add($"伤害 + {Attribute.Damage}");
        if (Attribute.Resonance != 0) attributes.Add($"共振 + {Attribute.Resonance}");
        if (Attribute.Piercing != 0) attributes.Add($"穿透 + {Attribute.Piercing}");

        return string.Join("\n", attributes);
    }
    #endregion
    
    #region 同步各种数据
    //数据同步
    public override void SyncData()
    {
        InstanceID = gameObject.GetInstanceID();
        GemJson gemDesignData = ToJosn();
        Name = gemDesignData.Name;
        Attribute = gemDesignData.Attribute;
        if (ItemSprite == null) ItemSprite = GetComponent<Image>();
        ItemSprite.sprite = ResManager.instance.GetAssetCache<Sprite>(
            PathConfig.GetGemPath(gemDesignData.ImageName));
    }
    
    public GemJson ToJosn()
    {
        GemJson gemJson = TrunkManager.Instance.GemDesignJsons
            .FirstOrDefault(each => each.ID == ID) ?? new GemJson();
        
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
