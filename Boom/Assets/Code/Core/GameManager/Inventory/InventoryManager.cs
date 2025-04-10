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
        //读取天赋数据，看看有无初始携带类天赋
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
        //ItemData newItemData = new ItemData(itemID, SlotManager.GetEmptySlotController(SlotType.BagItemSlot));
        //_InventoryData.AddItemToBag(newItemData);
        //BagItemTools<Item>.AddObjectGO(newItemData);
    }
    
    public void AddGemToBag(int gemID)
    {
        SlotController emptySlotController = SlotManager.GetEmptySlotController(SlotType.GemBagSlot);
        GemData newGemData = new GemData(gemID, emptySlotController);
        BagItemTools<GemNew>.AddObjectGO(newGemData);//在OnDrop中添加到数据层
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
        //初始化宝石
        List<GemData> tempGem = _InventoryData.BagGems
            .Concat(_InventoryData.EquipGems)
            .Select(curData => new GemData(curData.ID, curData.CurSlotController as SlotController))
            .ToList();
        _InventoryData.BagGems.Clear();
        _InventoryData.EquipGems.Clear();
        tempGem.ForEach(gem=>  BagItemTools<GemNew>.InitSaveFileObject(gem, gem.CurSlotController.SlotType));
        //初始化道具
        List<ItemData> tempItem = _InventoryData.BagItems
            .Concat(_InventoryData.EquipItems)
            .Select(curData => new ItemData(curData.ID, curData.CurSlotController as SlotController))
            .ToList();
        _InventoryData.BagItems.Clear();
        _InventoryData.EquipItems.Clear();
        tempItem.ForEach(item=>  BagItemTools<Item>.InitSaveFileObject(item, item.CurSlot.SlotType));
    }
    #endregion
    
    #region 初始化所有子弹资产
    //初始化当前装备的子弹
    void InitEquipBullets()
    {
        //...............Clear Old Data....................
        Bullet[] oldBullets = EternalCavans.Instance.EquipBulletSlotRoot.GetComponentsInChildren<Bullet>();
        for (int i = oldBullets.Length - 1; i >= 0; i--)
            Destroy(oldBullets[i].gameObject);
        //..............Instance New Data..................
        foreach (BulletData each in _BulletInvData.EquipBullets)
        {
            GameObject BulletIns = BulletFactory.CreateBullet(each, BulletInsMode.EditB).gameObject;
            each.CurSlotController.Assign(each,BulletIns);
        }
    }
    
    //初始化子弹孵化器
    void InitSpawners()
    {
        //..............Clear Old Data..................
        GameObject spawnerSlotRoot = EternalCavans.Instance.SpawnerSlotRoot;
        GameObject SpawnerSlotRootMini = EternalCavans.Instance.SpawnerSlotRootMini;
        BulletSpawnerNew[] oldSpawner = spawnerSlotRoot.GetComponentsInChildren<BulletSpawnerNew>(true);
        for (int i = oldSpawner.Length - 1; i >= 0; i--)
            Destroy(oldSpawner[i].gameObject);
        BulletSpawnerNew[] oldSpawnerMini = SpawnerSlotRootMini.GetComponentsInChildren<BulletSpawnerNew>(true);
        for (int i = oldSpawnerMini.Length - 1; i >= 0; i--)
            Destroy(oldSpawnerMini[i].gameObject);
        //..............Instance New Data..................
        ISlotController[] slots = SlotManager.GetAllSlotController(SlotType.SpawnnerSlot);
        ISlotController[] slotMinis = SlotManager.GetAllSlotController(SlotType.SpawnnerSlotInner);
        InitSpawnersSingel(slots);
        InitSpawnersSingel(slotMinis,true);
    }
    
    void InitSpawnersSingel(ISlotController[] slots, bool isMini = false)
    {
        foreach (BulletData each in _BulletInvData.BagBulletSpawners)
        {
            int curSpawnerFindID = each.ID % 10;
            ISlotController slot = slots.FirstOrDefault(s => s.SlotID == curSpawnerFindID);
            if (slot.IsEmpty)
            {
                GameObject newSpawnerIns = null;
                if (isMini)
                    newSpawnerIns = BulletFactory.CreateBullet(each, BulletInsMode.SpawnerInner).gameObject;
                else
                    newSpawnerIns = BulletFactory.CreateBullet(each, BulletInsMode.Spawner).gameObject;
                slot.Assign(each,newSpawnerIns);
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