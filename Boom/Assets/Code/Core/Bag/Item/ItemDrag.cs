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
        if (curItem.waterElement != 0)
            str += "<color=\"red\">水元素: " + curItem.waterElement + "\n";
        if (curItem.fireElement != 0)
            str += $"<color=FF4F00><font=\"NotoSans\" material=\"NotoSans Outline\">火元素: <color=#FFFFFF><size=80%><font=\"Impact SDF\">{curItem.fireElement}\n";
        if (curItem.thunderElement != 0)
            str += "<color=\"red\">雷元素: " + curItem.thunderElement + "\n";
        if (curItem.lightElement != 0)
            str += "<color=\"red\">光元素: " + curItem.lightElement + "\n";
        if (curItem.darkElement != 0)
            str += "<color=\"red\">暗元素: " + curItem.darkElement + "\n";
        
        str += "<color=\"red\">max: " + curItem.maxDamage;
        return str;
    }
}
