using System;
using System.Collections.Generic;
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
    public List<MiracleOddityData> EquipMiracleOddities = new();

    public void ClearData()
    {
        BagItems.Clear();
        BagGems.Clear();
        EquipItems.Clear();
        EquipGems.Clear();
        EquipMiracleOddities.Clear();
        OnStructureChanged?.Invoke();
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
    #endregion

    #region 奇迹物件操作
    public void EquipMiracleOddity(int id)
    {
        MiracleOddityData data = new MiracleOddityData(id);
        EquipMiracleOddities.Add(data);
        OnStructureChanged?.Invoke();
    }
    
    public void UnEquipMiracleOddity(MiracleOddityData _data)
    {
        EquipMiracleOddities.Remove(_data);
        OnStructureChanged?.Invoke();
    }

    public void ClearMiracleOddity()
    {
        EquipMiracleOddities.Clear();
        OnStructureChanged?.Invoke();
    }
    #endregion
}