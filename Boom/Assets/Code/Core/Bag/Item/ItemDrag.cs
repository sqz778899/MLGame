using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDrag : ItemBase
{
    internal override void DropSlot()
    {
        //同步新的Slot信息
        _dragIns.transform.position = _curSlot.transform.position;
        _curSlot.MainID = CurItem.ID;
        _curSlot.InstanceID = InstanceID;
        CurItemSlotType = _curSlot.SlotType;//GO层同步
        //清除旧的Slot信息
        ItemManager.ClearBagSlotByID(SlotID);
        SlotID = _curSlot.SlotID;
        //数据层同步
        SetItemData();
        MainRoleManager.Instance.RefreshAllItems();
    }
    
    internal override void NonFindSlot()
    {
        // 如果没有找到槽位，那么物品回到原始位置
        _dragIns.transform.position = originalPosition;
    }

    internal override void RightClick()
    {
        DisplayRightClickMenu(_eventData);
        Debug.Log("Right!!");
    }
    
    internal override void SetTooltipInfo()
    {
        CommonTooltip curTip = TooltipsGO.GetComponentInChildren<CommonTooltip>();
        string des = GetItemAttriInfo(CurItem);
        curTip.txtTitle.text = CurItem.name;
        curTip.txtDescription.text = des;
        GameObject ThumbnailIns = ItemManager.InstanceItemThumbnailByID(CurItem);
        ThumbnailIns.transform.SetParent(curTip.ThumbnailNode,false);
    }
    
    string GetItemAttriInfo(Item curItem)
    {
        string str = "";
        if (curItem.attribute.waterElement != 0)
            str += $"<color=#32AFE0>水元素: <color=#ECECEC> + {curItem.attribute.waterElement}\n";
        if (curItem.attribute.fireElement != 0)
            str += $"<color=#FF4F00>火元素: <color=#ECECEC> + {curItem.attribute.fireElement}\n";
        if (curItem.attribute.thunderElement != 0)
            str += $"<color=#8927B5>雷元素: <color=#ECECEC> + {curItem.attribute.thunderElement}\n";
        if (curItem.attribute.lightElement != 0)
            str += $"<color=#E7D889>光元素: <color=#ECECEC> + {curItem.attribute.lightElement}\n";
        if (curItem.attribute.darkElement != 0)
            str += $"<color=#2F985B>暗元素: <color=#ECECEC> + {curItem.attribute.darkElement}\n";
        
        if (curItem.attribute.extraWaterDamage != 0)
            str += $"<color=#32AFE0>额外水伤害: <color=#ECECEC> + {curItem.attribute.extraWaterDamage}%\n";
        if (curItem.attribute.extraFireDamage != 0)
            str += $"<color=#FF4F00>额外火伤害: <color=#ECECEC> + {curItem.attribute.extraFireDamage}%\n";
        if (curItem.attribute.extraThunderDamage != 0)
            str += $"<color=#8927B5>额外雷伤害: <color=#ECECEC> + {curItem.attribute.extraThunderDamage}%\n";
        if (curItem.attribute.extraLightDamage != 0)
            str += $"<color=#E7D889>额外光伤害: <color=#ECECEC> + {curItem.attribute.extraLightDamage}%\n";
        if (curItem.attribute.extraDarkDamage != 0)
            str += $"<color=#2F985B>额外暗伤害: <color=#ECECEC> + {curItem.attribute.extraDarkDamage}%\n";
        
        str += $"<color=#ECECEC>总伤害: <color=#ECECEC> + {curItem.attribute.maxDamage}%\n";
        return str;
    }
}
