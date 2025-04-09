using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : SlotBase
{
    [Header("持有宝石&&InnerSlot的重要数据")]
    ItemData _curItemData;
    public ItemData CurItemData
    {
        get => _curItemData;
        set
        {
            if (_curItemData != value)
            {
                _curItemData = value;
                InventoryManager.Instance._BulletInvData.RefreshModifiers();
            }
        }
    }

    public override void SOnDrop(GameObject _childIns)
    {
        base.SOnDrop(_childIns);
        ItemBase curSC = _childIns.GetComponentInChildren<ItemBase>();
        Item _itemNew = curSC as Item;
        //设置一下ToolTips根据不同Slot的偏移
        if (SlotType == SlotType.BagItemSlot)
            _itemNew.ToolTipsOffset = new Vector3(1.01f, -0.5f, 0);
        if (SlotType == SlotType.BagEquipSlot)
            _itemNew.ToolTipsOffset = new Vector3(-0.92f, -0.52f, 0);
        //同步数据
        _itemNew._data.CurSlot = this;
        MainID = _itemNew._data.ID;
        CurItemData = _itemNew._data;//槽位持有数据
        if (SlotType == SlotType.GemBagSlot)
            InventoryManager.Instance._InventoryData.AddItemToBag(CurItemData);
        else
            InventoryManager.Instance._InventoryData.EquipItem(CurItemData);
    }
}
