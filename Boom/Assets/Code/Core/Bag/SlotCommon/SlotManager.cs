using System.Linq;
using UnityEngine;

public static class SlotManager
{
    //根据SlotId 和 SlotType 还原Slot的MainID和InstanceID
    public static void ClearSlot(ISlotController curSlot)
    {
        if (curSlot == null) return;
        curSlot.Unassign();
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
    
    public static GemSlotController GetEmptySlotController(SlotType slotType)
    {
        SlotView[] allSlot = GetCurSlotArraySlotView(slotType);
        GemSlotController curTargetGemSlot = allSlot.FirstOrDefault(each => each.Controller.IsEmpty).Controller as GemSlotController;
        return curTargetGemSlot;
    }
    
    public static ISlotController GetSlotController(int SlotID, SlotType slotType)
    {
        SlotView[] allSlot = GetCurSlotArraySlotView(slotType);
        ISlotController curTargetSlot = allSlot.FirstOrDefault(each => each.Controller.SlotID == SlotID).Controller;
        return curTargetSlot;
    }

    public static SlotView[] GetAllSlotView()
    {
        SlotView[] allItemSlot = EternalCavans.Instance.ItemRoot.GetComponentsInChildren<SlotView>();
        SlotView[] allEuipItemSlot = EternalCavans.Instance.EquipItemRoot.GetComponentsInChildren<SlotView>();
        SlotView[] allGemSlot = EternalCavans.Instance.GemRoot.GetComponentsInChildren<SlotView>();
        SlotView[] allGemInlaySlot = EternalCavans.Instance.EquipBulletSlotRoot.GetComponentsInChildren<SlotView>();
        SlotView[] allBagMiniSlot = EternalCavans.Instance.BagRootMini.GetComponentsInChildren<SlotView>();
        SlotView[] allSlot = allItemSlot.Concat(allEuipItemSlot).Concat(allGemSlot).Concat(allGemInlaySlot).Concat(allBagMiniSlot).ToArray();
        return allSlot;
    }
    
    //交换逻辑
    public static void Swap(ItemDataBase dtA, ItemDataBase dtB,GameObject curDragGO = null)
    {
        ISlotController a = dtA.CurSlotController;
        ISlotController b = dtB.CurSlotController;
        
        if (a is BulletInnerSlotController && b == null)
        {
            //回退老子弹
            GM.Root.InventoryMgr.ReturnToSpawner(null,a.CurData as BulletData);
            a.Assign(dtB, curDragGO);
        }
        else if (a is BulletInnerSlotController && b is BulletInnerSlotController)
        {
            GM.Root.InventoryMgr.RemoveBulletToFight(a.CurData as BulletData);
            //暂存Data
            var dataA = a.CurData;
            var dataB = b.CurData;
            // 分别放入
            a.AssignDirectly(dataB as BulletData, null);
            b.AssignDirectly(dataA as BulletData, null);
            GameObject.Destroy(curDragGO);
        }
        else
        {
            var dataA = a.CurData;
            var dataB = b.CurData;

            GameObject goA = a.GetGameObject();
            GameObject goB = b.GetGameObject();

            // 分别放入
            a.AssignDirectly(dataB, goB);
            b.AssignDirectly(dataA, goA);   
        }
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
            case SlotType.GemBagSlotInner:
                allSlot = EternalCavans.Instance.GemRootInner.GetComponentsInChildren<SlotView>(true);
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
