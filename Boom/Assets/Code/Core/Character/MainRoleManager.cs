﻿using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
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
    }

    [Header("重要数据")]
    //...............重要数据................
    public int Score;
    public int Coins;
    public int RoomKeys;
    public int ShopCost = 5;
    public List<StandbyData> CurStandbyBulletMats = new List<StandbyData>();
    
    //...............Item................
    //子弹
    public List<BulletJson> CurBullets = new List<BulletJson>(); //当前上膛的子弹
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
    
    public List<BulletJson> CurBulletSpawners;                   //全部的子弹
    //道具
    public List<ItemJson> BagItems = new List<ItemJson>();
    public List<ItemJson> EquipItems = new List<ItemJson>();
    //宝石
    public List<GemJson> BagGems = new List<GemJson>();
    public List<GemJson> InLayGems = new List<GemJson>();
    public List<SupremeCharm> SupremeCharms = new List<SupremeCharm>();

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

    public void InitCurRollPR()
    {
        CurRollPR = new List<RollPR>(OrginalRollPR);
        RollPR rubbish = CurRollPR[0];
        List<RollPREvent> PRDesignJsons = TrunkManager.Instance.PRDesignJsons; //策划数据
        //遍历所有当前事件，把概率都加入到当前概率中。
        /*for (int i = 0; i < CurRollPREveIDs.Count; i++)
        {
            int curEID = CurRollPREveIDs[i];
            RollPREvent curPRE = ComFunc.GetSingle(PRDesignJsons, curEID);
            
            foreach (var each in curPRE.AddPRDict)
            {
                bool IsFind = false;
                foreach (var eachPR in CurRollPR)
                {
                    if (eachPR.ID == each.Key)
                    {
                        eachPR.Probability += each.Value;
                        IsFind = true;
                        break;
                    }
                }
                if (!IsFind)
                {
                    int tmp = int.Parse(each.Key.ToString());
                    CurRollPR.Add(new RollPR{ID = tmp,Probability = each.Value});
                }
                rubbish.Probability -= each.Value;
            }
            
            foreach (var each in curPRE.SubPRDict)
            {
                bool IsFind = false;
                foreach (var eachPR in CurRollPR)
                {
                    if (eachPR.ID == each.Key)
                    {
                        eachPR.Probability -= each.Value;
                        rubbish.Probability += each.Value;
                        IsFind = true;
                        break;
                    }
                }
            }
        }*/
    }

    public void AddPREve(int EveID)
    {
        CurRollPREveIDs.Add(EveID);
        InitCurRollPR();
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
            BulletJson preBullet = CurBullets[i - 1];
            BulletJson nextBullet = CurBullets[i];
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
                preBullet.IsResonance = true;
                nextBullet.IsResonance = true;
                CurBulletsPair[preBullet].IsResonance = true;
                //开始添加共振伤害
                Bullet nextBulletSC = CurBulletsPair[nextBullet];
                nextBulletSC.IsResonance = true;
                nextBulletSC.FinalDamage += nextBulletSC.FinalResonance * resonanceCount;
                nextBullet.FinalDamage += nextBullet.FinalResonance * resonanceCount;
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
        if (CurRollPREveIDs == null)
            CurRollPREveIDs = new List<int>();
        if (CurRollPR == null)
            CurRollPR = new List<RollPR>(OrginalRollPR);
        if (CurBulletsPair == null)
            CurBulletsPair = new Dictionary<BulletJson, Bullet>();
        //"游戏进程相关"
        if (CurMapSate == null)
            CurMapSate = new MapSate();
    }

    public void InitData()
    {
        InitContainer();
        InstanceSpawners();
        InstanceCurBullets();
        InitStandbyBulletMats();
        InitCurRollPR();
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
    public void RefreshCurBullets(MutMode mode, int BulletID,int InstanceID = -1)
    {
        switch (mode)
        {
            case MutMode.Sub:
                BulletJson bulletToRemove = CurBullets
                    .FirstOrDefault(b => b.ID == BulletID && b.InstanceID == InstanceID);
                if (bulletToRemove != null)
                    CurBullets.Remove(bulletToRemove);
                break;
            case MutMode.Add:
                if (CurBullets.Count >= 5) return;

                int SlotID = Enumerable.Range(1, 5)
                    .FirstOrDefault(i => !CurBullets.Any(b => b.SlotID == i));
                
                BulletJson curData = new BulletJson(BulletID,_slotID: SlotID);
                curData.SyncData();
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
                foreach (var each in BagGems)
                {
                    if (each.ID == gamJson.ID && each.InstanceID == gamJson.InstanceID)
                        BagGems.Remove(each);
                }
                foreach (var each in InLayGems)
                {
                    if (each.ID == gamJson.ID && each.InstanceID == gamJson.InstanceID)
                        InLayGems.Remove(each);
                }
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
                foreach (var each in BagItems)
                {
                    if (each.ID == itemJson.ID && each.InstanceID == itemJson.InstanceID)
                        BagItems.Remove(each);
                }
                foreach (var each in EquipItems)
                {
                    if (each.ID == itemJson.ID && each.InstanceID == itemJson.InstanceID)
                        BagItems.Remove(each);
                }

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
            .GetComponentsInChildren<Item>()
            .Select(item => item.ToJosn()));

        BagItems = new List<ItemJson>();
        BagItems.AddRange(UIManager.Instance.BagItemRootGO
            .GetComponentsInChildren<Item>()
            .Select(item => item.ToJosn()));

        BagGems = new List<GemJson>();
        BagGems.AddRange(UIManager.Instance.BagGemRootGO.
            GetComponentsInChildren<Gem>().Select(gem => gem.ToJosn()));
        
        //宝石镶嵌属性一并同步
        CurBullets = new List<BulletJson>();
        Bullet[] CurBulletSC = UIManager.Instance.BagReadySlotRootGO
            .GetComponentsInChildren<Bullet>();

        InLayGems = new List<GemJson>();
        Gem[] InLayGemSC = UIManager.Instance.BagReadySlotRootGO
            .GetComponentsInChildren<Gem>();
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
        foreach (var each in CurBulletSC)
        {
            BulletJson curJson = each.ToJosn();
            CurBullets.Add(curJson);
            CurBulletsPair[curJson] = each;
        }
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
        foreach (BulletJson each in CurBulletSpawners)
        {
            int curSpawnerFindID = each.ID % 10;
            for (int i = 0; i < slots.Length; i++)
            {
                if (curSpawnerFindID == slots[i].SlotID)
                {
                    slots[i].MainID = each.ID;
                    GameObject newSpawnerIns = BulletManager.Instance.
                        InstanceBullet(each.ID,BulletInsMode.Spawner);
                    var curSC = newSpawnerIns.GetComponentInChildren<DraggableBulletSpawner>();
                    curSC.Count = each.SpawnerCount;
                    newSpawnerIns.transform.SetParent(slots[i].gameObject.transform);
                    newSpawnerIns.transform.localScale = Vector3.one;
                    newSpawnerIns.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
                }
            }
        }
    }
    
    public void RefreshSpawnerIns()
    {
        DraggableBulletSpawner[] oldSpawner = UIManager.Instance
            .G_BulletSpawnerSlot.GetComponentsInChildren<DraggableBulletSpawner>(true);
        foreach (BulletJson each in CurBulletSpawners)
        {
            foreach (var perSpawner in oldSpawner)
            {
                if (each.ID == perSpawner.ID)
                    perSpawner.Count = each.SpawnerCount;
            }
        }
    }

    public void InstanceCurBullets()
    {
        GameObject roleSlotRoot = UIManager.Instance.BagReadySlotRootGO;
        GameObject bulletRoot = UIManager.Instance.DragObjRoot;
        //..............Clear Old Data..................
        for (int i = bulletRoot.transform.childCount - 1; i >= 0; i--)
            DestroyImmediate(bulletRoot.transform.GetChild(i).gameObject);
        //..............Instance New Data..................
        foreach (BulletJson each in CurBullets)
        {
            GameObject BulletIns = BulletManager.Instance.InstanceBullet(each.ID,BulletInsMode.EditB);
            each.InstanceID = BulletIns.GetComponentInChildren<Bullet>().InstanceID; //读取存档，要把InstanceID同步
            //...................SetSlot.......................
            GameObject curSlot = roleSlotRoot.transform.GetChild(each.SlotID - 1).gameObject;//找到对应的Slot
            BulletSlot curSlotSC = curSlot.GetComponentInChildren<BulletSlot>();
            curSlotSC.SOnDrop(BulletIns,SlotType.CurBulletSlot);
        }

        SyncBulletIcon();
    }

    public void SyncBulletIcon()
    {
        GameObject bulletIconRoot = UIManager.Instance.G_CurBulletIcon;
        for (int i = 0; i < 5; i++)
        {
            int curSlotID = i + 1;
            BulletJson curBulletReady = null;
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
        GameObject bulletDragRoot = UIManager.Instance.DragObjRoot;
        //1)重置Slot
       
        foreach (BulletSlotRole each in CurBulletSlotRoleSCs)
            each.MainID = -1;

        //2)设置Slot的BulletID
        foreach (BulletSlotRole eachSlot  in CurBulletSlotRoleSCs)
        {
            foreach (var eachBullet in CurBullets)
            {
                if (eachBullet.SlotID == eachSlot.SlotID)
                    eachSlot.MainID = eachBullet.ID;
            }
        }
        //3)设置Bullet的SlotID
        for (int i = bulletDragRoot.transform.childCount - 1; i >= 0; i--)
        {
            GameObject curBullet = bulletDragRoot.transform.GetChild(i).gameObject;
            DraggableBullet curSC = curBullet.GetComponentInChildren<DraggableBullet>();
            foreach (var each in CurBullets)
            {
                if (each.ID == curSC.ID
                    && each.InstanceID == curSC.InstanceID)
                    curSC.SlotID = each.SlotID;
            }
        }
        //4)刷新Icon
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
        RefreshSpawnerIns();
    }
    
    public void AddSpawner(int bulletID)
    {
        RefreshSpawner(MutMode.Add,bulletID);
        RefreshSpawnerIns();
    }

    public void SubBullet(int bulletID,int instanceID)
    {
        RefreshCurBullets(MutMode.Sub,bulletID,InstanceID:instanceID);
        RefreshSpawner(MutMode.Add,bulletID);
        RefreshSpawnerIns();
        RefreshCurBulletSlots();
    }
    #endregion

    #region 一些私有方法的方便封装
    enum SlotKind
    {
        SlotRole = 1,
        SlotSpawn = 2,
    }
    GameObject GetSlot(int slotID,SlotKind slotKind)
    {
        GameObject root = null;
        switch (slotKind)
        {
            case SlotKind.SlotRole:
                root = UIManager.Instance.BagReadySlotRootGO;
                break;
            case SlotKind.SlotSpawn:
                break;
        }
        if (root == null) return null;

        GameObject targetSlot = null;
        for (int i = root.transform.childCount - 1; i >= 0; i--)
        {
            GameObject curSlot = root.transform.GetChild(i).gameObject;
            BulletSlot curSC = curSlot.GetComponentInChildren<BulletSlot>();
            if (curSC.SlotID == slotID)
                targetSlot = curSlot;
        }

        return targetSlot;
    }

    GameObject GetReadyBulletBySlotID(int slotID)
    {
        GameObject curIns = null;
        GameObject bulletRoot = UIManager.Instance.DragObjRoot;
        for (int i = bulletRoot.transform.childCount - 1; i >= 0; i--)
        {
            GameObject curbullet = bulletRoot.transform.GetChild(i).gameObject;
            DraggableBullet curSC = curbullet.GetComponentInChildren<DraggableBullet>();
            if (curSC.SlotID == slotID)
                curIns = curbullet;
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