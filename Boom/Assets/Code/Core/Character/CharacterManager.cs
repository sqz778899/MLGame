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
    public List<BulletData> CurBullets;
    
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
        InstanceBagBullet();
        InitStandbyBullet();
            
        WinOrFailState = WinOrFail.InLevel;
    }

    public void InitStandbyBullet()
    {
        List<StandbyData> saveSD = TrunkManager.Instance._saveFile.UserStandbyBullet;
        for (int i = 0; i < UIManager.Instance.GroupSlotStandby.transform.childCount; i++)
        {
            GameObject curSlot = UIManager.Instance.GroupSlotStandby.transform.GetChild(i).gameObject;
            BulletSlotStandby curSlotSC = curSlot.GetComponent<BulletSlotStandby>();
            curSlotSC.SlotID = saveSD[i].SlotID;
            curSlotSC.BulletID = saveSD[i].BulletID;
            if (curSlotSC.BulletID != 0)
            {
                BulletManager.Instance.InstanceStandByBullet(curSlotSC.BulletID,curSlot);
            }
        }
    }

    public void SetBullet()
    {
        //............SlotRole 更新.....................
        GameObject SlotRoot = UIManager.Instance.GroupBulletSlotRole;
        GameObject GroupBullet = UIManager.Instance.GroupBullet;
        //............Clear.................
        for (int i = 0; i < SlotRoot.transform.childCount; i++)
        {
            GameObject perSlot = SlotRoot.transform.GetChild(i).gameObject;
            BulletSlotRole perSlotSc = perSlot.GetComponentInChildren<BulletSlotRole>();
            perSlotSc.BulletID = 0;
        }
        //............Refresh.................
        for (int i = 0; i < GroupBullet.transform.childCount; i++)
        {
            GameObject perBullet = GroupBullet.transform.GetChild(i).gameObject;
            DraggableBullet perSc = perBullet.GetComponentInChildren<DraggableBullet>();
            for (int j = 0; j < SlotRoot.transform.childCount; j++)
            {
                GameObject perSlot = SlotRoot.transform.GetChild(i).gameObject;
                BulletSlotRole perSlotSc = perSlot.GetComponentInChildren<BulletSlotRole>();
                if (perSlotSc.SlotID == perSc.CurBagSlotID)
                    perSlotSc.BulletID = perSc._bulletData.ID;
            }
        }
        
        //...........Spawner 更新......................
        DraggableBulletSpawner[] Spawners = UIManager.Instance
            .GroupBulletSlot.GetComponentsInChildren<DraggableBulletSpawner>();
        CurBulletSpawners = new List<BulletSpawner>();
        foreach (DraggableBulletSpawner eachSpawner in Spawners)
            CurBulletSpawners.Add(new BulletSpawner(eachSpawner._bulletData.ID, eachSpawner.Count));
        
        //.....................子弹上膛.........................
        //CurBullets = BagData.curBullets;
        
        Debug.Log("Set BulletSlotRole");
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

    public void RefreshCurBullets(BulletMutMode mode, int BulletID,
        BulletInsMode bulletInsMode = BulletInsMode.EditA)
    {
        switch (mode)
        {
            case BulletMutMode.Sub:
                foreach (var each in CurBullets)
                {
                    if (each.ID == BulletID)
                        CurBullets.Remove(each);
                }
                break;
            case BulletMutMode.Add:
                BulletData curData = new BulletData(BulletID);
                curData.SetDataByID(bulletInsMode);
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

    public bool AddStandbyBullet(int BulletID)
    {
        GameObject BulletIns = BulletManager.Instance.InstanceStandByBullet(BulletID);
        if (BulletIns == null)
            return false;
        SetStandbyBullets();
        return true;
    }

    public void SetStandbyBullets()
    {
        CurStandbyBullets = new List<StandbyData>();
        for (int i = 0; i < UIManager.Instance.GroupSlotStandby.transform.childCount; i++)
        {
            GameObject curSlot = UIManager.Instance.GroupSlotStandby.transform.GetChild(i).gameObject;
            BulletSlotStandby curSlotSC = curSlot.GetComponent<BulletSlotStandby>();
            StandbyData curSD = new StandbyData( curSlotSC.BulletID,curSlotSC.SlotID);
            CurStandbyBullets.Add(curSD);
        }
        TrunkManager.Instance.SaveFile();
    }

    void InstanceBagBullet()
    {
        GameObject groupBulletSlot = UIManager.Instance.GroupBulletSlot;
        GameObject groupBulletSlotRole = UIManager.Instance.GroupBulletSlotRole;
        GameObject groupBullet = UIManager.Instance.GroupBullet;
        BulletSlot[] allSlots = groupBulletSlot.GetComponentsInChildren<BulletSlot>();

        foreach (var each in CurBulletSpawners)
        {
            GameObject bagSlotBullet = BulletManager.Instance.
                InstanceBullet(each.bulletID,BulletInsMode.Spawner);
            DraggableBulletSpawner perSc = bagSlotBullet.GetComponentInChildren<DraggableBulletSpawner>();
            perSc.Count = each.bulletCount;
            foreach (var eachSlot in allSlots)
            {
                BulletSlot curSC = eachSlot;
                if (curSC.SlotID == (each.bulletID%10))
                {
                    bagSlotBullet.transform.SetParent(curSC.gameObject.transform,false);
                    bagSlotBullet.transform.localScale = Vector3.one;
                }
            }
        }

        /*//实例化bullet
        if (BagData.slotRole01 != 0)
            _instanceslotRole(BagData.slotRole01,1);
        if (BagData.slotRole02 != 0)
            _instanceslotRole(BagData.slotRole02,2);
        if (BagData.slotRole03 != 0)
            _instanceslotRole(BagData.slotRole03,3);
        if (BagData.slotRole04 != 0)
            _instanceslotRole(BagData.slotRole04,4);
        if (BagData.slotRole05 != 0)
            _instanceslotRole(BagData.slotRole05,5);

        void _instanceslotRole(int bulletID,int slotIndex)
        {
            //
            GameObject curSlotRole = groupBulletSlotRole.transform.GetChild(slotIndex - 1).gameObject;
            BulletSlotRole curSlotRoleSC = curSlotRole.GetComponent<BulletSlotRole>();
            
            GameObject slotRoleGO = BulletManager.Instance.InstanceBullet(bulletID,BulletInsMode.EditB);
            slotRoleGO.transform.SetParent(groupBullet.transform);
            slotRoleGO.transform.localScale = Vector3.one;
            slotRoleGO.transform.position = curSlotRole.transform.position;
        }*/
    }

    #region 模板
    /*
    void Temp()
    {
        BulletDataJson ss = new BulletDataJson();
        ss.speed = 10;
        ss.damage = 1;
        ss.bulletPrefabName = "P_Bullet_Inner_01";
        ss.bulletEditAName = "P_Bullet_Edit_a_01";
        ss.bulletEditBName = "P_Bullet_Edit_b_01";
        ss.bulleSpawnerName = "P_Bullet_Spawner_01";
        ss.hitEffectName = "";
        List<BulletDataJson> ppp = new List<BulletDataJson>();
        ppp.Add(ss);
        string content01 = JsonConvert.SerializeObject(ppp,(Formatting) Formatting.Indented);
        File.WriteAllText(PathConfig.BulletDesignJson, content01);
    }*/
    #endregion
}

