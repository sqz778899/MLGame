using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SlotManager
{
    //根据SlotId 和 SlotType 还原Slot的MainID和InstanceID
    public static void ClearSlotByID(int SlotID,SlotType slotType)
    {
        SlotBase curSlot = GetSlot(SlotID,slotType);
        if (curSlot == null) return;
        curSlot.MainID = -1;
    }
    
    public static void ClearSlot(SlotBase curSlot)
    {
        if (curSlot == null) return;
        curSlot.MainID = -1;
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
        SlotBase[] allSlot = GetCurSlotArray(slotType);
        SlotBase curTargetSlot = allSlot.FirstOrDefault(each => each.MainID == -1);
        return curTargetSlot;
    }

    //根据SlotID 和SlotType精准定位到具体的Slot
    public static SlotBase GetSlot(int SlotID, SlotType slotType)
    {
        SlotBase[] allSlot = GetCurSlotArray(slotType);
        SlotBase curTargetSlot = allSlot.FirstOrDefault(each => each.SlotID == SlotID);
        return curTargetSlot;
    }

    #region 不需要关心的私有方法
    static SlotBase[] GetCurSlotArray(SlotType slotType)
    {
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
            case SlotType.SpawnnerSlot:
                allSlot = UIManager.Instance.G_BulletSpawnerSlot.GetComponentsInChildren<SlotBase>();
                break;
            default:
                allSlot = UIManager.Instance.BagItemRootGO.GetComponentsInChildren<SlotBase>();
                break;
        }

        return allSlot;
    }
    #endregion
}
