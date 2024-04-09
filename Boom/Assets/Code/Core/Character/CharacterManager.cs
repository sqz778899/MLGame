using UnityEngine;
using System;
using System.Collections.Generic;

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
    public List<BulletData> Bullets;
    
    //...............重要数据................
    public BagData BagData = new BagData();//背包相关
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
        
        for (int i = 0; i < UIManager.Instance.GroupBulletSlotRole.transform.childCount; i++)
            UIManager.Instance.GroupBulletSlotRole.transform.GetChild(i)
                .GetComponent<BulletSlotRole>().IsHaveBullet = false;
            
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
            curSlotSC.curBulletID = saveSD[i].BulletID;
            if (curSlotSC.curBulletID != 0)
            {
                BulletManager.Instance.InstanceStandByBullet(curSlotSC.curBulletID,curSlot);
            }
        }
    }

    public void SetBullet()
    {
        //............SlotRole 更新.....................
        BagData.ClearRoleSlotData();
        GameObject GroupBulletSlotRole = UIManager.Instance.GroupBulletSlotRole;
        for (int i = 0; i < GroupBulletSlotRole.transform.childCount; i++)
            GroupBulletSlotRole.transform.GetChild(i)
                .GetComponent<BulletSlotRole>().IsHaveBullet = false;
        
        for (int i = 0; i < UIManager.Instance.GroupBullet.transform.childCount; i++)
        {
            GameObject perBullet = UIManager.Instance.GroupBullet.transform.GetChild(i).gameObject;
            DraggableBullet perSc = perBullet.GetComponentInChildren<DraggableBullet>();
            BulletEditMode curBulletSate = perSc.BulletState;
            switch (curBulletSate)
            {
                case BulletEditMode.SlotRole01:
                    GroupBulletSlotRole.transform.GetChild(0).GetComponent<BulletSlotRole>().IsHaveBullet = true;
                    BagData.slotRole01 = perSc._bulletData.ID;
                    break;
                case BulletEditMode.SlotRole02:
                    BagData.slotRole02 = perSc._bulletData.ID;
                    GroupBulletSlotRole.transform.GetChild(1).GetComponent<BulletSlotRole>().IsHaveBullet = true;
                    break;
                case BulletEditMode.SlotRole03:
                    BagData.slotRole03 = perSc._bulletData.ID;
                    GroupBulletSlotRole.transform.GetChild(2).GetComponent<BulletSlotRole>().IsHaveBullet = true;
                    break;
                case BulletEditMode.SlotRole04:
                    BagData.slotRole04 = perSc._bulletData.ID;
                    GroupBulletSlotRole.transform.GetChild(3).GetComponent<BulletSlotRole>().IsHaveBullet = true;
                    break;
                case BulletEditMode.SlotRole05:
                    BagData.slotRole05 = perSc._bulletData.ID;
                    GroupBulletSlotRole.transform.GetChild(4).GetComponent<BulletSlotRole>().IsHaveBullet = true;
                    break;
            }
        }
        
        //...........Spawner 更新......................
        DraggableBulletSpawner[] Spawners = UIManager.Instance
            .GroupBulletSlot.GetComponentsInChildren<DraggableBulletSpawner>();
        CurBulletSpawners = new List<BulletSpawner>();
        foreach (DraggableBulletSpawner eachSpawner in Spawners)
            CurBulletSpawners.Add(new BulletSpawner(eachSpawner._bulletData.ID,eachSpawner.Count));
        
        //.....................子弹上膛.........................
        BagData.RefreshBullets();
        Bullets = BagData.curBullets;
        
        Debug.Log("Set BulletSlotRole");
    }

    public bool AddStandbyBullet(int BulletID)
    {
        bool isAdd = false;
        GameObject curSlot = null;
        BulletSlotStandby curSlotSC = null;
        for (int i = 0; i < UIManager.Instance.GroupSlotStandby.transform.childCount; i++)
        {
            GameObject tmpSlot = UIManager.Instance.GroupSlotStandby.transform.GetChild(i).gameObject;
            curSlotSC = tmpSlot.GetComponent<BulletSlotStandby>();
            if (curSlotSC.curBulletID == 0)
            {
                curSlot = tmpSlot;
                curSlotSC.curBulletID = BulletID;
                break;
            }
        }
        if (curSlot==null) return false;
        
        SetStandbyBullets();
        isAdd = true;
        BulletManager.Instance.InstanceStandByBullet(BulletID,curSlot);
        return isAdd;
    }

    public void SetStandbyBullets()
    {
        CurStandbyBullets = new List<StandbyData>();
        for (int i = 0; i < UIManager.Instance.GroupSlotStandby.transform.childCount; i++)
        {
            StandbyData curSD = new StandbyData();
            GameObject curSlot = UIManager.Instance.GroupSlotStandby.transform.GetChild(i).gameObject;
            BulletSlotStandby curSlotSC = curSlot.GetComponent<BulletSlotStandby>();
            curSD.SlotID = curSlotSC.SlotID;
            curSD.BulletID = curSlotSC.curBulletID;
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
                if (curSC.SlotID == each.bulletID)
                {
                    bagSlotBullet.transform.SetParent(curSC.gameObject.transform,false);
                    bagSlotBullet.transform.localScale = Vector3.one;
                }
            }
        }

        //实例化bullet
        if (BagData.slotRole01 != 0)
            _instanceslotRole(BagData.slotRole01,1,BulletEditMode.SlotRole01);
        if (BagData.slotRole02 != 0)
            _instanceslotRole(BagData.slotRole02,2,BulletEditMode.SlotRole02);
        if (BagData.slotRole03 != 0)
            _instanceslotRole(BagData.slotRole03,3,BulletEditMode.SlotRole03);
        if (BagData.slotRole04 != 0)
            _instanceslotRole(BagData.slotRole04,4,BulletEditMode.SlotRole04);
        if (BagData.slotRole05 != 0)
            _instanceslotRole(BagData.slotRole05,5,BulletEditMode.SlotRole05);

        void _instanceslotRole(int slotRoleID,int slotIndex,BulletEditMode bulletEditMode)
        {
            //
            GameObject curSlotRole = groupBulletSlotRole.transform.GetChild(slotIndex - 1).gameObject;
            BulletSlotRole curSlotRoleSC = curSlotRole.GetComponent<BulletSlotRole>();
            curSlotRoleSC.IsHaveBullet = true;
            
            GameObject slotRoleGO = BulletManager.Instance.InstanceBullet(slotRoleID,BulletInsMode.EditB);
            DraggableBullet slotRoleGOSc = slotRoleGO.GetComponentInChildren<DraggableBullet>();
            slotRoleGOSc.BulletState = bulletEditMode;
            slotRoleGO.transform.SetParent(groupBullet.transform);
            slotRoleGO.transform.localScale = Vector3.one;
            slotRoleGO.transform.position = curSlotRole.transform.position;
        }
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

