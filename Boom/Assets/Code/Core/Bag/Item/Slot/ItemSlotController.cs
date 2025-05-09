﻿using UnityEngine;

public class ItemSlotController:BaseSlotController<ItemDataBase>
{
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
        GM.Root.InventoryMgr._InventoryData.AddItemToBag((ItemData)data);
        GM.Root.InventoryMgr._BulletInvData?.RefreshModifiers();
    }
    
    public override void Unassign()
    {
        if (_curData == null) return;
        GM.Root.InventoryMgr._InventoryData.RemoveItemToBag((ItemData)_curData);
        _curData.CurSlotController = null;
        
        _curData = null;
        _view?.Clear();
    }
}