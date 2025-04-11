using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public InventoryData _InventoryData;
    public BulletInvData _BulletInvData;
    
    public List<BulletInner> CurBulletsInFight;
    
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
        ItemData newItemData = new ItemData(itemID, SlotManager.GetEmptySlotController(SlotType.BagItemSlot));
        _InventoryData.AddItemToBag(newItemData);
        BagItemTools<Item>.AddObjectGO(newItemData);
    }
    
    public void AddGemToBag(int gemID)
    {
        GemSlotController emptyGemSlotController = SlotManager.GetEmptySlotController(SlotType.GemBagSlot);
        GemData newGemData = new GemData(gemID, emptyGemSlotController);
        BagItemTools<GemNew>.AddObjectGO(newGemData);//在OnDrop中添加到数据层
    }

    #region 子弹的一些外部操作
    public void AddBulletToFight(BulletData bulletData) =>
        CurBulletsInFight.Add(BulletFactory.CreateBullet(bulletData, BulletInsMode.Inner) as BulletInner);
    
    public void RemoveBulletToFight(GameObject bulletInnerGO)
    {
        BulletInner bulletSC = bulletInnerGO.GetComponent<BulletInner>();
        CurBulletsInFight.Remove(bulletSC);
        Destroy(bulletInnerGO);
    }
    
    public void RemoveBulletToFight(BulletData bulletData)
    {
        BulletInner bulletSC = GetBulletInnerFormFight(bulletData);
        CurBulletsInFight.Remove(bulletSC);
        Destroy(bulletSC.gameObject);
    }

    //在开始战斗的时候，根据角色槽位的子弹，创建五个跟着他跑的傻逼嘻嘻的小子弹
    public void CreateAllBulletToFight()
    {
        //清空子弹
        CurBulletsInFight.RemoveAll(bullet => bullet == null);
        if (CurBulletsInFight.Count > 0)
        {
            for (int i = 0; i < CurBulletsInFight.Count; i++)
                Destroy(CurBulletsInFight[i].gameObject);
        }
        CurBulletsInFight.Clear();
        //创建子弹
        List<BulletData> CurBullets = _BulletInvData.EquipBullets;
        for (int i = 0; i < CurBullets.Count; i++)
            AddBulletToFight(CurBullets[i]);
    }
    //把当前子弹，归还给子弹槽
    public void ReturnToSpawner(GameObject tmpBulletGO,BulletData _data)
    {
        _BulletInvData.AddSpawner(_data.ID);
        _BulletInvData.UnEquipBullet(_data);
        //把战场数据也删除掉
        BulletInner bulletSC = GetBulletInnerFormFight(_data);
        if (bulletSC != null)
        {
            CurBulletsInFight.Remove(bulletSC);
            Destroy(bulletSC.gameObject);
        }
       
        Destroy(tmpBulletGO);
        SlotManager.ClearSlot(_data.CurSlotController);
    }
    
    BulletInner GetBulletInnerFormFight(BulletData bulletData) =>
        CurBulletsInFight.FirstOrDefault(b => b._data == bulletData);
      
    
    #endregion
    
    #region 初始化所有宝石道具资产
    void InitItemGem()
    {
        //初始化宝石
        List<GemData> tempGem = _InventoryData.BagGems
            .Concat(_InventoryData.EquipGems)
            .Select(curData => new GemData(curData.ID, curData.CurSlotController as GemSlotController))
            .ToList();
        _InventoryData.BagGems.Clear();
        _InventoryData.EquipGems.Clear();
        tempGem.ForEach(gem=>  BagItemTools<GemNew>.InitSaveFileObject(gem, gem.CurSlotController.SlotType));
        //初始化道具
        List<ItemData> tempItem = _InventoryData.BagItems
            .Concat(_InventoryData.EquipItems)
            .Select(curData => new ItemData(curData.ID, curData.CurSlotController as GemSlotController))
            .ToList();
        _InventoryData.BagItems.Clear();
        _InventoryData.EquipItems.Clear();
        tempItem.ForEach(item=>  BagItemTools<Item>.InitSaveFileObject(item, item.CurSlotController.SlotType));
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

        CurBulletsInFight = new List<BulletInner>();
        _InventoryData =  ResManager.instance.GetAssetCache<InventoryData>(PathConfig.InventoryDataPath);
        _BulletInvData =  ResManager.instance.GetAssetCache<BulletInvData>(PathConfig.BulletInvDataPath);
        _BulletInvData.InitData();
    }
    #endregion 
}