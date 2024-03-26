using System;
using System.Collections.Generic;
using UnityEngine;

public enum BulletEditMode
{
    Non = 0,
    SlotRole01 = 1,
    SlotRole02 = 2,
    SlotRole03 = 3,
    SlotRole04 = 4,
    SlotRole05 = 5,
}

[Serializable]
public class BulletData
{
    public int ID;
    public string name;
    public float speed;
    public int damage;
    public GameObject bulletPrefab;
    public GameObject bulletEditAPrefab;
    public GameObject bulletEditBPrefab;
    public GameObject hitEffect; // 击中效果预制体

    public void SetDataByJson(BulletDataJson curData)
    {
        ID = curData.ID;
        name = curData.name;
        speed = curData.speed;
        damage = curData.damage;
        bulletPrefab = ResManager.instance.GetAssetCache<GameObject>(
                PathConfig.BulletAssetDir + curData.bulletPrefabName + ".prefab");
        bulletEditAPrefab = ResManager.instance.GetAssetCache<GameObject>(
            PathConfig.BulletAssetDir + curData.bulletEditAName + ".prefab");
        bulletEditBPrefab = ResManager.instance.GetAssetCache<GameObject>(
            PathConfig.BulletAssetDir + curData.bulletEditBName + ".prefab");
        hitEffect = ResManager.instance.GetAssetCache<GameObject>(
            PathConfig.BulletAssetDir + curData.hitEffectName + ".prefab");
    }

    public BulletDataJson GetJsonData()
    {
        if (ID == null)
            return null;

        BulletDataJson curDataJson = null;
        List<BulletDataJson> bulletDataJsons = CharacterManager.Instance.LoadBulletData();
        foreach (BulletDataJson perDataJson in bulletDataJsons)
        {
            if (ID == perDataJson.ID)
                curDataJson = perDataJson;
        }
        return curDataJson;
    }
}

public class BagData
{
    public int slotID;
    public int bulletID;
    public int bulletCount;

    public int slotRole01;
    public int slotRole02;
    public int slotRole03;
    public int slotRole04;
    public int slotRole05;

    public List<BulletData> curBullets;

    public void InitCurBullets(List<BulletDataJson> BulletDesignJsons)
    {
        if (curBullets == null)
            curBullets = new List<BulletData>();
        if (slotRole01 != 0)
            curBullets.Add(FindBulletData(BulletDesignJsons,slotRole01));
        if (slotRole02 != 0)
            curBullets.Add(FindBulletData(BulletDesignJsons,slotRole02));
        if (slotRole03 != 0)
            curBullets.Add(FindBulletData(BulletDesignJsons,slotRole03));
        if (slotRole04 != 0)
            curBullets.Add(FindBulletData(BulletDesignJsons,slotRole04));
        if (slotRole05 != 0)
            curBullets.Add(FindBulletData(BulletDesignJsons,slotRole05));
    }

    BulletData FindBulletData(List<BulletDataJson> BulletDesignJsons,int ID)
    {
        BulletData curBulletData = new BulletData();
        bool isFind = false;
        foreach (var each in BulletDesignJsons)
        {
            if (each.ID == ID)
            {
                isFind = true;
                curBulletData.SetDataByJson(each);
                break;
            }
        }

        if (isFind)
            return curBulletData;
        else
            return null;
    }
    public void ClearSlotRole()
    {
        slotRole01 = 0;
        slotRole02 = 0;
        slotRole03 = 0;
        slotRole04 = 0;
        slotRole05 = 0;
    }

    #region IO
    public void InitDataByJson(BagDataJson BagJson)
    {
        slotID = BagJson.slotID;
        bulletID = BagJson.bulletID;
        bulletCount = BagJson.bulletCount;
        slotRole01 = BagJson.slotRole01;
        slotRole02 = BagJson.slotRole02;
        slotRole03 = BagJson.slotRole03;
        slotRole04 = BagJson.slotRole04;
        slotRole05 = BagJson.slotRole05;
    }

    public BagDataJson SetDataJson()
    {
        BagDataJson BagJson = new BagDataJson();
        BagJson.slotID = slotID;
        BagJson.bulletID = bulletID;
        BagJson.bulletCount = bulletCount;
        BagJson.slotRole01 = slotRole01;
        BagJson.slotRole02 = slotRole02;
        BagJson.slotRole03 = slotRole03;
        BagJson.slotRole04 = slotRole04;
        BagJson.slotRole05 = slotRole05;
        return BagJson;
    }
    #endregion
}

public class BagDataJson
{
    public int slotID;
    public int bulletID;
    public int bulletCount;

    public int slotRole01;
    public int slotRole02;
    public int slotRole03;
    public int slotRole04;
    public int slotRole05;
}

public class BulletDataJson
{
    public int ID;
    public string name;
    public float speed;
    public int damage;
    public string bulletPrefabName;
    public string bulletEditAName;
    public string bulletEditBName;
    public string hitEffectName;
}

public class SaveFileJson
{
    public BagDataJson BagData;
}