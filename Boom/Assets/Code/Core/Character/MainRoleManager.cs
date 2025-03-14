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
    public MapManager CurMapManager;
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
    public List<ItemData> BagItems = new List<ItemData>();
    public List<ItemData> EquipItems = new List<ItemData>();
    //宝石
    public List<GemData> BagGems = new List<GemData>();//在背包中的宝石
    public List<GemData> InLayGems = new List<GemData>();//在镶嵌槽中的宝石
    
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
        CurMapManager.SetRolePos();
    }
    
    public void FailThisLevel()
    {
        CurMapManager.SetRolePos();
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
                    ResonanceClusterDict[clusterCount].Add(nextBullet.CurSlot.SlotID);
                else
                    ResonanceClusterDict[clusterCount] = new List<int>{preBullet.CurSlot.SlotID,nextBullet.CurSlot.SlotID};
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
        //"游戏进程相关"
        if (CurMapSate == null)
            CurMapSate = new MapSate();
    }

    public void InitData()
    {
        InitContainer();
        InitSpawners();
        InitCurBullets();
        InitStandbyBulletMats();
        CurBulletSlotRoleSCs = UIManager.Instance.BagReadySlotRootGO.GetComponentsInChildren<BulletSlotRole>();
        WinOrFailState = WinOrFail.InLevel;
    }

    #region 纯数据层操作
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
    #endregion

    #region 场景内GO操作
    public void InitSpawners()
    {
        //..............Clear Old Data..................
        DraggableBulletSpawner[] oldSpawner = UIManager.Instance
            .SpawnerSlotRoot.GetComponentsInChildren<DraggableBulletSpawner>();
        for (int i = oldSpawner.Length - 1; i >= 0; i--)
            DestroyImmediate(oldSpawner[i].gameObject);
        //..............Instance New Data..................
        BulletSlot[] slots = UIManager.Instance.SpawnerSlotRoot.GetComponentsInChildren<BulletSlot>();
        BulletSlot[] slotMinis = UIManager.Instance.SpawnerSlotRootMini.GetComponentsInChildren<BulletSlot>();
        InitSpawnersSingel(slots);
        InitSpawnersSingel(slotMinis,true);
    }
    void InitSpawnersSingel(BulletSlot[] slots, bool isMini = false)
    {
        foreach (BulletData each in CurBulletSpawners)
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
    
    //初始化当前子弹的GO
    public void InitCurBullets()
    {
        //...............Clear Old Data....................
        Bullet[] oldBullets = UIManager.Instance.BagReadySlotRootGO.GetComponentsInChildren<Bullet>();
        for (int i = oldBullets.Length - 1; i >= 0; i--)
            DestroyImmediate(oldBullets[i].gameObject);
        SlotManager.GetEmptySlot(SlotType.CurBulletSlot);
        //..............Instance New Data..................
        foreach (BulletData each in CurBullets)
        {
            GameObject BulletIns = BulletFactory.CreateBullet(each, BulletInsMode.EditB).gameObject;
            each.CurSlot.SOnDrop(BulletIns);
        }
        SyncBulletIcon();
    }

    public void InitStandbyBulletMats()
    {
        GameObject SDBulletRoot = UIManager.Instance.StandbyRoot;
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
        GameObject curSD = UIManager.Instance.StandbyRoot;

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
        ItemData newItemData = new ItemData(ItemID, SlotManager.GetEmptySlot(SlotType.BagSlot));
        BagItems.Add(newItemData);
        BagItemManager<Item>.AddObjectGO(newItemData);
    }
    
    public void SubItem(ItemData itemData) =>BagItems.Remove(itemData);


    public void AddGem(int GemID)
    {
        GemData newGemData = new GemData(GemID, SlotManager.GetEmptySlot(SlotType.GemBagSlot));
        BagGems.Add(newGemData);
        BagItemManager<Gem>.AddObjectGO(newGemData);
    }

    public void SubGem(GemData gemData) => BagGems.Remove(gemData);
    
    public void AddSpawner(int bulletID)
    {
        var spawner = CurBulletSpawners.FirstOrDefault(each => each.ID == bulletID);
        if (spawner != null)
            spawner.SpawnerCount++;
    }
    
    public void AddCurBullet(BulletData bulletData)
    {
        if (CurBullets.Count >= 5) return;
        CurBullets.Add(bulletData);
        SortCurBullet();
        SyncBulletIcon();
    }

    public void SortCurBullet()
    {
        CurBullets.Sort((bullet1, bullet2) => bullet1.CurSlot.SlotID.CompareTo(bullet2.CurSlot.SlotID));
        SyncBulletIcon();
    }

    public void SubCurBullet(BulletData bulletData)
    {
        CurBullets.Remove(bulletData);
        SyncBulletIcon();
    }
    
    //根据槽位清除当前子弹信息
    public void ReturnCurBulletBySlotID(BulletData _data)
    {
        if (_data == null) return;
        SubCurBullet(_data);
        AddSpawner(_data.ID);
    }
    #endregion

    #region 不关心的私有方法
    void SyncBulletIcon()
    {
        GameObject bulletIconRoot = UIManager.Instance.G_CurBulletIcon;
        for (int i = 0; i < 5; i++)
        {
            int curSlotID = i + 1;
            BulletData curBulletReady = null;
            foreach (var each in CurBullets)
            {
                if (each.CurSlot.SlotID == curSlotID)
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