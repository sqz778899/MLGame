using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItermBase : DragBase
{
    Item CurItem;
    public int ItermID;
    public int InstanceID;
    public SlotType CurItemSlotType;
    
    public void SetItemData()
    {
        CurItem.slotID = _curSlot.SlotID;
        CurItem.slotType = (int)_curSlot.SlotType;
        //MainRoleManager.RefreshCurItems();
    }
}
