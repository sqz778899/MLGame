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
        BagItemTools<Gem>.AddObjectGO(newGemData);//在OnDrop中添加到数据层
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
        _BulletInvData.InitData();
    }
    #endregion 
}