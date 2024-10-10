using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItermBase : DragBase
{
    public Item CurItem;
    public int InstanceID;
    public int SlotID; //物品此刻的Slot位置，如果有移动的话，需要清除之前的Slot信息
    public SlotType CurItemSlotType;
    
    public void SetItemData()
    {
        CurItem.slotID = _curSlot.SlotID;
        CurItem.slotType = (int)_curSlot.SlotType;
    }
    
    public void SetItemData(SlotBase slot)
    {
        CurItem.slotID = slot.SlotID;
        CurItem.slotType = (int)slot.SlotType;
    }
}
