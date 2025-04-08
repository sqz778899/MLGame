using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryData: ScriptableObject
{
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
    public void AddGemToBag(GemData gem) => BagGems.Add(gem);
    public void RemoveGemToBag(GemData gem) => BagGems.Remove(gem);
    public void EquipGem(GemData gem) => EquipGems.Add(gem);
    public void UnEquipGem(GemData gem) => EquipGems.Remove(gem);
    #endregion
    
    #region 道具操作
    public void AddItemToBag(ItemData item) => BagItems.Add(item);
    public void AddItemToEquip(ItemData item) => EquipItems.Add(item);
    public void RemoveItem(ItemData itemData) =>BagItems.Remove(itemData);
    public void EquipItem(ItemData item) => EquipItems.Add(item);
    public void UnEquipItem(ItemData item) => EquipItems.Remove(item);
    #endregion
}