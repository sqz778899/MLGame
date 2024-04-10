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

    public void RefreshCurBullets(BulletMutMode mode, int BulletID,int SlotID = -1,
        BulletInsMode bulletInsMode = BulletInsMode.EditA)
    {
        switch (mode)
        {
            case BulletMutMode.Sub:
                for (int i = CurBullets.Count - 1; i >= 0; i--)
                {
                    if (CurBullets[i].bulletID == BulletID)
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
                BulletReady curData = new BulletReady(BulletID,SlotID);
                CurBullets.Add(curData);
                break;
        }
    }
    
    public void RefreshStandbyBullets(BulletMutMode mode, int BulletID)
    {
        switch (mode)
        {
            case BulletMutMode.Sub:
                foreach (var each in CurStandbyBullets)
                {
                    if (each.BulletID == BulletID)
                        CurStandbyBullets.Remove(each);
                }
                break;
            case BulletMutMode.Add:
                foreach (var each in CurStandbyBullets)
                {
                    if (each.BulletID == 0)
                        each.BulletID = BulletID;
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
        {
            DestroyImmediate(oldSpawner[i].gameObject);
        }
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
                    newSpawnerIns.GetComponentInChildren<DraggableBulletSpawner>().Count = each.bulletCount;
                    newSpawnerIns.transform.SetParent(slots[i].gameObject.transform);
                    newSpawnerIns.transform.localScale = Vector3.one;
                    newSpawnerIns.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
                }
            }
        }
    }

    public void InstanceCurBullets()
    {
        GameObject roleSlotRoot = UIManager.Instance.GroupBulletSlotRole;
        GameObject bulletRoot = UIManager.Instance.GroupBullet;
        //..............Clear Old Data..................
        for (int i = bulletRoot.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(bulletRoot.transform.GetChild(i).gameObject);
        }
        //..............Instance New Data..................
        foreach (BulletReady each in CurBullets)
        {
            GameObject BulletIns = BulletManager.Instance.InstanceBullet(each.bulletID,BulletInsMode.EditA);
            BulletIns.transform.SetParent(bulletRoot.transform);
            BulletIns.transform.localScale = Vector3.one;
            BulletIns.name = "ssssss";
            BulletIns.transform.position = roleSlotRoot.transform.GetChild(each.curSlotID - 1).position;
        }
    }
    
    public void InstanceStandbyBullets()
    {
        GameObject SDSlotRoot = UIManager.Instance.GroupSlotStandby;
        GameObject SDBulletRoot = UIManager.Instance.GroupBulletStandby;
        //..............Clear Old Data..................
        for (int i = SDBulletRoot.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(SDBulletRoot.transform.GetChild(i).gameObject);
        }
        //..............Instance New Data..................
        for (int i = 0; i < SDSlotRoot.transform.childCount; i++)
        {
            GameObject curSlot = SDSlotRoot.transform.GetChild(i).gameObject;
            BulletSlotStandby curSlotSC = curSlot.GetComponent<BulletSlotStandby>();
            if (CurStandbyBullets[i].BulletID != 0)
            {
                curSlotSC.BulletID = CurStandbyBullets[i].BulletID;
                BulletManager.Instance.InstanceStandByBullet(CurStandbyBullets[i].BulletID,curSlot);
            }
        }
    }
    
    public bool AddStandbyBullet(int BulletID)
    {
        GameObject BulletIns = BulletManager.Instance.InstanceStandByBullet(BulletID);
        if (BulletIns == null)
            return false;
        return true;
    }
    #endregion

    public void AddBullet(int bulletID,int slotID)
    {
        RefreshCurBullets(BulletMutMode.Add,bulletID,slotID);
        RefreshSpawner(BulletMutMode.Sub,bulletID);
        InstanceSpawners();
        InstanceCurBullets();
    }

    public void SubBullet(int bulletID)
    {
        RefreshCurBullets(BulletMutMode.Sub,bulletID);
        RefreshSpawner(BulletMutMode.Add,bulletID);
        InstanceSpawners();
        InstanceCurBullets();
    }
}

