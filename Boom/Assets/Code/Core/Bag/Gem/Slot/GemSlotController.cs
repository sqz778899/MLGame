﻿using UnityEngine;

public class GemSlotController : BaseSlotController<ItemDataBase>
{
    //持有的视窗数据
    public GemInnerSlotController LinkedGemInnerSlotController; // 指向关联的 GemSlotInner 的 controller
    
    public bool IsEmpty => CurData == null; //槽位是不是空的
    public bool IsLocked; //槽位是不是锁定的

    public override bool CanAccept(ItemDataBase data)
    {
        if (IsLocked || data == null) return false;

        switch (SlotType)
        {
            case SlotType.GemBagSlot:
            case SlotType.GemInlaySlot:
                return data is GemData;
            case SlotType.BagItemSlot:
            case SlotType.BagEquipSlot:
                return data is ItemData;
        }
        return false;
    }

    public override void Assign(ItemDataBase data, GameObject itemGO)
    {
        // step 1: 卸载原槽位
        GemSlotController from = data.CurSlotController as GemSlotController;
        itemGO.transform.SetParent(DragManager.Instance.dragRoot.transform);
        from?.Unassign();
        from?.LinkedGemInnerSlotController?.Unassign();

        AssignDirectly(data, itemGO);
    }
    
    public override void AssignDirectly(ItemDataBase data, GameObject itemGO)
    {
        //赋值自己
        _curData = data;
        _curData.CurSlotController = this;
        CachedGO = itemGO;
        
        //UI更新
        _view?.Display(itemGO);
        
        //创建影子
        if (data is GemData gemData && LinkedGemInnerSlotController != null)
        {
            var shadow = BagItemTools<GemInnerNew>.CreateTempObjectGO(gemData, CreateItemType.MiniBagGem);
            shadow.GetComponent<GemInnerNew>().SourceGem = itemGO.GetComponent<GemNew>();
            LinkedGemInnerSlotController.Assign(gemData, shadow);
        }
        
        //刷新数据层
        switch (SlotType)
        {
            case SlotType.GemBagSlot:
                GM.Root.InventoryMgr._InventoryData.AddGemToBag((GemData)data);
                break;
            case SlotType.GemInlaySlot:
                GM.Root.InventoryMgr._InventoryData.EquipGem((GemData)data);
                break;
        }
        GM.Root.InventoryMgr._BulletInvData?.RefreshModifiers();
    }

    public override void Unassign()
    {
        if (_curData == null) return;

        switch (SlotType)
        {
            case SlotType.GemBagSlot:
                GM.Root.InventoryMgr._InventoryData.RemoveGemToBag((GemData)_curData);
                break;
            case SlotType.GemInlaySlot:
                GM.Root.InventoryMgr._InventoryData.UnEquipGem((GemData)_curData);
                break;
        }
        
        _curData.CurSlotController = null;
        
        _curData = null;
        _view?.Clear();
    }
}
public enum SlotType
{
    SpawnnerSlot = 0,
    BagItemSlot = 1,
    BulletSlot = 2,
    BagEquipSlot = 3,
    GemBagSlot = 4,
    GemInlaySlot = 5,
    CurBulletSlot = 6,
    BulletInnerSlot = 7,
    SpawnnerSlotInner = 8,
}
