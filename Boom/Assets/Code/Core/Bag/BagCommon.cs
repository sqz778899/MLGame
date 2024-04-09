using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SingleSlot
{
    public int slotID;
    public int bulletID;
    public int bulletCount;
}

[Serializable]
public class BulletSpawner
{
    public int BulletID;
    public int BulletCount;
}

[Serializable]
public class BagData
{
    public BagData()
    {
        bagSlots = new List<SingleSlot>();
        curBullets = new List<BulletData>();
    }
    public List<SingleSlot> bagSlots;
    
    public int slotRole01;
    public int slotRole02;
    public int slotRole03;
    public int slotRole04;
    public int slotRole05;

    public List<BulletData> curBullets;

    public void RefreshBullets()
    {
        if (curBullets == null)
            curBullets = new List<BulletData>();
        curBullets.Clear();
        if (slotRole01 != 0)
            curBullets.Add(FindBulletData(slotRole01));
        if (slotRole02 != 0)
            curBullets.Add(FindBulletData(slotRole02));
        if (slotRole03 != 0)
            curBullets.Add(FindBulletData(slotRole03));
        if (slotRole04 != 0)
            curBullets.Add(FindBulletData(slotRole04));
        if (slotRole05 != 0)
            curBullets.Add(FindBulletData(slotRole05));
    }

    BulletData FindBulletData(int ID)
    {
        BulletData curBulletData = new BulletData();
        bool isFind = false;
        foreach (var each in TrunkManager.Instance.BulletDesignJsons)
        {
            if (each.ID == ID)
            {
                isFind = true;
                curBulletData.ID = ID;
                curBulletData.SetDataByID();
                break;
            }
        }

        if (isFind)
            return curBulletData;
        else
            return null;
    }
    public void ClearBagData()
    {
        foreach (var each in bagSlots)
        {
            each.bulletID = 0;
            each.bulletCount = 0;
        }
        slotRole01 = 0;
        slotRole02 = 0;
        slotRole03 = 0;
        slotRole04 = 0;
        slotRole05 = 0;
    }
    
    #region IO
    public void InitDataByJson(BagDataJson BagJson)
    {
        bagSlots = BagJson.bagSlots;
        slotRole01 = BagJson.slotRole01;
        slotRole02 = BagJson.slotRole02;
        slotRole03 = BagJson.slotRole03;
        slotRole04 = BagJson.slotRole04;
        slotRole05 = BagJson.slotRole05;
    }

    public BagDataJson SetDataJson()
    {
        BagDataJson BagJson = new BagDataJson();
        BagJson.bagSlots = bagSlots;
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
    public List<SingleSlot> bagSlots;

    public int slotRole01;
    public int slotRole02;
    public int slotRole03;
    public int slotRole04;
    public int slotRole05;
}