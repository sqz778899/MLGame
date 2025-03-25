using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public InventoryData _InventoryData;
    public BulletInvData _BulletInvData;

    public void ClearInventoryData()
    {
        _InventoryData.ClearData();
        _BulletInvData.ClearData();
        InitAllBagGO();
    }
    
    public void InitAllBagGO()
    {
        BagItemTools<ItemBase>.ClearAllObject();
        InitEquipBullets();
        InitSpawners();
        InitItemGem();
    }

    public void AddItemToBag(int itemID)
    {
        ItemData newItemData = new ItemData(itemID, SlotManager.GetEmptySlot(SlotType.BagItemSlot));
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
    
    #region 初始化所有宝石道具资产
    void InitItemGem()
    {
        foreach (var gem in _InventoryData.BagGems)
            BagItemTools<Gem>.InitSaveFileObject(gem, SlotType.GemBagSlot);
        foreach (var gem in _InventoryData.EquipGems)
            BagItemTools<Gem>.InitSaveFileObject(gem, SlotType.GemInlaySlot);
        
        foreach (var item in _InventoryData.BagItems)
            BagItemTools<Item>.InitSaveFileObject(item, SlotType.BagItemSlot);
        foreach (var item in _InventoryData.EquipItems)
            BagItemTools<Item>.InitSaveFileObject(item, SlotType.BagEquipSlot);
    }
    #endregion
    
    #region 初始化所有子弹资产
    //初始化当前装备的子弹
    void InitEquipBullets()
    {
        //...............Clear Old Data....................
        Bullet[] oldBullets = UIManager.Instance.BagUI.EquipBulletSlotRoot.GetComponentsInChildren<Bullet>();
        for (int i = oldBullets.Length - 1; i >= 0; i--)
            Destroy(oldBullets[i].gameObject);
        SlotManager.GetEmptySlot(SlotType.CurBulletSlot);
        //..............Instance New Data..................
        foreach (BulletData each in _BulletInvData.EquipBullets)
        {
            GameObject BulletIns = BulletFactory.CreateBullet(each, BulletInsMode.EditB).gameObject;
            each.CurSlot.SOnDrop(BulletIns);
        }
    }
    
    //初始化子弹孵化器
    void InitSpawners()
    {
        //..............Clear Old Data..................
        GameObject spawnerSlotRoot = UIManager.Instance.BagUI.SpawnerSlotRoot;
        GameObject SpawnerSlotRootMini = UIManager.Instance.BagUI.SpawnerSlotRootMini;
        DraggableBulletSpawner[] oldSpawner = spawnerSlotRoot.GetComponentsInChildren<DraggableBulletSpawner>();
        for (int i = oldSpawner.Length - 1; i >= 0; i--)
            Destroy(oldSpawner[i].gameObject);
        //..............Instance New Data..................
        BulletSlot[] slots = spawnerSlotRoot.GetComponentsInChildren<BulletSlot>();
        BulletSlot[] slotMinis = SpawnerSlotRootMini.GetComponentsInChildren<BulletSlot>();
        InitSpawnersSingel(slots);
        InitSpawnersSingel(slotMinis,true);
    }
    
    void InitSpawnersSingel(BulletSlot[] slots, bool isMini = false)
    {
        foreach (BulletData each in _BulletInvData.BagBulletSpawners)
        {
            int curSpawnerFindID = each.ID % 10;
            var slot = slots.FirstOrDefault(s => s.SlotID == curSpawnerFindID);
            if (slot != null)
            {
                slot.MainID = each.ID;
                GameObject newSpawnerIns = null;
                if (isMini)
                    newSpawnerIns = BulletFactory.CreateBullet(each, BulletInsMode.SpawnerInner).gameObject;
                else
                    newSpawnerIns = BulletFactory.CreateBullet(each, BulletInsMode.Spawner).gameObject;
                newSpawnerIns.transform.SetParent(slot.gameObject.transform, false);
            }
        }
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
        _BulletInvData.InitData();
    }
    #endregion 
}