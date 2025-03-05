using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainRoleManager :ScriptableObject
{
    [Header("游戏进程相关")] 
    public GameObject MainRoleIns;
    public MapLogic CurMapLogic;
    public MapSate CurMapSate;
    public EnemyMiddleData CurEnemyMidData;
    public void InitFightData(EnemyMiddleData _enemyMidData,int _levelID)
    {
        CurEnemyMidData = _enemyMidData;
        CurMapSate.CurLevelID = _levelID;
        CurEnemyMidData.CurAward = _enemyMidData.CurAward;
    }

    [Header("重要数据")]
    //...............重要数据................
    public int MaxHP = 3;
    public int _hp;
    public int HP
    {
        get => _hp;
        set { if (_hp != value) { _hp = value; HPChanged?.Invoke(); } } 
    }
    public event Action HPChanged;
    
    public int Score;
    public int Coins;
    public int RoomKeys;
    public int ShopCost = 5;
    public List<StandbyData> CurStandbyBulletMats = new List<StandbyData>();
    
    //...............Item................
    //子弹
    public List<BulletData> CurBullets = new List<BulletData>(); //当前上膛的子弹
    Dictionary<BulletJson, Bullet> CurBulletsPair;
    #region 子弹槽的状态
    public event Action BulletSlotStateChanged;
    public BulletSlotRole[] CurBulletSlotRoleSCs;
    private Dictionary<int, bool> _curBulletSlotLockedState = new Dictionary<int, bool>();
    public Dictionary<int, bool> CurBulletSlotLockedState
    {
        get => _curBulletSlotLockedState;
        set
        {
            if (_curBulletSlotLockedState != value)
            {
                _curBulletSlotLockedState = value;
                BulletSlotStateChanged?.Invoke(); // 通知变化
            }
        }
    }
    #endregion
    
    public List<BulletData> CurBulletSpawners = new List<BulletData>();                   //全部的子弹
    //道具
    public List<ItemJson> BagItems = new List<ItemJson>();
    public List<ItemJson> EquipItems = new List<ItemJson>();
    //宝石
    public List<GemJson> BagGems = new List<GemJson>();//在背包中的宝石
    public List<GemJson> InLayGems = new List<GemJson>();//在镶嵌槽中的宝石
    public List<GemSlot> InlayGemSlots = new List<GemSlot>();//宝石槽
    public GemSlot GetEmptyGemSlot() => InlayGemSlots.FirstOrDefault(gemSlot => 
        gemSlot.State == UILockedState.isNormal && gemSlot.MainID==-1);//找到一个空的宝石槽
    public List<GemSlot> BagGemSlots = new List<GemSlot>();//背包中的宝石槽
    public GemSlot GetEmptyBagGemSlot() => BagGemSlots.FirstOrDefault(gemSlot => 
        gemSlot.State == UILockedState.isNormal && gemSlot.MainID==-1);//找到一个空的背包位
    
    [Header("战报")]
    public WarReport CurWarReport;

    #region 人物属性
    [Header("人物属性")] 
    public int WaterElement;
    public int FireElement;
    public int ThunderElement;
    public int LightElement;
    public int DarkElement;

    public int DebuffMaxDamage;
    [Header("伤害倍率")]
    public int WaterDamage;
    public int FireDamage;
    public int ThunderDamage;
    public int LightDamage;
    public int DarkDamage;
    public int MaxDamage;
    ItemAttribute _attrInfo;
    #endregion
    
    #region 在数据层挪移道具
    public void MoveItem(Item curItem)
    {
        MoveItemBetweenLists(BagItems,curItem);
        MoveItemBetweenLists(EquipItems,curItem);
    }
    
    void MoveItemBetweenLists(List<ItemJson> itemList, Item curItem)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].InstanceID == curItem.InstanceID)
            {
                itemList[i].SlotID = curItem.SlotID;
                itemList[i].SlotType = (int)curItem.SlotType;
                return;
            }
        }
    }
    #endregion
    
    #region 决定商店抽卡概率的部分
    public List<RollPR> CurRollPR;
    public List<int> CurRollPREveIDs = new List<int>();
    
    List<RollPR> _orginalRollPR;
    public List<RollPR> OrginalRollPR
    {
        get
        {
            _orginalRollPR = new List<RollPR>();
            RollPR ScorePro = new RollPR();
            ScorePro.ID = 0;
            ScorePro.Probability = 0.99f;
            RollPR Bullet01Pro = new RollPR();
            Bullet01Pro.ID = 1;
            Bullet01Pro.Probability = 0.01f;
            _orginalRollPR.Add(ScorePro);
            _orginalRollPR.Add(Bullet01Pro);
            return _orginalRollPR;
        }
    }
    #endregion

    public WinOrFail WinOrFailState;

    #region 游戏进程相关
    public void WinThisLevel()
    {
        CurMapSate.IsFinishedRooms.Add(CurMapSate.CurRoomID);
        CurMapSate.CurRoomID = CurMapSate.TargetRoomID; //切换当前房间
        CurMapLogic.SetRolePos();
    }
    
    public void FailThisLevel()
    {
        CurMapLogic.SetRolePos();
    }
    #endregion

    //子弹关系
    public void ProcessBulletRelations()
    {
        if (CurBullets.Count < 2) return;

        ResonanceSlotCol[] ResonanceSlotCols = UIManager.Instance.BagReadySlotRootGO.
            GetComponentsInChildren<ResonanceSlotCol>();
        //处理共振
        Dictionary<int,List<int>> ResonanceClusterDict = new Dictionary<int, List<int>>();
        int clusterCount = 1;
        int resonanceCount = 0;
        for (int i = 1; i < CurBullets.Count; i++)
        {
            BulletData preBullet = CurBullets[i - 1];
            BulletData nextBullet = CurBullets[i];
            if (preBullet.FinalResonance == 0 || nextBullet.FinalResonance == 0)//不符合共振条件
            {
                resonanceCount = 0;
                continue;//不符合共振条件
            }

            bool isResonance = false;
            int preRemainder = preBullet.ID % 100;
            int nextRemainder = nextBullet.ID % 100;
            if (nextRemainder == preRemainder)//符合共振条件
            {
                resonanceCount++;
                //preBullet.IsResonance = true;
                //nextBullet.IsResonance = true;
                //CurBulletsPair[preBullet].IsResonance = true;
                //开始添加共振伤害
                /*Bullet nextBulletSC = CurBulletsPair[nextBullet];
                nextBulletSC.IsResonance = true;
                nextBulletSC.FinalDamage += nextBulletSC.FinalResonance * resonanceCount;
                nextBullet.FinalDamage += nextBullet.FinalResonance * resonanceCount;*/
                //构建共振簇
                if (ResonanceClusterDict.ContainsKey(clusterCount))
                    ResonanceClusterDict[clusterCount].Add(nextBullet.SlotID);
                else
                    ResonanceClusterDict[clusterCount] = new List<int>{preBullet.SlotID,nextBullet.SlotID};
            }
            else
            {
                resonanceCount = 0;
            }

            if (resonanceCount == 0)//说明共振被中断了，要重新开始
            {
                clusterCount++;
            }
        }
        
        //处理共振簇
        foreach (var slotCol in ResonanceSlotCols)
        {
            slotCol.CloseEffect();
        }

        foreach (var each in ResonanceClusterDict)
        {
            foreach (var slotCol in ResonanceSlotCols)
            {
                if (each.Value.Count != slotCol.ResonanceSlots.Count) continue;

                if (each.Value.OrderBy(x => x).SequenceEqual(slotCol.ResonanceSlots.OrderBy(x => x)))
                {
                    slotCol.OpenEffect();
                }
            }
        }
    }
    
    public void InitContainer()
    {
        if(CurWarReport==null)
            CurWarReport = new WarReport();
        if (CurRollPREveIDs == null)
            CurRollPREveIDs = new List<int>();
        if (CurRollPR == null)
            CurRollPR = new List<RollPR>(OrginalRollPR);
        if (CurBulletsPair == null)
            CurBulletsPair = new Dictionary<BulletJson, Bullet>();
        //"游戏进程相关"
        if (CurMapSate == null)
            CurMapSate = new MapSate();
        //初始化宝石槽的数据
        InlayGemSlots = new List<GemSlot>();
        InlayGemSlots.AddRange(UIManager.Instance.BagReadySlotRootGO
            .GetComponentsInChildren<GemSlot>());
        BagGemSlots = new List<GemSlot>();
        BagGemSlots.AddRange(UIManager.Instance.BagGemRootGO
            .GetComponentsInChildren<GemSlot>());
    }

    public void InitData()
    {
        InitContainer();
        InstanceSpawners();
        InstanceCurBullets();
        InitStandbyBulletMats();
        RefreshAllItems();
        CurBulletSlotRoleSCs = UIManager.Instance.BagReadySlotRootGO.GetComponentsInChildren<BulletSlotRole>();
        WinOrFailState = WinOrFail.InLevel;
    }

    #region 纯数据层操作
    //更新当前子弹生成器数据
    public void RefreshSpawner(MutMode mode,int BulletID)
    {
        switch (mode)
        {
            case MutMode.Sub:
                CurBulletSpawners.FirstOrDefault(b => b.ID == BulletID).SpawnerCount -= 1;
                break;
            case MutMode.Add:
                CurBulletSpawners.FirstOrDefault(b => b.ID == BulletID).SpawnerCount += 1;
                break;
        }
    }

    //更新当前子弹数据
    public void RefreshCurBullets(MutMode mode, int BulletID,int InstanceID = -1,int TargetSlotID = -1)
    {
        switch (mode)
        {
            case MutMode.Sub:
                BulletData bulletToRemove = CurBullets
                    .FirstOrDefault(b => b.ID == BulletID && b.InstanceID == InstanceID);
                if (bulletToRemove != null)
                    CurBullets.Remove(bulletToRemove);
                break;
            case MutMode.Add:
                if (CurBullets.Count >= 5) return;

                if (TargetSlotID == -1)
                {
                    TargetSlotID = Enumerable.Range(1, 5)
                        .FirstOrDefault(i => !CurBullets.Any(b => b.SlotID == i));
                }
                
                BulletData curData = new BulletData(BulletID);
                curData.SlotID = TargetSlotID;
                CurBullets.Add(curData);
                break;
        }
    }
    
    //更新当前备用子弹数据
    public void RefreshStandbyBulletMats(MutMode mode, int BulletID,int InstanceID)
    {
        switch (mode)
        {
            case MutMode.Sub:
                foreach (var each in CurStandbyBulletMats)
                {
                    if (each.ID == BulletID)
                    {
                        each.ID = 0;
                        each.InstanceID = 0;
                    }
                }
                break;
            case MutMode.Add:
                foreach (var each in CurStandbyBulletMats)
                {
                    if (each.ID == 0)
                    {
                        each.ID = BulletID;
                        each.InstanceID = InstanceID;
                        break;
                    }
                }
                break;
        }
    }
    
    //更新当前背包内物品数据
    public void RefreshGemData(MutMode mode, GemJson gamJson)
    {
        switch (mode)
        {
            case MutMode.Sub:
                BagGems.RemoveAll(each => each.ID == gamJson.ID && each.InstanceID == gamJson.InstanceID);
                InLayGems.RemoveAll(each => each.ID == gamJson.ID && each.InstanceID == gamJson.InstanceID);
                break;
            case MutMode.Add:
                BagGems.Add(gamJson);
                break;
        }
    }
    public void RefreshBagData(MutMode mode,ItemJson itemJson)
    {
        switch (mode)
        {
            case MutMode.Sub:
                BagItems.RemoveAll(each => each.ID == itemJson.ID && each.InstanceID == itemJson.InstanceID);
                EquipItems.RemoveAll(each => each.ID == itemJson.ID && each.InstanceID == itemJson.InstanceID);
                break;
            case MutMode.Add:
                BagItems.Add(itemJson);
                break;
        }
    }
    
    //更新当前全部
    public void RefreshAllItems()
    {
        #region 更新数据
        EquipItems = new List<ItemJson>();
        EquipItems.AddRange(UIManager.Instance.EquipItemRootGO
            .GetComponentsInChildren<Item>(true)
            .Select(item => item.ToJosn()));

        BagItems = new List<ItemJson>();
        BagItems.AddRange(UIManager.Instance.BagItemRootGO
            .GetComponentsInChildren<Item>(true)
            .Select(item => item.ToJosn()));

        BagGems = new List<GemJson>();
        BagGems.AddRange(UIManager.Instance.BagGemRootGO.
            GetComponentsInChildren<Gem>(true).Select(gem => gem.ToJosn()));
        
        //宝石镶嵌属性一并同步
        CurBullets = new List<BulletData>();
        Bullet[] CurBulletSC = UIManager.Instance.BagReadySlotRootGO
            .GetComponentsInChildren<Bullet>(true);

        InLayGems = new List<GemJson>();
        Gem[] InLayGemSC = UIManager.Instance.BagReadySlotRootGO
            .GetComponentsInChildren<Gem>(true);
        InLayGems.AddRange(InLayGemSC.Select(gem => gem.ToJosn()));
        foreach (var each in CurBulletSC)
        {
            each.ClearGem();
            foreach (var eachGem in InLayGemSC)
            {
                if (each.SlotID == eachGem.BulletSlotIndex)
                    each.AddGem(eachGem);
            }
        }

        CurBulletsPair.Clear();
        /*foreach (var each in CurBulletSC)
        {
            BulletData curJson = each.ToJosn();
            CurBullets.Add(curJson);
            CurBulletsPair[curJson] = each;
        }*/
        #endregion

        #region 属性添加
        _attrInfo = new ItemAttribute();
        //把元素均衡界面的属性，同步到角色身上
        foreach (var each in EquipItems)
            _attrInfo.Aggregate(each);
        
        WaterElement = _attrInfo.waterElement;
        FireElement = _attrInfo.fireElement;
        ThunderElement = _attrInfo.thunderElement;
        LightElement = _attrInfo.lightElement;
        DarkElement = _attrInfo.darkElement;

        WaterDamage = _attrInfo.extraWaterDamage;
        FireDamage = _attrInfo.extraFireDamage;
        ThunderDamage = _attrInfo.extraThunderDamage;
        LightDamage = _attrInfo.extraLightDamage;
        DarkDamage = _attrInfo.extraDarkDamage;

        MaxDamage = _attrInfo.maxDamage;
        #endregion

        //刷新子弹关系
        ProcessBulletRelations();
        //刷新侧边栏显示
        SyncBulletIcon();
    }
    #endregion

    #region 场景内GO操作
    public void InstanceSpawners()
    {
        //..............Clear Old Data..................
        DraggableBulletSpawner[] oldSpawner = UIManager.Instance
            .G_BulletSpawnerSlot.GetComponentsInChildren<DraggableBulletSpawner>();
        for (int i = oldSpawner.Length - 1; i >= 0; i--)
            DestroyImmediate(oldSpawner[i].gameObject);
        //..............Instance New Data..................
        BulletSlot[] slots = UIManager.Instance.G_BulletSpawnerSlot.GetComponentsInChildren<BulletSlot>();
        BulletSlot[] slotMinis = UIManager.Instance.G_BulletSpawnerSlot_Mini.GetComponentsInChildren<BulletSlot>();
        InstanceSpawnersSingel(slots);
        InstanceSpawnersSingel(slotMinis);
    }
    void InstanceSpawnersSingel(BulletSlot[] slots)
    {
        foreach (BulletData each in CurBulletSpawners)
        {
            int curSpawnerFindID = each.ID % 10;
            var slot = slots.FirstOrDefault(s => s.SlotID == curSpawnerFindID);
            if (slot != null)
            {
                slot.MainID = each.ID;
                GameObject newSpawnerIns = BulletFactory.CreateBullet(each, BulletInsMode.Spawner).gameObject;
                newSpawnerIns.transform.SetParent(slot.gameObject.transform, false);
            }
        }
    }

    public void InstanceCurBullets()
    {
        GameObject roleSlotRoot = UIManager.Instance.BagReadySlotRootGO;
        //..............Clear Old Data..................
        Bullet[] allOldBullets = roleSlotRoot.GetComponentsInChildren<Bullet>(true);
        foreach (BulletSlotRole each in CurBulletSlotRoleSCs)
            each.MainID = -1;
        for (int i = allOldBullets.Length - 1; i >= 0; i--)
            DestroyImmediate(allOldBullets[i].gameObject);
        
        //..............Instance New Data..................
        foreach (BulletData each in CurBullets)
        {
            GameObject BulletIns = BulletManager.Instance.InstanceBullet(each.ID,BulletInsMode.EditB);
            each.InstanceID = BulletIns.GetComponentInChildren<Bullet>().InstanceID; //读取存档，要把InstanceID同步
            //...................SetSlot.......................
            SlotBase baseSC = SlotManager.GetEmptySlot(SlotType.CurBulletSlot);
            if (baseSC == null)return;
            baseSC.SOnDrop(BulletIns);
        }
        SyncBulletIcon();
    }

    public void SyncBulletIcon()
    {
        GameObject bulletIconRoot = UIManager.Instance.G_CurBulletIcon;
        for (int i = 0; i < 5; i++)
        {
            int curSlotID = i + 1;
            BulletData curBulletReady = null;
            foreach (var each in CurBullets)
            {
                if (each.SlotID == curSlotID)
                    curBulletReady = each;
            }
            GameObject curIconSlot = bulletIconRoot.transform.GetChild(curSlotID).gameObject;//找到对应的IconSlot
            Image curImg = curIconSlot.transform.GetChild(0).GetComponent<Image>();
            if (curBulletReady == null)
                curImg.color = Color.clear;
            else
            {
                curImg.color = Color.white;
                curImg.sprite = ResManager.instance.GetAssetCache<
                    Sprite>(PathConfig.GetBulletImageOrSpinePath(curBulletReady.ID, BulletInsMode.Icon));
            }
        }
    }
    
    public void RefreshCurBulletSlots()
    {
        //1)重置Slot
        foreach (BulletSlotRole each in CurBulletSlotRoleSCs)
            each.MainID = -1;

        DraggableBullet[] bulletSCs = UIManager.Instance.
            BagReadySlotRootGO.GetComponentsInChildren<DraggableBullet>();
        //2)设置Slot的BulletID
        foreach (var eachBullet in bulletSCs)
        {
            foreach (BulletSlotRole eachSlot in CurBulletSlotRoleSCs)
            {
                if (eachBullet.SlotID == eachSlot.SlotID)
                    eachSlot.MainID = eachBullet.ID;
            }
        }
        //3)刷新Icon
        SyncBulletIcon();
    }

    public void InitStandbyBulletMats()
    {
        GameObject SDBulletRoot = UIManager.Instance.G_StandbyMat;
        SlotStandbyMat[] SDSlots = UIManager.Instance.
            G_StandbyIcon.GetComponentsInChildren<SlotStandbyMat>();
        //..............Clear Old Data..................
        for (int i = SDBulletRoot.transform.childCount - 1; i >= 0; i--)
            DestroyImmediate(SDBulletRoot.transform.GetChild(i).gameObject);
        
        for (int i = 0; i < SDSlots.Length; i++)
            SDSlots[i].MainID = -1;
        //..............Instance New Data..................
        for (int i = 0; i < SDSlots.Length; i++)
        {
            if (CurStandbyBulletMats[i].ID != 0)
            {
                GameObject StandbyMatIns = BulletManager.Instance.InstanceStandbyMat(CurStandbyBulletMats[i].ID);
                SDSlots[i].AddIns(StandbyMatIns);
            }
        }
    }
    
    public bool AddStandbyBulletMat(int BulletID)
    {
        StandbyData curSD = null;
        foreach (StandbyData each in CurStandbyBulletMats)
        {
            if (each.ID == 0)
            {
                curSD = each;
                break;
            }
        }

        if (curSD == null)
            return false;
        
        SlotStandbyMat[] SDSlots = UIManager.Instance.
            G_StandbyIcon.GetComponentsInChildren<SlotStandbyMat>();
        SlotStandbyMat curSlot = SDSlots[curSD.SlotID];
        GameObject StandbyMatIns = BulletManager.Instance.InstanceStandbyMat(BulletID);
        curSlot.AddIns(StandbyMatIns);
        
        RefreshStandbyBulletMats(MutMode.Add, BulletID,StandbyMatIns.GetInstanceID());
        return true;
    }

    public void SubStandebyBullet(int BulletID,int InstanceID = -1)
    {
        RefreshStandbyBulletMats(MutMode.Sub, BulletID,InstanceID);
        GameObject curSD = UIManager.Instance.G_StandbyMat;

        for (int i = curSD.transform.childCount-1 ; i >= 0; i--)
        {
            GameObject curBullet = curSD.transform.GetChild(i).gameObject;
            StandbyBulletMat curSC = curBullet.GetComponentInChildren<StandbyBulletMat>();
            if (curSC.ID == BulletID)
            {
                if (InstanceID == -1)
                    RefreshStandbyBulletMats(MutMode.Sub, curSC.ID, curBullet.GetInstanceID());
                else
                    RefreshStandbyBulletMats(MutMode.Sub, curSC.ID, InstanceID);
            }
        }
        InitStandbyBulletMats();
    }
    #endregion

    #region 外部可以调用的操作组封装
    //添加道具
    public void AddItem(int ItemID)
    {
        Item itemSC = null;
        BagItemManager<Item>.AddObjectGO(ItemID,ref itemSC,
            UIManager.Instance.BagItemRootGO.transform,SlotType.BagSlot);
        ItemJson itemJson = itemSC.ToJosn();
        //数据层加进来了
        RefreshBagData(MutMode.Add,itemJson);
        //GO层
    }

    public void AddGem(int GemID)
    {
        //GO层
        Gem gemSC = null;
        BagItemManager<Gem>.AddObjectGO(GemID,ref gemSC,
            UIManager.Instance.BagGemRootGO.transform,SlotType.GemBagSlot);
        GemJson gemJson = gemSC.ToJosn();
        
        //数据层加进来了
        RefreshGemData(MutMode.Add,gemJson);
    }

    public void TmpHongSpawner(int bulletID)
    {
        RefreshSpawner(MutMode.Sub,bulletID);
    }
    
    public void AddSpawner(int bulletID)
    {
        RefreshSpawner(MutMode.Add,bulletID);
    }

    public void SubBullet(int bulletID,int instanceID)
    {
        RefreshCurBullets(MutMode.Sub,bulletID,InstanceID:instanceID);
        RefreshSpawner(MutMode.Add,bulletID);
        RefreshCurBulletSlots();
    }
    
    public void SubBullet(int SlotIndex)
    {
        GameObject curIns = GetReadyBulletBySlotID(SlotIndex);
        //DestroyImmediate(curIns);
        Bullet curSC = curIns.GetComponentInChildren<Bullet>();
        RefreshCurBullets(MutMode.Sub,curSC.ID,InstanceID:curSC.InstanceID);
        RefreshSpawner(MutMode.Add,curSC.ID);
        RefreshCurBulletSlots();
        DestroyImmediate(curIns);
        RefreshAllItems();
    }
    #endregion

    #region 一些私有方法的方便封装
    public GameObject GetReadyBulletBySlotID(int slotID)
    {
        GameObject curIns = null;
        DraggableBullet[] curBulletSCs = UIManager.Instance.
            BagReadySlotRootGO.GetComponentsInChildren<DraggableBullet>();
        for (int i = curBulletSCs.Length - 1; i >= 0; i--)
        {
            DraggableBullet curSC = curBulletSCs[i];
            if (curSC.SlotID == slotID)
                curIns = curSC.gameObject;
        }
        return curIns;
    }
    #endregion
    
    #region 单例
    static MainRoleManager s_instance;
    public static MainRoleManager Instance
    {
        get
        {
            s_instance??= ResManager.instance.GetAssetCache<MainRoleManager>(PathConfig.MainRoleManagerOBJ);
            if (s_instance != null) { DontDestroyOnLoad(s_instance); }
            return s_instance;
        }
    }
    void OnEnable()
    {
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(this);
        }
    }
    #endregion
}