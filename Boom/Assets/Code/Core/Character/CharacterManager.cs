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
    
    public GameObject BulletGroup;
    public GameObject GroupBulletSlot;
    public GameObject GroupBulletSlotRole;
    public List<BulletData> Bullets;
    
    //...............重要数据................
    public BagData BagData = new BagData();//背包相关
    public int Score;
    public int Gold;
    public int Cost = 5;
    public List<SupremeCharm> SupremeCharms = new List<SupremeCharm>();
    
    
    public WinOrFail WinOrFailState;

    public void InitData()
    {
        if (BulletGroup == null)
            BulletGroup = GameObject.Find("GroupBullet");
        if (GroupBulletSlot == null)
            GroupBulletSlot = GameObject.Find("GroupBulletSlot");
        if (GroupBulletSlotRole == null)
            GroupBulletSlotRole = GameObject.Find("imBulletSlotRole");

        for (int i = 0; i < GroupBulletSlotRole.transform.childCount; i++)
            GroupBulletSlotRole.transform.GetChild(i)
                .GetComponent<BulletSlotRole>().IsHaveBullet = false;

        WinOrFailState = WinOrFail.InLevel;
        SetBullet();
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

