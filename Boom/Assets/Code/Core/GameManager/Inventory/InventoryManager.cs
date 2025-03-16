using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public InventoryData _InventoryData;
    public BulletInvData _BulletInvData;

    public void AddItemToBag(int itemID)
    {
        ItemData newItemData = new ItemData(itemID, SlotManager.GetEmptySlot(SlotType.BagSlot));
        _InventoryData.AddItemToBag(newItemData);
        BagItemTools<Item>.AddObjectGO(newItemData);
    }
    
    public void AddGemToBag(int gemID)
    {
        GemData newGemData = new GemData(gemID, SlotManager.GetEmptySlot(SlotType.GemBagSlot));
        _InventoryData.AddGemToBag(newGemData);
        BagItemTools<Gem>.AddObjectGO(newGemData);
    }
    
    //把当前子弹，归还给子弹槽
    public void ReturnToSpawner(GameObject bulletGO)
    {
        BulletData _data = bulletGO.GetComponent<Bullet>()._data;
        foreach (var eachBulletData in _BulletInvData.BagBulletSpawners)
        {
            if (eachBulletData.ID == _data.ID)
            {
                _BulletInvData.AddSpawner(_data.ID);
                _BulletInvData.RemoveEquipBullet(_data);
                Destroy(bulletGO);
                SlotManager.ClearSlot(_data.CurSlot);
                break;
            }
        }
    }

    #region 装备相关
    public bool TryEquipItem(ItemData item)
    {
        if (_InventoryData.BagItems.Contains(item))
        {
            _InventoryData.EquipItem(item);
            // 通知UI更新
            //UIManager.Instance.BagUI.Refresh();
            return true;
        }
        return false;
    }

    public bool TryUnEquipItem(ItemData item)
    {
        if (_InventoryData.EquipItems.Contains(item))
        {
            _InventoryData.UnEquipItem(item);
            //UIManager.Instance.BagUI.Refresh();
            return true;
        }
        return false;
    }
    
    public bool TryEquipGem(GemData gem)
    {
        if (_InventoryData.BagGems.Contains(gem))
        {
            _InventoryData.EquipGem(gem);
            // 通知UI更新
            //UIManager.Instance.BagUI.Refresh();
            return true;
        }
        return false;
    }

    public bool TryUnEquipGem(GemData gem)
    {
        if (_InventoryData.EquipGems.Contains(gem))
        {
            _InventoryData.UnEquipGem(gem);
            //UIManager.Instance.BagUI.Refresh();
            return true;
        }
        return false;
    }
    #endregion
    
    #region 单例的加载卸载
    public static InventoryManager Instance { get; private set; }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        
        _InventoryData =  ResManager.instance.GetAssetCache<InventoryData>(PathConfig.InventoryDataPath);
        _BulletInvData =  ResManager.instance.GetAssetCache<BulletInvData>(PathConfig.BulletInvDataPath);
    }
    #endregion 
}