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
        curTip.SyncInfo(CurItem.ID,TipTypes.Item);
    }
}
