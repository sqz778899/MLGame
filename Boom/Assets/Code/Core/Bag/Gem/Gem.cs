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
        switch (CurSlot.SlotType)
        {
            case SlotType.GemBagSlot:
                IsInLay = false;
                BulletSlotIndex = -1;
                CurSlot.SOnDrop(_dragIns,SlotType.GemBagSlot);
                break;
            case SlotType.GemInlaySlot:
                IsInLay = true;
                GemSlot gemSlot = (GemSlot)CurSlot;
                BulletSlotIndex = gemSlot.BulletSlotIndex;
                CurSlot.SOnDrop(_dragIns,SlotType.GemInlaySlot);
                break;
            default:
                IsInLay = false;
                BulletSlotIndex = -1;
                CurSlot.SOnDrop(_dragIns,SlotType.GemBagSlot);
                break;
        }
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
