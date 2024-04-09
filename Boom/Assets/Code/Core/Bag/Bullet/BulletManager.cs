using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BulletManager :ScriptableObject
{
    #region 单例
    static BulletManager s_instance;
    
    public static BulletManager Instance
    {
        get
        {
            if (s_instance == null)
                s_instance = ResManager.instance.GetAssetCache<BulletManager>(PathConfig.BulletManagerOBJ);
            return s_instance;
        }
    }
    #endregion
    void GetIns(BulletInsMode bulletInsMode,out GameObject Bullet,Vector3 pos = new Vector3())
    {
        Bullet = GameObject.Instantiate(
            ResManager.instance.GetAssetCache<GameObject>
                (PathConfig.GetBulletTemplate(bulletInsMode)),pos,
            quaternion.identity);
    }
   
    public GameObject InstanceBullet(int ID,BulletInsMode bulletInsMode
        ,Vector3 pos = new Vector3())
    {
        GetIns(bulletInsMode, out GameObject Bullet,pos);
        BulletBase bulletbase = Bullet.GetComponentInChildren<BulletBase>();
        bulletbase._bulletData.ID = ID;
        bulletbase.bulletInsMode = bulletInsMode;
        bulletbase.InitBulletData();
        return Bullet;
    }
    
    public GameObject InstanceBullet(BulletData bulletData,
        BulletInsMode bulletInsMode,Vector3 pos = new Vector3())
    {
        GetIns(bulletInsMode, out GameObject Bullet,pos);
        BulletBase bulletbase = Bullet.GetComponentInChildren<BulletBase>();
        bulletbase._bulletData = bulletData;
        bulletbase.bulletInsMode = bulletInsMode;
        bulletbase.InitBulletData();
        return Bullet;
    }

    public GameObject InstanceStandByBullet(int bulletID,GameObject curSlot)
    {
        GameObject curSDIns = InstanceBullet(bulletID, BulletInsMode.Standby);
        curSDIns.transform.SetParent(UIManager.Instance.GroupBulletStandby.transform);
        curSDIns.transform.position = Vector3.zero;
        curSDIns.transform.localScale = Vector3.one;
        curSDIns.GetComponent<RectTransform>().anchoredPosition3D =
            curSlot.GetComponent<RectTransform>().anchoredPosition3D;
        return curSDIns;
    }

    public void BulletUpgrade()
    {
        GameObject curSD = UIManager.Instance.GroupBulletStandby;
        Dictionary<int, int> IDCount = new Dictionary<int, int>();
        for (int i = 0; i < curSD.transform.childCount; i++)
        {
            GameObject curBullet = curSD.transform.GetChild(i).gameObject;
            StandbyBullet curSC = curBullet.GetComponentInChildren<StandbyBullet>();
            if (!IDCount.ContainsKey(curSC._bulletData.ID))
                IDCount.Add(curSC._bulletData.ID,1);
            else
                IDCount[curSC._bulletData.ID] += 1;
        }

        foreach (var each in IDCount)
        {
            if (each.Value == 2)
            {
                //Check
                //CharacterManager.Instance.BagData.bagSlots
            }
            if (each.Value == 3)
            {
                //Upgrade
            }
        }
        Debug.Log("Upgrade !!!!");
    }
}