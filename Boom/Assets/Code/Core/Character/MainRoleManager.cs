using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

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
    
    //...............重要数据................
    public int Score;
    public int Gold;
    public int Cost = 5;
    public List<BulletSpawner> CurBulletSpawners;
    public List<StandbyData> CurStandbyBullets = new List<StandbyData>();
    public List<SupremeCharm> SupremeCharms = new List<SupremeCharm>();

    public WinOrFail WinOrFailState;

    public void InitData()
    {
        InstanceSpawners();
        InstanceCurBullets();
        InstanceStandbyBullets();
        
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
        //CurBulletSpawners
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
            GameObject curSlot = roleSlotRoot.transform.GetChild(each.curSlotID - 1).gameObject;
            BulletSlot curSlotSC = curSlot.GetComponentInChildren<BulletSlot>();
            curSC.curSlotID = each.curSlotID;
            curSlotSC.BulletID = each.bulletID;
            curSlotSC.InstanceID = each.instanceID;
            
            BulletIns.transform.SetParent(UIManager.Instance.G_Bullet.transform,false);
            BulletIns.transform.position = curSlot.transform.position;
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

    public bool IsNullDrag(int instanceID)
    {
        bool isNullDrag = false;
        //如果这个Instance有了，证明是在RoleSlot内空拖行为，删掉原来的数据再更新
        for (int i = CurBullets.Count - 1; i >= 0; i--)
        {
            BulletReady preData = CurBullets[i];
            if (preData.instanceID == instanceID)
                isNullDrag = true;
        }
        return isNullDrag;
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
        RefreshSpawner(BulletMutMode.Sub,bulletID);
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

