using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SlotManager
{
    //根据SlotId 和 SlotType 还原Slot的MainID和InstanceID
    public static void ClearBagSlotByID(int SlotID,SlotType slotType)
    {
        SlotBase curSlot = GetBagSlotByID(SlotID,slotType);
        if (curSlot == null) return;
        curSlot.MainID = -1;
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
                allSlot = UIManager.Instance.BagReadySlotRootGO.GetComponentsInChildren<GemSlot>();
                break;
            case SlotType.CurBulletSlot:
                allSlot = UIManager.Instance.BagReadySlotRootGO.GetComponentsInChildren<BulletSlotRole>();
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
    
    public static SlotBase GetMiniBagSlotByID(int SlotID,SlotType slotType = SlotType.BagSlot)
    {
        SlotBase curTargetSlot = null;
        SlotBase[] allSlot;
      
        switch (slotType)
        {
            case SlotType.GemBagSlot:
                allSlot = UIManager.Instance.BagGemRootGO_Mini.GetComponentsInChildren<SlotBase>();
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
    
    public static SlotBase GetEmptySlot(SlotType slotType)
    {
        SlotBase curTargetSlot = null;
        SlotBase[] allSlot;
      
        switch (slotType)
        {
            case SlotType.GemBagSlot:
                allSlot = UIManager.Instance.BagGemRootGO.GetComponentsInChildren<SlotBase>();
                break;
            case SlotType.GemInlaySlot:
                allSlot = UIManager.Instance.BagReadySlotRootGO.GetComponentsInChildren<GemSlot>();
                break;
            case SlotType.CurBulletSlot:
                allSlot = UIManager.Instance.BagReadySlotRootGO.GetComponentsInChildren<BulletSlotRole>();
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
            if (each.MainID == -1)
            {
                curTargetSlot = each;
                break;
            }
        }
        return curTargetSlot;
    }
    
}
