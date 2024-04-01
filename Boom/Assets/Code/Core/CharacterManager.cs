using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

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

    #region 存档相关
    SaveFileJson _saveFile = new SaveFileJson();
    public void SetSaveFileTemplate()
    {
        _saveFile = new SaveFileJson();
        BagDataJson bagDataJson = new BagDataJson();
        List<SingleSlot> bagSlots = new List<SingleSlot>();
        bagDataJson.bagSlots = bagSlots;
        _saveFile.BagData = bagDataJson;

        for (int i = 0; i < 12; i++)
        {
            SingleSlot temSlot = new SingleSlot();
            temSlot.slotID = i + 1;
            temSlot.bulletCount = 0;
            temSlot.bulletID = 0;
            bagSlots.Add(temSlot);
        }
        bagDataJson.slotRole01 = 0;
        bagDataJson.slotRole02 = 0;
        bagDataJson.slotRole03 = 0;
        bagDataJson.slotRole04 = 0;
        bagDataJson.slotRole05 = 0;
        
        string content01 = JsonConvert.SerializeObject(_saveFile,(Formatting) Formatting.Indented);
        File.WriteAllText(PathConfig.SaveFileJson, content01);
    }
    
    public void LoadSaveFile()
    {
        string SaveFileJsonString = File.ReadAllText(PathConfig.SaveFileJson);
        _saveFile = JsonConvert.DeserializeObject<SaveFileJson>(SaveFileJsonString);
        _bagData = new BagData();
        _bagData.InitDataByJson(_saveFile.BagData,BulletDesignJsons);
        WinOrFailState = WinOrFail.InLevel;
        SetBullet();
    }

    public void SaveFile()
    {
        _saveFile.BagData = _bagData.SetDataJson();
        string content01 = JsonConvert.SerializeObject(_saveFile,(Formatting) Formatting.Indented);
        File.WriteAllText(PathConfig.SaveFileJson, content01);
    }
    #endregion
    
    public GameObject BulletGroup;
    public GameObject GroupBulletSlot;
    public GameObject GroupBulletSlotRole;
    public List<BulletData> Bullets;
    public List<BulletDataJson> BulletDesignJsons;
    
    BagData _bagData;//背包相关
    public int Score;
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
        
    }
    
    public void SetBullet()
    {
        InitData();
        if (BulletGroup == null || GroupBulletSlot == null)
            return;

        //............SlotRole 更新.....................
        _bagData.ClearBagData();
        for (int i = 0; i < BulletGroup.transform.childCount; i++)
        {
            GameObject perBullet = BulletGroup.transform.GetChild(i).gameObject;
            DraggableBullet perSc = perBullet.GetComponentInChildren<DraggableBullet>();
            BulletEditMode curBulletSate = perSc.BulletState;
            switch (curBulletSate)
            {
                case BulletEditMode.SlotRole01:
                    GroupBulletSlotRole.transform.GetChild(i).GetComponent<BulletSlotRole>().IsHaveBullet = true;
                    _bagData.slotRole01 = perSc._bulletData.ID;
                    break;
                case BulletEditMode.SlotRole02:
                    _bagData.slotRole02 = perSc._bulletData.ID;
                    GroupBulletSlotRole.transform.GetChild(i).GetComponent<BulletSlotRole>().IsHaveBullet = true;
                    break;
                case BulletEditMode.SlotRole03:
                    _bagData.slotRole03 = perSc._bulletData.ID;
                    GroupBulletSlotRole.transform.GetChild(i).GetComponent<BulletSlotRole>().IsHaveBullet = true;
                    break;
                case BulletEditMode.SlotRole04:
                    _bagData.slotRole04 = perSc._bulletData.ID;
                    GroupBulletSlotRole.transform.GetChild(i).GetComponent<BulletSlotRole>().IsHaveBullet = true;
                    break;
                case BulletEditMode.SlotRole05:
                    _bagData.slotRole05 = perSc._bulletData.ID;
                    GroupBulletSlotRole.transform.GetChild(i).GetComponent<BulletSlotRole>().IsHaveBullet = true;
                    break;
            }
        }
        
        //...........BagSlot 更新......................
        DraggableBulletSpawner[] Spawners = GroupBulletSlot.GetComponentsInChildren<DraggableBulletSpawner>();
       
        foreach (SingleSlot each in _bagData.bagSlots)
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
        _bagData.RefreshBullets(BulletDesignJsons);
        Bullets = _bagData.curBullets;
        
        SaveFile();
        Debug.Log("Set BulletSlotRole");
    }

    //..............初始化相关....................
    void OnEnable()
    {
        BulletDesignJsons = LoadBulletData();
        Score = 0;
    }
    
    public List<BulletDataJson> LoadBulletData()
    {
        string BulletDesignString = File.ReadAllText(PathConfig.BulletDesignJson);
        List<BulletDataJson> BulletDataJsons = JsonConvert.DeserializeObject<List<BulletDataJson>>(BulletDesignString);
        return BulletDataJsons;
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

