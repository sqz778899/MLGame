using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

#region 一些枚举类
public enum BulletMutMode
{
    Add = 1,
    Sub = 2,
}

public enum BulletInsMode
{
    Inner = 1,
    EditA = 2,
    EditB = 3,
    Spawner = 4,
    Standby = 6,
    Thumbnail = 7,
    Icon = 8,
    Mat = 9
}

public enum RollBulletMatType
{
    Mat = 1,
    Score = 2
}
#endregion

#region 主要的子弹类
public enum ElementalTypes
{
    Non = 1,
    Ice = 2,
    Fire = 3,
    Electric = 4
}

[Serializable]
public class StandbyData:HaveID
{
    public int SlotID;
    public int InstanceID;

    public StandbyData(int slotID = 0,int instanceID = 0)
    {
        SlotID = slotID;
        InstanceID = instanceID;
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
public class BulletReady
{
    public int bulletID;
    public int curSlotID;
    public int instanceID;
    public BulletReady(int _bulletID,int _curSlotID,int _instanceID = -1)
    {
        bulletID = _bulletID;
        curSlotID = _curSlotID;
        instanceID = _instanceID;
    }

    public BulletData GetBulletData()
    {
        BulletData curData = new BulletData(bulletID);
        curData.SetDataByID();
        return curData;
    }
}

[Serializable]
public class BulletData
{
    //.............Attribute..............
    public int ID;
    public int Level;
    public string name;
    public int damage;
    public int penetration;
    public ElementalTypes elementalType;
    
    //..............Instance..............
    public Sprite imgBullet;     //....image自动根据ID设置
    public GameObject hitEffect; // 击中效果预制体

    public void SetDataByID(BulletInsMode bulletInsMode = BulletInsMode.EditA)
    {
        BulletDataJson curData = null;
        List<BulletDataJson> BulletDesignJsons = TrunkManager.Instance.BulletDesignJsons;
        foreach (BulletDataJson eachDesignJson in BulletDesignJsons)
        {
            if (eachDesignJson.ID == ID)
            {
                curData = eachDesignJson;
                break;
            }
        }
        ID = curData.ID;
        Level = curData.Level;
        name = curData.name;
        damage = curData.damage;
        penetration = curData.penetration;
        elementalType = (ElementalTypes)curData.elementalType;

        imgBullet = ResManager.instance.GetAssetCache<Sprite>(PathConfig.GetBulletImagePath(ID, bulletInsMode));
        //实例化Prefab
        hitEffect = ResManager.instance.GetAssetCache<GameObject>(
            PathConfig.FXAssetDir + curData.hitEffectName + ".prefab");
    }

    public BulletDataJson GetJsonData()
    {
        if (ID == null)
            return null;

        BulletDataJson curDataJson = null;
        foreach (BulletDataJson perDataJson in TrunkManager.Instance.BulletDesignJsons)
        {
            if (ID == perDataJson.ID)
                curDataJson = perDataJson;
        }
        return curDataJson;
    }

    public BulletData(int _id)
    {
        ID = _id;
    }
}
#endregion

public class BulletDataJson
{
    public int ID;
    public int Level;
    public string name;
    public int damage;
    public int penetration;
    public int elementalType;
    
    public string hitEffectName;
}

public class BulletTooltipInfo
{
    public Sprite bulletImage;
    public string name;
    public string description;
}
