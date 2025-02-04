using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SlotManager
{
    //根据SlotId 和 SlotType 还原Slot的MainID和InstanceID
    public static void ClearBagSlotByID(int SlotID,SlotType slotType)
    {
        SlotBase curSlot = GetBagSlotByID(SlotID,slotType);
        curSlot.MainID = -1;
        curSlot.InstanceID = -1;
    }
    
    //根据SlotId 和 SlotType 拿到背包的Slot
    public static SlotBase GetBagSlotByID(int SlotID,SlotType slotType = SlotType.BagSlot)
    {
        SlotBase curTargetSlot = null;
        SlotBase[] allSlot;
        switch (slotType)
        {
            case SlotType.GemBagSlot:
                allSlot = UIManager.Instance.BagGemRootGO.GetComponentsInChildren<SlotBase>();
                break;
            case SlotType.GemInlaySlot:
                allSlot = UIManager.Instance.BagReadySlotRootGO.GetComponentsInChildren<SlotBase>();
                break;
            case SlotType.ElementSlot:
                allSlot = UIManager.Instance.EquipItemRootGO.GetComponentsInChildren<SlotBase>();
                break;
            case SlotType.BagSlot:
                allSlot = UIManager.Instance.BagItemRootGO.GetComponentsInChildren<SlotBase>();
                break;
            default:
                allSlot = UIManager.Instance.BagItemRootGO.GetComponentsInChildren<SlotBase>();
                break;
        }
        foreach (var each in allSlot)
        {
            if (each.SlotID == SlotID)
            {
                curTargetSlot = each;
                break;
            }
        }
        return curTargetSlot;
    }
    
}
