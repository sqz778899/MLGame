using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SingleSlot
{
    public int slotID;
  

    public SingleSlot(int _slotID=0)
    {
        slotID = _slotID;
    }
}

[Serializable]
public class BulletSpawner
{
    public int bulletID;
    public int bulletCount;
    public BulletSpawner(int _bulletID = 0,int _bulletCount = 0)
    {
        bulletID = _bulletID;
        bulletCount = _bulletCount;
    }
}

[Serializable]
public class BagData
{
    public BagData()
    {
        bagSlots = new List<SingleSlot>();
    }
    public List<SingleSlot> bagSlots;
    
    public int slotRole01;
    public int slotRole02;
    public int slotRole03;
    public int slotRole04;
    public int slotRole05;

    BulletData FindBulletData(int ID)
    {
        BulletData curBulletData = new BulletData(ID);
        bool isFind = false;
        foreach (var each in TrunkManager.Instance.BulletDesignJsons)
        {
            if (each.ID == ID)
            {
                isFind = true;
                curBulletData.SetDataByID();
                break;
            }
        }

        if (isFind)
            return curBulletData;
        else
            return null;
    }
    public void ClearRoleSlotData()
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
        bagSlots = BagJson.bagSlots;
    }

    public BagDataJson SetDataJson()
    {
        BagDataJson BagJson = new BagDataJson();
        BagJson.bagSlots = bagSlots;
        return BagJson;
    }
    #endregion
}

public class BagDataJson
{
    public List<SingleSlot> bagSlots;
    
}