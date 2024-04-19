using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class CharacterManager :ScriptableObject
{
    #region 单例
    static CharacterManager s_instance;
    
    public static CharacterManager Instance
    {
        get
        {
            if (s_instance == null)
                s_instance = ResManager.instance.GetAssetCache<CharacterManager>(PathConfig.CharacterManagerOBJ);
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
            .GroupBulletSlot.GetComponentsInChildren<DraggableBulletSpawner>();
        for (int i = oldSpawner.Length - 1; i >= 0; i--)
            DestroyImmediate(oldSpawner[i].gameObject);
        //..............Instance New Data..................
        BulletSlot[] slots = UIManager.Instance.GroupBulletSlot.GetComponentsInChildren<BulletSlot>();
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
            .GroupBulletSlot.GetComponentsInChildren<DraggableBulletSpawner>();
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
        GameObject roleSlotRoot = UIManager.Instance.GroupBulletSlotRole;
        GameObject bulletRoot = UIManager.Instance.GroupBullet;
        //..............Clear Old Data..................
        for (int i = bulletRoot.transform.childCount - 1; i >= 0; i--)
            DestroyImmediate(bulletRoot.transform.GetChild(i).gameObject);
        //..............Instance New Data..................
        foreach (BulletReady each in CurBullets)
        {
            GameObject BulletIns = BulletManager.Instance.InstanceBullet(each.bulletID,BulletInsMode.EditA);
            DraggableBullet curSC = BulletIns.GetComponentInChildren<DraggableBullet>();
            GameObject curSlot = roleSlotRoot.transform.GetChild(each.curSlotID - 1).gameObject;
            BulletSlot curSlotSC = curSlot.GetComponentInChildren<BulletSlot>();
            curSC.curSlotID = each.curSlotID;
            curSlotSC.BulletID = each.bulletID;
            curSlotSC.InstanceID = each.instanceID;
            SetBulletPos(BulletIns.transform, curSlot.transform);
        }
    }
    
    public void RefreshCurBulletSlots()
    {
        GameObject roleSlotRoot = UIManager.Instance.GroupBulletSlotRole;
        GameObject bulletRoot = UIManager.Instance.GroupBullet;
        //设置Slot的BulletID
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
        //设置Bullet的SlotID
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

    public void BulletInterchangePos(int curSlotID,int targetSlotID)
    {
        GameObject curSlot = GetSlot(curSlotID, SlotKind.SlotRole);
        GameObject targetSlot = GetSlot(targetSlotID, SlotKind.SlotRole);
        GameObject curIns = GetReadyBulletBySlotID(curSlotID);
        GameObject targetIns = GetReadyBulletBySlotID(targetSlotID);
        DraggableBullet curSC = curIns.GetComponentInChildren<DraggableBullet>();
        DraggableBullet targetSC = targetIns.GetComponentInChildren<DraggableBullet>();
        
        //挪出来
        RefreshCurBullets(BulletMutMode.Sub, targetSC._bulletData.ID,targetSlotID,targetSC.InstanceID);
        RefreshCurBullets(BulletMutMode.Add,targetSC._bulletData.ID,curSlotID,targetSC.InstanceID);
        targetIns.transform.position = curSlot.transform.position;
        targetIns.GetComponentInChildren<DraggableBullet>().curSlotID = curSlotID;
        //移进去
        RefreshCurBullets(BulletMutMode.Sub, curSC._bulletData.ID,curSlotID,curSC.InstanceID);
        RefreshCurBullets(BulletMutMode.Add, curSC._bulletData.ID,targetSlotID,curSC.InstanceID);
        curIns.transform.position = targetSlot.transform.position;
        curIns.GetComponentInChildren<DraggableBullet>().curSlotID = targetSlotID;
    }
    
    public void InstanceStandbyBullets()
    {
        GameObject SDSlotRoot = UIManager.Instance.GroupSlotStandby;
        GameObject SDBulletRoot = UIManager.Instance.GroupBulletStandby;
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
        GameObject curSD = UIManager.Instance.GroupBulletStandby;
        GameObject curSDSlot = UIManager.Instance.GroupSlotStandby;

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
    public void AddBullet(int bulletID,int slotID,int instanceID)
    {
        RefreshCurBullets(BulletMutMode.Add,bulletID,slotID,instanceID);
        RefreshSpawner(BulletMutMode.Sub,bulletID);
        RefreshSpawnerIns();
        RefreshCurBulletSlots();
    }

    public void SubBullet(int bulletID,int instanceID)
    {
        RefreshCurBullets(BulletMutMode.Sub,bulletID,InstanceID:instanceID);
        RefreshSpawner(BulletMutMode.Add,bulletID);
        RefreshSpawnerIns();
        RefreshCurBulletSlots();
    }

    public void SetBulletPos(Transform bulletTrans,Transform slotTrans)
    {
        bulletTrans.SetParent(UIManager.Instance.GroupBullet.transform);
        bulletTrans.localScale = Vector3.one;
        bulletTrans.position = slotTrans.position;
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
                root = UIManager.Instance.GroupBulletSlotRole;
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
        GameObject bulletRoot = UIManager.Instance.GroupBullet;
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

