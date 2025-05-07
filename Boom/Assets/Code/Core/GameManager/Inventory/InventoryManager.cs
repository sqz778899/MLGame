using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventoryData _InventoryData;
    public BulletInvData _BulletInvData;
    public MiracleOddityManager MiracleOddityMrg;
    
    public List<BulletInner> CurBulletsInFight;
    public List<BulletSlotController> CurBulletSlotControllers;
    public List<BulletInnerSlotController> CurBulletInnerSlotControllers;

    #region 宝石&道具&奇迹物品等外部操作
    public void AddItemToBag(int itemID, int amount = 1)
    {  
        // 1. 尝试找所有能堆叠的同类道具
        List<ItemData> candidates = _InventoryData.BagItems
            .Where(item => item.ID == itemID && item.IsStackable && item.StackCount < item.MaxStackCount)
            .ToList();
        int remaining = amount;
        
        // 2. 先补充已有的堆叠
        foreach (ItemData item in candidates)
        {
            int space = item.MaxStackCount - item.StackCount;
            int add = Mathf.Min(space, remaining);
            item.StackCount += add;
            //item.OnDataChanged?.Invoke();
            remaining -= add;
            if (remaining <= 0)
                break;
        }
        
        // 3. 如果还有剩余，开新物品
        while (remaining > 0)
        {
            int createAmount = Mathf.Min(remaining, 5); // 每组最多5个
            ItemSlotController slotController = SlotManager.GetEmptySlotController(SlotType.ItemBagSlot) as ItemSlotController;
            ItemData newItemData = new ItemData(itemID, slotController);
            newItemData.StackCount = createAmount;
            _InventoryData.AddItemToBag(newItemData);
            BagItemTools<Item>.AddObjectGO(newItemData);
            remaining -= createAmount;
        }
    }
    public void AddGemToBag(int gemID,int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            GemSlotController emptyGemSlotController = 
                SlotManager.GetEmptySlotController(SlotType.GemBagSlot) as GemSlotController;
            GemData newGemData = new GemData(gemID, emptyGemSlotController);
            BagItemTools<Gem>.AddObjectGO(newGemData);//在OnDrop中添加到数据层
        }
    }
    public void EquipMiracleOddity(int miracleID) => _InventoryData.EquipMiracleOddity(miracleID);
    #endregion

    #region 子弹的一些外部操作
    public void AddBulletToFight(BulletData bulletData)
    {
        BulletInner bulletInnerSC = BulletFactory.CreateBullet(bulletData, BulletInsMode.Inner) as BulletInner;
        bulletInnerSC.view.UpText();
        CurBulletsInFight.Add(bulletInnerSC);
        CurBulletsInFight = CurBulletsInFight
            .OrderBy(bullet => bullet.controller.Data.CurSlotController.SlotID).ToList();
    }
    
    public void SyncMainBulletSlot()
    {
        List<BulletData> cash = new List<BulletData>();
        foreach (var each in _BulletInvData.EquipBullets)
            cash.Add(each);
        for (int i = cash.Count - 1; i >=0; i--)
        {
            BulletData curBullet = cash[i];
            BulletSlotController mainController = CurBulletSlotControllers.
                FirstOrDefault(c => c.SlotID == curBullet.CurSlotController.SlotID);
            Bullet bulletSC = BulletFactory.CreateBullet(curBullet, BulletInsMode.EditA) as Bullet;
            bulletSC.transform.SetParent(DragManager.Instance.dragRoot, false);
            mainController.Assign(curBullet,bulletSC.gameObject);
            TooltipsManager.Instance.Hide();
        }
    }

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
        {
            BulletInnerSlotController innerController = CurBulletInnerSlotControllers.
                FirstOrDefault(c => c.SlotID == i + 1);
            innerController.Assign(CurBullets[i],null);
        }
        
        CurBulletsInFight.ForEach(b=>b.view.ReturnText());
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
        CurBulletsInFight.FirstOrDefault(b => b.controller.Data == bulletData);
    
    //初始化子弹槽的锁定状态
    public void RefreshBulletSlotLockedState()
    {
        for (int i = 0; i < 5; i++)
        {
            BulletSlotView curBulletSlotView = CurBulletSlotControllers[i]._view as BulletSlotView;
            curBulletSlotView.State = GM.Root.PlayerMgr._PlayerData.
                CurBulletSlotLockedState[i]?UILockedState.isNormal:UILockedState.isLocked;
        }
    }
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
        tempGem.ForEach(gem=>  BagItemTools<Gem>.InitSaveFileObject(gem, gem.CurSlotController.SlotType));
        //初始化道具
        List<ItemData> tempItem = _InventoryData.BagItems
            .Concat(_InventoryData.EquipItems)
            .Select(curData => new ItemData(curData.ID, curData.CurSlotController as ItemSlotController))
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
            each.CurSlotController.AssignDirectly(each,BulletIns,false);
        }
    }
    
    //初始化子弹孵化器
    void InitSpawners()
    {
        //..............Clear Old Data..................
        SlotManager.ClearSlot(SlotType.SpawnnerSlot);
        SlotManager.ClearSlot(SlotType.SpawnnerSlotInner);
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

    #region 不关心的方法
    //抽取奇迹物件的时候，不能抽取重复的，这一步查重
    public bool MiracleOdditiesDuplicateCheck(int id)
    {
        foreach (MiracleOddityData each in _InventoryData.EquipMiracleOddities)
        {
            if (each.ID == id)
                return true;
        }
        return false;
    }

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
        MiracleOddityMrg = new MiracleOddityManager();
    }

    //初始化一些依赖UI的容器
    public void InitStep2(BulletSlotView[] bulletViews,
        BulletInnerSlotView[] bulletInnerViews)
    {
        CurBulletSlotControllers = new List<BulletSlotController>();
        CurBulletInnerSlotControllers = new List<BulletInnerSlotController>();
        for (int i = 0; i < bulletViews.Length; i++)
            CurBulletSlotControllers.Add(bulletViews[i].Controller as BulletSlotController);
        for (int i = 0; i < bulletInnerViews.Length; i++)
            CurBulletInnerSlotControllers.Add(bulletInnerViews[i].Controller as BulletInnerSlotController);

        _BulletInvData.CurBulletSlotControllers = CurBulletSlotControllers;
        _BulletInvData.OnModifiersChanged += MiracleOddityMrg.ApplyAlltimesEffectsToBullets;
    }
    //初始化一些需要读档之后的信息
    public void InitStep3()
    {
        MiracleOddityMrg.InitData();
    }
    
    void OnDestroy() =>  _BulletInvData.OnModifiersChanged 
        -= MiracleOddityMrg.ApplyAlltimesEffectsToBullets;
    #endregion 
}