using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

#region 一些枚举类
public enum BulletEditMode
{
    Non = 0,
    SlotRole01 = 1,
    SlotRole02 = 2,
    SlotRole03 = 3,
    SlotRole04 = 4,
    SlotRole05 = 5,
}

public enum BulletInsMode
{
    Inner = 1,
    EditA = 2,
    EditB = 3,
    Spawner = 4,
    Roll = 5,
    Standby = 6
}
#endregion

#region 主要的子弹类
[Serializable]
public class BulletData
{
    //.............Attribute..............
    public int ID;
    public int Level;
    public string name;
    public float speed;
    public int damage;
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
        speed = curData.speed;
        damage = curData.damage;
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
    
}
#endregion

public class BulletDataJson
{
    public int ID;
    public int Level;
    public string name;
    public float speed;
    public int damage;
    public int elementalType;
    
    public string hitEffectName;
}
