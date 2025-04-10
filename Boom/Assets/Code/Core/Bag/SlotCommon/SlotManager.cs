using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SlotManager
{
    //根据SlotId 和 SlotType 还原Slot的MainID和InstanceID
    public static void ClearSlot(SlotBase curSlot,bool isClearChildIns = false)
    {
        if (curSlot == null) return;
        if (isClearChildIns && curSlot.ChildIns != null)
            GameObject.Destroy(curSlot.ChildIns);
        
        if (curSlot is BulletSlotRole _roleSlot)
            _roleSlot.CurBulletData = null;
        curSlot.MainID = -1;
    }
    
    //获得当前Slot的GameObject
    public static GameObject GetSlotGO(int SlotID, SlotType slotType)
    {
        ISlotController curController = GetSlotController(SlotID, slotType);
        BaseSlotController<ItemDataBase> curBaseController = curController as BaseSlotController<ItemDataBase>;
        return curBaseController._view?.gameObject;
    }
    
    public static ISlotController[] GetAllSlotController(SlotType slotType)
    {
        SlotView[] allSlot = GetCurSlotArraySlotView(slotType);
        ISlotController[] allController = allSlot.Select(each => each.Controller).ToArray();
        return allController;
    }
    
    public static SlotController GetEmptySlotController(SlotType slotType)
    {
        SlotView[] allSlot = GetCurSlotArraySlotView(slotType);
        SlotController curTargetSlot = allSlot.FirstOrDefault(each => each.Controller.IsEmpty).Controller as SlotController;
        return curTargetSlot;
    }
    
    public static ISlotController GetSlotController(int SlotID, SlotType slotType)
    {
        SlotView[] allSlot = GetCurSlotArraySlotView(slotType);
        ISlotController curTargetSlot = allSlot.FirstOrDefault(each => each.Controller.SlotID == SlotID).Controller;
        return curTargetSlot;
    }

    public static SlotView[] GetAllSlotBase()
    {
        SlotView[] allItemSlot = EternalCavans.Instance.ItemRoot.GetComponentsInChildren<SlotView>();
        SlotView[] allEuipItemSlot = EternalCavans.Instance.EquipItemRoot.GetComponentsInChildren<SlotView>();
        SlotView[] allGemSlot = EternalCavans.Instance.GemRoot.GetComponentsInChildren<SlotView>();
        SlotView[] allGemInlaySlot = EternalCavans.Instance.EquipBulletSlotRoot.GetComponentsInChildren<SlotView>();
        SlotView[] allBagMiniSlot = EternalCavans.Instance.BagRootMini.GetComponentsInChildren<SlotView>();
        SlotView[] allSlot = allItemSlot.Concat(allEuipItemSlot).Concat(allGemSlot).Concat(allGemInlaySlot).Concat(allBagMiniSlot).ToArray();
        return allSlot;
    }
    
    //宝石交换逻辑
    public static void Swap(SlotController a, SlotController b)
    {
        var dataA = a.CurData;
        var dataB = b.CurData;

        var goA = a.GetGameObject();
        var goB = b.GetGameObject();

        // 分别放入
        a.AssignDirectly(dataB, goB);
        b.AssignDirectly(dataA, goA);
    }
    #region 不需要关心的私有方法
    static SlotView[] GetCurSlotArraySlotView(SlotType slotType)
    {
        SlotView[] allSlot;
        switch (slotType)
        {
            case SlotType.GemBagSlot:
                allSlot = EternalCavans.Instance.GemRoot.GetComponentsInChildren<SlotView>(true);
                break;
            case SlotType.GemInlaySlot:
                allSlot = EternalCavans.Instance.EquipBulletSlotRoot.GetComponentsInChildren<GemSlotView>(true);
                break;
            case SlotType.BulletSlot:
                allSlot = EternalCavans.Instance.SpawnerSlotRoot.GetComponentsInChildren<SlotView>(true);
                break;
            case SlotType.CurBulletSlot:
                allSlot = EternalCavans.Instance.EquipBulletSlotRoot.GetComponentsInChildren<BulletSlotView>(true);
                break;
            case SlotType.BagEquipSlot:
                allSlot = EternalCavans.Instance.EquipItemRoot.GetComponentsInChildren<SlotView>(true);
                break;
            case SlotType.BagItemSlot:
                allSlot = EternalCavans.Instance.ItemRoot.GetComponentsInChildren<SlotView>(true);
                break;
            case SlotType.SpawnnerSlot:
                allSlot = EternalCavans.Instance.SpawnerSlotRoot.GetComponentsInChildren<SlotView>(true);
                break;
            case SlotType.SpawnnerSlotInner:
                allSlot = EternalCavans.Instance.SpawnerSlotRootMini.GetComponentsInChildren<SlotView>(true);
                break;
            default:
                allSlot = EternalCavans.Instance.ItemRoot.GetComponentsInChildren<SlotView>(true);
                break;
        }

        return allSlot;
    }
    #endregion
}
