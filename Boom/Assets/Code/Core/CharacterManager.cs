using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

[CreateAssetMenu]
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
        _bagData.InitDataByJson(_saveFile.BagData,_bulletDesignJsons);
    }

    public void SaveFile()
    {
        _saveFile.BagData = _bagData.SetDataJson();
        string content01 = JsonConvert.SerializeObject(_saveFile,(Formatting) Formatting.Indented);
        File.WriteAllText(PathConfig.SaveFileJson, content01);
    }
    #endregion
    
    public GameObject BulletGroup;
    public List<BulletData> Bullets;
    
    BagData _bagData;
    List<BulletDataJson> _bulletDesignJsons;
    public void SetBullet(GameObject Bullet)
    {
        BulletGroup = GameObject.Find("GroupBullet");
        if (BulletGroup == null)
            return;

        //............SlotRole 更新.....................
        _bagData.ClearSlotRole();
        for (int i = 0; i < BulletGroup.transform.childCount; i++)
        {
            GameObject perBullet = BulletGroup.transform.GetChild(i).gameObject;
            DraggableBullet perSc = perBullet.GetComponentInChildren<DraggableBullet>();
            int BagSlot = perSc.CurBagSlotID;
            BulletEditMode curBulletSate = perSc.BulletState;
            switch (curBulletSate)
            {
                case BulletEditMode.SlotRole01:
                    _bagData.slotRole01 = perSc._bulletData.ID;
                    break;
                case BulletEditMode.SlotRole02:
                    _bagData.slotRole02 = perSc._bulletData.ID;
                    break;
                case BulletEditMode.SlotRole03:
                    _bagData.slotRole03 = perSc._bulletData.ID;
                    break;
                case BulletEditMode.SlotRole04:
                    _bagData.slotRole04 = perSc._bulletData.ID;
                    break;
                case BulletEditMode.SlotRole05:
                    _bagData.slotRole05 = perSc._bulletData.ID;
                    break;
            }
        }
        _bagData.RefreshBullets(_bulletDesignJsons);
        Bullets = _bagData.curBullets;
        
        //...........BagSlot 更新......................
        //int BagSlot = Bullet.GetComponent<DraggableBullet>().CurBagSlotID;
        
        
        SaveFile();
        Debug.Log("Set BulletSlotRole");
    }

    void OnEnable()
    {
        _bulletDesignJsons = LoadBulletData();
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
        ss.hitEffectName = "";
        List<BulletDataJson> ppp = new List<BulletDataJson>();
        ppp.Add(ss);
        string content01 = JsonConvert.SerializeObject(ppp,(Formatting) Formatting.Indented);
        File.WriteAllText(PathConfig.BulletDesignJson, content01);
    }*/
    #endregion
}

