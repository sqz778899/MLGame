using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

#region 一些枚举类
public enum ElementalTypes
{
    NonElemental = 1,
    Ice = 2,
    Fire = 3,
    Electric = 4
}

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
    Spawner = 4
}
#endregion

#region 主要的子弹类
[Serializable]
public class BulletData
{
    //.............Attribute..............
    public int ID;
    public string name;
    public float speed;
    public int damage;
    public ElementalTypes elementalType;
    
    //..............Instance..............
    public GameObject bulletPrefab;
    public GameObject bulletEditAPrefab;
    public GameObject bulletEditBPrefab;
    public GameObject bulletSpawnerPrefab;
    public GameObject hitEffect; // 击中效果预制体

    public void SetDataByID()
    {
        BulletDataJson curData = null;
        List<BulletDataJson> BulletDesignJsons = CharacterManager.Instance.BulletDesignJsons;
        foreach (BulletDataJson eachDesignJson in BulletDesignJsons)
        {
            if (eachDesignJson.ID == ID)
            {
                curData = eachDesignJson;
                break;
            }
        }
        ID = curData.ID;
        name = curData.name;
        speed = curData.speed;
        damage = curData.damage;
        elementalType = (ElementalTypes)curData.elementalType;
        bulletPrefab = ResManager.instance.GetAssetCache<GameObject>(
                PathConfig.BulletAssetDir + curData.bulletPrefabName + ".prefab");
        bulletEditAPrefab = ResManager.instance.GetAssetCache<GameObject>(
            PathConfig.BulletAssetDir + curData.bulletEditAName + ".prefab");
        bulletEditBPrefab = ResManager.instance.GetAssetCache<GameObject>(
            PathConfig.BulletAssetDir + curData.bulletEditBName + ".prefab");
        bulletSpawnerPrefab = ResManager.instance.GetAssetCache<GameObject>(
            PathConfig.BulletAssetDir + curData.bulleSpawnerName + ".prefab");
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

    public GameObject InstanceBullet(Vector3 pos = new Vector3(),BulletInsMode insMode= BulletInsMode.Inner)
    {
        SetDataByID();
        List<BulletDataJson> BulletDesignJsons = CharacterManager.Instance.BulletDesignJsons;
        BulletDataJson curDesign = null;
        foreach (BulletDataJson eachDesign in BulletDesignJsons)
        {
            if (eachDesign.ID == ID)
            {
                curDesign = eachDesign;
                break;
            }
        }

        if (curDesign != null)
        {
            GameObject bullet = null;
            switch (insMode)
            {
                case BulletInsMode.Inner:
                    bullet = GameObject.Instantiate(bulletPrefab,pos,quaternion.identity);
                    break;
                case BulletInsMode.EditA:
                    bullet = GameObject.Instantiate(bulletEditAPrefab,pos,quaternion.identity);
                    break;
                case BulletInsMode.EditB:
                    bullet = GameObject.Instantiate(bulletEditBPrefab,pos,quaternion.identity);
                    break;
                case BulletInsMode.Spawner:
                    bullet = GameObject.Instantiate(bulletSpawnerPrefab,pos,quaternion.identity);
                    break;
            }
            bullet.transform.localScale = Vector3.one;
            BulletBase bulletBase = bullet.GetComponentInChildren<BulletBase>();
            bulletBase._bulletData = this;
            bulletBase.InitBulletData();
            return bullet;
        }
        return null;
    }
}
#endregion


public class BulletDataJson
{
    public int ID;
    public string name;
    public float speed;
    public int damage;
    public int elementalType;
    
    public string bulletPrefabName;
    public string bulletEditAName;
    public string bulletEditBName;
    public string bulleSpawnerName;
    public string hitEffectName;
}