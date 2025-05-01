using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryData: ScriptableObject
{
    public event Action OnStructureChanged;
    // 存放在背包内的道具和宝石
    public List<ItemData> BagItems = new();
    public List<GemData> BagGems = new();

    // 已装备的道具和宝石
    public List<ItemData> EquipItems = new();
    public List<GemData> EquipGems = new();

    public void ClearData()
    {
        BagItems.Clear();
        BagGems.Clear();
        EquipItems.Clear();
        EquipGems.Clear();
    }

    #region 宝石操作
    public void AddGemToBag(GemData gem)
    {
        gem.ReturnGemType();
        BagGems.Add(gem);
    }

    public void RemoveGemToBag(GemData gem) => BagGems.Remove(gem);
    public void EquipGem(GemData gem)
    {
        EquipGems.Add(gem);
        OnStructureChanged?.Invoke();
    }

    public void UnEquipGem(GemData gem)
    {
        EquipGems.Remove(gem);
        OnStructureChanged?.Invoke();
    }
    #endregion
    
    #region 道具操作
    public void AddItemToBag(ItemData item)
    {
        if (EquipItems.Contains(item))
            EquipItems.Remove(item);
        BagItems.Add(item);
        OnStructureChanged?.Invoke();
    }

    public void RemoveItemToBag(ItemData itemData) => BagItems.Remove(itemData);
    public void EquipItem(ItemData item)
    {
        if (BagItems.Contains(item))
            BagItems.Remove(item);
        EquipItems.Add(item);
        OnStructureChanged?.Invoke();
    }

    public void UnEquipItem(ItemData item)
    {
        EquipItems.Remove(item);
        OnStructureChanged?.Invoke();
    }
    
    /// 查找背包内的可堆叠道具
    public ItemData FindStackableItem(int id) =>  BagItems.FirstOrDefault(item => item.ID == id && item.IsStackable);
    #endregion
}