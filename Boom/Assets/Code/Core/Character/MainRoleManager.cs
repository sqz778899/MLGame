using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class MainRoleManager :ScriptableObject
{
    #region 单例
    static MainRoleManager s_instance;
    
    public static MainRoleManager Instance
    {
        get
        {
            if (s_instance == null)
                s_instance = ResManager.instance.GetAssetCache<MainRoleManager>(PathConfig.MainRoleManagerOBJ);
            return s_instance;
        }
    }
    #endregion
    
    //...............子弹上膛................
    public List<BulletReady> CurBullets;
    public List<BulletBuff> CurBulletBuffs;
    public List<BulletEntry> CurBulletEntries;
    
    //...............重要数据................
    public int Score;
    public int Gold;
    public int Cost = 5;
    public List<BulletSpawner> CurBulletSpawners;
    public List<StandbyData> CurStandbyBullets = new List<StandbyData>();
    public List<SupremeCharm> SupremeCharms = new List<SupremeCharm>();

    #region 词条相关
    public void AddEntry(int EntryID)
    {
        List<BulletEntry> DesignEntries = TrunkManager.Instance.BulletEntryDesignJsons;
        foreach (var each in DesignEntries)
        {
            if (each.ID == EntryID && !CurBulletEntries.Contains(each))
                CurBulletEntries.Add(each);
        }

        InitBulletEntries();
    }
    public void RemoveBulletBuff(int ID)
    {
        List<int> NeedDelete = new List<int>();
        //注意从大到小正确排序
        for (int i = CurBulletBuffs.Count - 1; i >= 0; i--)
        {
            if (CurBulletBuffs[i].ID == ID)
                NeedDelete.Add(i);
        }

        foreach (int each in NeedDelete)
            CurBulletBuffs.RemoveAt(each);
    }
    void InitBulletEntries()
    {
        foreach (var each in CurBulletEntries)
            EntryFunc.InvokeEntry(each.ID);
        UIManager.Instance.G_Help.GetComponent<HelpMono>().InitBulletEntryDes();
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
        for (int i = 0; i < CurRollPREveIDs.Count; i++)
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
        }
    }

    public void AddPREve(int EveID)
    {
        CurRollPREveIDs.Add(EveID);
        InitCurRollPR();
    }
    #endregion

    public WinOrFail WinOrFailState;

    public void InitContainer()
    {
        if (CurRollPREveIDs == null)
            CurRollPREveIDs = new List<int>();
        if (CurBulletEntries == null)
            CurBulletEntries = new List<BulletEntry>();
        if (CurRollPR == null)
            CurRollPR = new List<RollPR>(OrginalRollPR);
        if (CurBulletBuffs == null)
            CurBulletBuffs = new List<BulletBuff>();
    }

    public void InitData()
    {
        InitContainer();
        InstanceSpawners();
        InstanceCurBullets();
        InstanceStandbyBullets();
        InitCurRollPR();
        InitBulletEntries();
        
        WinOrFailState = WinOrFail.InLevel;
    }

    #region 纯数据层操作
    public void RefreshSpawner(BulletMutMode mode,int BulletID)
    {
        switch (mode)
        {
            case BulletMutMode.Sub:
                foreach (var each in CurBulletSpawners)
                {
                    if (each.bulletID == BulletID)
                        each.bulletCount -= 1;
                }
                break;
            case BulletMutMode.Add:
                foreach (var each in CurBulletSpawners)
                {
                    if (each.bulletID == BulletID)
                        each.bulletCount += 1;
                }
                break;
        }
    }

    public void RefreshCurBullets(BulletMutMode mode, int BulletID,int SlotID = -1,int InstanceID = -1,
        BulletInsMode bulletInsMode = BulletInsMode.EditA)
    {
        switch (mode)
        {
            case BulletMutMode.Sub:
                for (int i = CurBullets.Count - 1; i >= 0; i--)
                {
                    if (CurBullets[i].bulletID == BulletID &&
                        CurBullets[i].instanceID == InstanceID)
                    {
                        CurBullets.RemoveAt(i);
                        break;
                    }
                }
                break;
            case BulletMutMode.Add:
                if (SlotID == -1)
                {
                    Debug.LogError("未设置SlotID");
                    return;
                }
                BulletReady curData = new BulletReady(BulletID,SlotID,InstanceID);
                CurBullets.Add(curData);
                break;
        }
    }
    
    public void RefreshStandbyBullets(BulletMutMode mode, int BulletID,int InstanceID)
    {
        switch (mode)
        {
            case BulletMutMode.Sub:
                foreach (var each in CurStandbyBullets)
                {
                    if (each.BulletID == BulletID)
                    {
                        each.BulletID = 0;
                        each.InstanceID = 0;
                    }
                }
                break;
            case BulletMutMode.Add:
                foreach (var each in CurStandbyBullets)
                {
                    if (each.BulletID == 0)
                    {
                        each.BulletID = BulletID;
                        each.InstanceID = InstanceID;
                        break;
                    }
                }
                break;
        }
    }

    public void RefreshSupremeCharms(BulletMutMode mode, int CharmID)
    {
        switch (mode)
        {
            case BulletMutMode.Sub:
                foreach (var each in SupremeCharms)
                {
                    if (each.ID == CharmID)
                        SupremeCharms.Remove(each);
                }
                break;
            case BulletMutMode.Add:
                SupremeCharm newCharm = new SupremeCharm(CharmID);
                newCharm.GetSupremeCharmByID();
                SupremeCharms.Add(newCharm);
                break;
        }
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
        foreach (BulletSpawner each in CurBulletSpawners)
        {
            int curSpawnerFindID = each.bulletID % 10;
            for (int i = 0; i < slots.Length; i++)
            {
                if (curSpawnerFindID == slots[i].SlotID)
                {
                    slots[i].BulletID = each.bulletID;
                    GameObject newSpawnerIns = BulletManager.Instance.
                        InstanceBullet(each.bulletID,BulletInsMode.Spawner);
                    var curSC = newSpawnerIns.GetComponentInChildren<DraggableBulletSpawner>();
                    curSC.Count = each.bulletCount;
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
            .G_BulletSpawnerSlot.GetComponentsInChildren<DraggableBulletSpawner>();
        foreach (BulletSpawner each in CurBulletSpawners)
        {
            foreach (var perSpawner in oldSpawner)
            {
                if (each.bulletID == perSpawner._bulletData.ID)
                    perSpawner.Count = each.bulletCount;
            }
        }
    }

    public void InstanceCurBullets()
    {
        GameObject roleSlotRoot = UIManager.Instance.G_BulletRoleSlot;
        GameObject bulletRoot = UIManager.Instance.G_Bullet;
        //..............Clear Old Data..................
        for (int i = bulletRoot.transform.childCount - 1; i >= 0; i--)
            DestroyImmediate(bulletRoot.transform.GetChild(i).gameObject);
        //..............Instance New Data..................
        foreach (BulletReady each in CurBullets)
        {
            GameObject BulletIns = BulletManager.Instance.InstanceBullet(each.bulletID,BulletInsMode.EditB);
            each.instanceID = BulletIns.GetComponentInChildren<BulletBase>().InstanceID; //读取存档，要把InstanceID同步
            
            DraggableBullet curSC = BulletIns.GetComponentInChildren<DraggableBullet>();
            //...................SetSlot.......................
            GameObject curSlot = roleSlotRoot.transform.GetChild(each.curSlotID - 1).gameObject;//找到对应的Slot
            BulletSlot curSlotSC = curSlot.GetComponentInChildren<BulletSlot>();
            curSC.curSlotID = each.curSlotID;
            curSlotSC.BulletID = each.bulletID;
            curSlotSC.InstanceID = each.instanceID;
            
            BulletIns.transform.SetParent(UIManager.Instance.G_Bullet.transform,false);
            BulletIns.transform.position = curSlot.transform.position;
        }

        SyncBulletIcon();
    }

    public void SyncBulletIcon()
    {
        GameObject bulletIconRoot = UIManager.Instance.G_CurBulletIcon;
        for (int i = 0; i < 5; i++)
        {
            int curSlotID = i + 1;
            BulletReady curBulletReady = null;
            foreach (var each in CurBullets)
            {
                if (each.curSlotID == curSlotID)
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
                    Sprite>(PathConfig.GetBulletImagePath(curBulletReady.bulletID, BulletInsMode.Icon));
            }
        }
    }
    
    public void RefreshCurBulletSlots()
    {
        GameObject roleSlotRoot = UIManager.Instance.G_BulletRoleSlot;
        GameObject bulletRoot = UIManager.Instance.G_Bullet;
        //1)重置Slot
        BulletSlot[] bulletSlots = roleSlotRoot.GetComponentsInChildren<BulletSlot>();
        foreach (BulletSlot each in bulletSlots)
        {
            each.BulletID = 0;
            each.InstanceID = 0;
        }

        //2)设置Slot的BulletID
        for (int i = roleSlotRoot.transform.childCount - 1; i >= 0; i--)
        {
            GameObject curSlot = roleSlotRoot.transform.GetChild(i).gameObject;
            BulletSlot curSlotSC = curSlot.GetComponentInChildren<BulletSlot>();
            foreach (var each in CurBullets)
            {
                if (each.curSlotID == curSlotSC.SlotID)
                {
                    curSlotSC.BulletID = each.bulletID;
                    curSlotSC.InstanceID = each.instanceID;
                }
            }
        }
        //3)设置Bullet的SlotID
        for (int i = bulletRoot.transform.childCount - 1; i >= 0; i--)
        {
            GameObject curBullet = bulletRoot.transform.GetChild(i).gameObject;
            DraggableBullet curSC = curBullet.GetComponentInChildren<DraggableBullet>();
            foreach (var each in CurBullets)
            {
                if (each.bulletID == curSC._bulletData.ID
                    && each.instanceID == curSC.InstanceID)
                    curSC.curSlotID = each.curSlotID;
            }
        }
        //4)刷新Icon
        SyncBulletIcon();
    }

    /// <summary>
    /// 满足两个逻辑，一个是子弹空拖动，一个是子弹交换拖动
    /// </summary>
    /// <param name="curSlotID"></param>
    /// <param name="targetSlotID"></param>
    public void BulletInterchangePos(int curSlotID,int targetSlotID)
    {
        GameObject curSlot = GetSlot(curSlotID, SlotKind.SlotRole);
        GameObject targetSlot = GetSlot(targetSlotID, SlotKind.SlotRole);
        GameObject curIns = GetReadyBulletBySlotID(curSlotID);
        DraggableBullet curSC = curIns.GetComponentInChildren<DraggableBullet>();
        GameObject targetIns = GetReadyBulletBySlotID(targetSlotID);
        
        if (targetIns != null)//子弹交换逻辑
        {
            DraggableBullet targetSC = targetIns.GetComponentInChildren<DraggableBullet>();
            //挪出来
            RefreshCurBullets(BulletMutMode.Sub, targetSC._bulletData.ID,targetSlotID,targetSC.InstanceID);
            RefreshCurBullets(BulletMutMode.Add,targetSC._bulletData.ID,curSlotID,targetSC.InstanceID);
            targetIns.transform.position = curSlot.transform.position;
            targetIns.GetComponentInChildren<DraggableBullet>().curSlotID = curSlotID;
        }
        //移进去
        RefreshCurBullets(BulletMutMode.Sub, curSC._bulletData.ID,curSlotID,curSC.InstanceID);  //....数据层
        RefreshCurBullets(BulletMutMode.Add, curSC._bulletData.ID,targetSlotID,curSC.InstanceID); //....数据层
        curIns.transform.position = targetSlot.transform.position;
        curIns.GetComponentInChildren<DraggableBullet>().curSlotID = targetSlotID;
        
        RefreshCurBulletSlots();//....GO层
    }
    
    public void InstanceStandbyBullets()
    {
        GameObject SDSlotRoot = UIManager.Instance.G_SlotStandby;
        GameObject SDBulletRoot = UIManager.Instance.G_BulletStandby;
        //..............Clear Old Data..................
        for (int i = SDBulletRoot.transform.childCount - 1; i >= 0; i--)
            DestroyImmediate(SDBulletRoot.transform.GetChild(i).gameObject);
        //..............Instance New Data..................
        for (int i = 0; i < SDSlotRoot.transform.childCount; i++)
        {
            GameObject curSlot = SDSlotRoot.transform.GetChild(i).gameObject;
            BulletSlotStandby curSlotSC = curSlot.GetComponent<BulletSlotStandby>();
            if (CurStandbyBullets[i].BulletID != 0)
            {
                curSlotSC.BulletID = CurStandbyBullets[i].BulletID;
                //curSlotSC.InstanceID =  CurStandbyBullets[i].;
                BulletManager.Instance.InstanceStandByBullet(CurStandbyBullets[i].BulletID,curSlot);
            }
        }
    }
    
    public bool AddStandbyBullet(int BulletID,int InstanceID)
    {
        GameObject BulletIns = BulletManager.Instance.InstanceStandByBullet(BulletID);
        if (BulletIns == null)
            return false;

        RefreshStandbyBullets(BulletMutMode.Add, BulletID,InstanceID);
        return true;
    }

    public void SubStandebyBullet(int BulletID,int InstanceID = -1)
    {
        RefreshStandbyBullets(BulletMutMode.Sub, BulletID,InstanceID);
        GameObject curSD = UIManager.Instance.G_BulletStandby;
        GameObject curSDSlot = UIManager.Instance.G_SlotStandby;

        if (InstanceID == -1)//全删
        {
            for (int i = curSD.transform.childCount-1 ; i >= 0; i--)
            {
                GameObject curBullet = curSD.transform.GetChild(i).gameObject;
                StandbyBullet curSC = curBullet.GetComponentInChildren<StandbyBullet>();
                if (curSC._bulletData.ID == BulletID)
                {
                    foreach (var each in CurStandbyBullets)
                    {
                        if (each.BulletID == curSC._bulletData.ID )
                        {
                            each.BulletID = 0;
                        }
                    }
                    curSC.DestroySelf();
                }
            }

            for (int i = curSDSlot.transform.childCount - 1; i >= 0; i--)
            {
                GameObject curSlot = curSDSlot.transform.GetChild(i).gameObject;
                BulletSlot curSC = curSlot.GetComponentInChildren<BulletSlot>();
                if (curSC.BulletID == BulletID)
                {
                    curSC.BulletID = 0;
                }
            }
        }
        else//根据InstanceID删一个
        {
            for (int i = curSD.transform.childCount-1 ; i >= 0; i--)
            {
                GameObject curBullet = curSD.transform.GetChild(i).gameObject;
                StandbyBullet curSC = curBullet.GetComponentInChildren<StandbyBullet>();
                if (curSC._bulletData.ID == BulletID)
                {
                    foreach (var each in CurStandbyBullets)
                    {
                        if (each.BulletID == curSC._bulletData.ID &&
                            curSC.InstanceID == InstanceID)
                        {
                            each.BulletID = 0;
                        }
                    }
                    curSC.DestroySelf();
                }
            }

            for (int i = curSDSlot.transform.childCount - 1; i >= 0; i--)
            {
                GameObject curSlot = curSDSlot.transform.GetChild(i).gameObject;
                BulletSlot curSC = curSlot.GetComponentInChildren<BulletSlot>();
                if (curSC.BulletID == BulletID && curSC.InstanceID == InstanceID)
                {
                    curSC.BulletID = 0;
                }
            }
        }
        
    }
    #endregion

    #region 外部可以调用的操作组封装

    public bool InstanceIDIsInCurBullet(int instanceID)
    {
        bool instanceIDIsInCurBullet = false;
        //如果这个Instance有了，证明是在RoleSlot内空拖行为，删掉原来的数据再更新
        for (int i = CurBullets.Count - 1; i >= 0; i--)
        {
            BulletReady preData = CurBullets[i];
            if (preData.instanceID == instanceID)
                instanceIDIsInCurBullet = true;
        }
        return instanceIDIsInCurBullet;
    }

    public void TmpHongSpawner(int bulletID)
    {
        RefreshSpawner(BulletMutMode.Sub,bulletID);
        RefreshSpawnerIns();
    }
    public void AddBullet(int bulletID,int slotID,int instanceID)
    {
        RefreshCurBullets(BulletMutMode.Add,bulletID,slotID,instanceID);
        RefreshSpawner(BulletMutMode.Sub,bulletID);
        RefreshSpawnerIns();
        RefreshCurBulletSlots();
    }
    
    public void AddBulletOnlyData(int bulletID,int slotID,int instanceID)
    {
        RefreshCurBullets(BulletMutMode.Add,bulletID,slotID,instanceID);
        RefreshCurBulletSlots();
       // RefreshSpawner(BulletMutMode.Sub,bulletID);
    }

    public void SubBullet(int bulletID,int instanceID)
    {
        RefreshCurBullets(BulletMutMode.Sub,bulletID,InstanceID:instanceID);
        RefreshSpawner(BulletMutMode.Add,bulletID);
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
                root = UIManager.Instance.G_BulletRoleSlot;
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
        GameObject bulletRoot = UIManager.Instance.G_Bullet;
        for (int i = bulletRoot.transform.childCount - 1; i >= 0; i--)
        {
            GameObject curbullet = bulletRoot.transform.GetChild(i).gameObject;
            DraggableBullet curSC = curbullet.GetComponentInChildren<DraggableBullet>();
            if (curSC.curSlotID == slotID)
                curIns = curbullet;
        }
        return curIns;
    }
    #endregion
}