using UnityEngine;

public class ItemSlotController:BaseSlotController<ItemDataBase>
{
    public bool IsEmpty => CurData == null; //槽位是不是空的
    public bool IsLocked; //槽位是不是锁定的
    
    public override bool CanAccept(ItemDataBase data)
    {
        if (IsLocked || data == null) return false;
        //不允许 Persistent 道具进装备槽
        if (SlotType == SlotType.ItemEquipSlot && data is ItemData item && item.IsPersistent)
            return false;
        return data is ItemData;
    }
    
    public override void Assign(ItemDataBase data, GameObject itemGO)
    {
        // step 1: 卸载原槽位
        ItemSlotController from = data.CurSlotController as ItemSlotController;
        itemGO.transform.SetParent(DragManager.Instance.dragRoot.transform);
        from?.Unassign();

        AssignDirectly(data, itemGO);
    }
    
    public override void AssignDirectly(ItemDataBase data, GameObject itemGO,bool isRefreshData =true)
    {
        //赋值自己
        _curData = data;
        _curData.CurSlotController = this;
        CachedGO = itemGO;
        
        //UI更新
        _view?.Display(itemGO);
        
        //刷新数据层
        switch (SlotType)
        {
            case SlotType.ItemBagSlot:
                GM.Root.InventoryMgr._InventoryData.AddItemToBag((ItemData)data);
                break;
            case SlotType.ItemEquipSlot:
                GM.Root.InventoryMgr._InventoryData.EquipItem((ItemData)data);
                break;
        }
        GM.Root.InventoryMgr._BulletInvData?.RefreshModifiers();
    }
    
    public override void Unassign()
    {
        if (_curData == null) return;

        switch (SlotType)
        {
            case SlotType.ItemBagSlot:
                GM.Root.InventoryMgr._InventoryData.RemoveItemToBag((ItemData)_curData);
                break;
            case SlotType.ItemEquipSlot:
                GM.Root.InventoryMgr._InventoryData.UnEquipItem((ItemData)_curData);
                break;
        }
        
        _curData.CurSlotController = null;
        
        _curData = null;
        _view?.Clear();
    }
}