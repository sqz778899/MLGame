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

    #region 一些重要的UIGroup
    //..........EditBulletScene............
    GameObject BulletGroup;
    GameObject GroupBulletSlot;
    GameObject GroupBulletSlotRole;
    
    //..........MapScene............
    GameObject GroupRoll;
    GameObject GroupSlotStandby;
    GameObject GroupRollBullet;
    #endregion
    
    //...............子弹上膛................
    public List<BulletData> Bullets;
    
    //...............重要数据................
    public BagData BagData = new BagData();//背包相关
    public int Score;
    public int Gold;
    public int Cost = 5;
    public List<StandbyData> CurStandbyBullets = new List<StandbyData>();
    public List<SupremeCharm> SupremeCharms = new List<SupremeCharm>();

    public WinOrFail WinOrFailState;

    public void InitData()
    {
        //..........EditBulletScene............
        if (BulletGroup == null)
            BulletGroup = GameObject.Find("GroupBullet");
        if (GroupBulletSlot == null)
            GroupBulletSlot = GameObject.Find("GroupBulletSlot");
        if (GroupBulletSlotRole == null)
            GroupBulletSlotRole = GameObject.Find("imBulletSlotRole");
        
        //..........MapScene............
        if (GroupRoll == null)
            GroupRoll = GameObject.Find("GroupRoll");
        if (GroupSlotStandby == null)
            GroupSlotStandby = GameObject.Find("GroupSlotStandby");
        if (GroupRollBullet == null)
            GroupRollBullet = GameObject.Find("GroupRollBullet");

        if (GroupSlotStandby != null && GroupRollBullet!= null)
        {
            List<StandbyData> saveSD = TrunkManager.Instance._saveFile.UserStandbyBullet;
            for (int i = 0; i < GroupSlotStandby.transform.childCount; i++)
            {
                GameObject curSlot = GroupSlotStandby.transform.GetChild(i).gameObject;
                BulletSlotStandby curSlotSC = curSlot.GetComponent<BulletSlotStandby>();
                curSlotSC.SlotID = saveSD[i].SlotID;
                curSlotSC.curBulletID = saveSD[i].BulletID;
                if (curSlotSC.curBulletID != 0)
                {
                    BulletManager.Instance.InstanceStandByBullet(curSlotSC.curBulletID,GroupRollBullet,curSlot);
                }
            }
        }

        if (GroupBulletSlotRole != null)
        {
            for (int i = 0; i < GroupBulletSlotRole.transform.childCount; i++)
                GroupBulletSlotRole.transform.GetChild(i)
                    .GetComponent<BulletSlotRole>().IsHaveBullet = false;
            
            WinOrFailState = WinOrFail.InLevel;
            SetBullet();
        }
    }
    
    void SetBullet()
    {
        if (BulletGroup == null || GroupBulletSlot == null)
            return;

        //............SlotRole 更新.....................
        BagData.ClearBagData();
        for (int i = 0; i < GroupBulletSlotRole.transform.childCount; i++)
            GroupBulletSlotRole.transform.GetChild(i)
                .GetComponent<BulletSlotRole>().IsHaveBullet = false;
        for (int i = 0; i < BulletGroup.transform.childCount; i++)
        {
            GameObject perBullet = BulletGroup.transform.GetChild(i).gameObject;
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
        
        //...........BagSlot 更新......................
        DraggableBulletSpawner[] Spawners = GroupBulletSlot.GetComponentsInChildren<DraggableBulletSpawner>();
       
        foreach (SingleSlot each in BagData.bagSlots)
        {
            foreach (DraggableBulletSpawner eachSpawner in Spawners)
            {
                if (each.slotID == eachSpawner._bulletData.ID)
                {
                    each.bulletID = eachSpawner._bulletData.ID;
                    each.bulletCount = eachSpawner.Count;
                }
            }
        }
        //.....................子弹上膛.........................
        BagData.RefreshBullets(TrunkManager.Instance.BulletDesignJsons);
        Bullets = BagData.curBullets;
        
        //TrunkManager.Instance.SaveFile();
        Debug.Log("Set BulletSlotRole");
    }

    public bool AddStandbyBullet(int BulletID)
    {
        bool isAdd = false;
        InitData();
        GameObject curSlot = null;
        BulletSlotStandby curSlotSC = null;
        for (int i = 0; i < GroupSlotStandby.transform.childCount; i++)
        {
            GameObject tmpSlot = GroupSlotStandby.transform.GetChild(i).gameObject;
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
        BulletManager.Instance.InstanceStandByBullet(BulletID,GroupRollBullet,curSlot);
        return isAdd;
    }
    

    public void SetStandbyBullets()
    {
        CurStandbyBullets = new List<StandbyData>();
        for (int i = 0; i < GroupSlotStandby.transform.childCount; i++)
        {
            StandbyData curSD = new StandbyData();
            GameObject curSlot = GroupSlotStandby.transform.GetChild(i).gameObject;
            BulletSlotStandby curSlotSC = curSlot.GetComponent<BulletSlotStandby>();
            curSD.SlotID = curSlotSC.SlotID;
            curSD.BulletID = curSlotSC.curBulletID;
            CurStandbyBullets.Add(curSD);
        }
        TrunkManager.Instance.SaveFile();
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

