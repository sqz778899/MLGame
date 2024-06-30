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

    #region InstanceBullet
    void GetIns(BulletInsMode bulletInsMode,out GameObject Bullet,Vector3 pos = new Vector3())
    {
        Bullet = Instantiate(
            ResManager.instance.GetAssetCache<GameObject>
                (PathConfig.GetBulletTemplate(bulletInsMode)),pos,
            quaternion.identity);
    }

    public GameObject InstanceRollBulletMat(int ID, BulletInsMode bulletInsMode
        , Vector3 pos = new Vector3())
    {
        GetIns(bulletInsMode, out GameObject RBMIns,pos);
        RollBulletMat curSC = RBMIns.GetComponent<RollBulletMat>();
        curSC.ID = ID;
        curSC.InitImg();
        return RBMIns;
    }

    public GameObject InstanceBullet(int ID,BulletInsMode bulletInsMode
        ,Vector3 pos = new Vector3())
    {
        GetIns(bulletInsMode, out GameObject Bullet,pos);
        BulletBase bulletbase = Bullet.GetComponentInChildren<BulletBase>();
        bulletbase.Ins = Bullet;
        bulletbase._bulletData.ID = ID;
        bulletbase.InstanceID = Bullet.GetInstanceID();
        bulletbase.bulletInsMode = bulletInsMode;
        bulletbase.InitBulletData();
        return Bullet;
    }
    
    //使用真的InstanceID
    public GameObject InstanceBullet(BulletData bulletData,
        BulletInsMode bulletInsMode,Vector3 pos = new Vector3())
    {
        GetIns(bulletInsMode, out GameObject Bullet,pos);
        BulletBase bulletbase = Bullet.GetComponentInChildren<BulletBase>();
        bulletbase.Ins = Bullet;
        bulletbase.InstanceID = Bullet.GetInstanceID();
        bulletbase._bulletData = bulletData;
        bulletbase.bulletInsMode = bulletInsMode;
        bulletbase.InitBulletData();
        return Bullet;
    }

    public GameObject InstanceStandbyMat(int bulletID,GameObject curSlot = null)
    {
        GetIns(BulletInsMode.Standby, out GameObject StandbyMatIns);
        StandbyBulletMat curSC = StandbyMatIns.GetComponent<StandbyBulletMat>();
        curSC.InitData(bulletID);
        StandbyMatIns.transform.SetParent(
            UIManager.Instance.G_StandbyMat.transform,false);
        return StandbyMatIns;
    }
    #endregion
  
    public void BulletUpgrade()
    {
        //.................检查一下是否有子弹需要升级...............
        Dictionary<int, int> IDCount = new Dictionary<int, int>();
        List<StandbyData> curSBMs = MainRoleManager.Instance.CurStandbyBulletMats;
        for (int i = 0; i < curSBMs.Count; i++)
        {
            if (curSBMs[i].ID != 0)
            {
                if (!IDCount.ContainsKey(curSBMs[i].ID))
                    IDCount.Add(curSBMs[i].ID,1);
                else
                    IDCount[curSBMs[i].ID] += 1;
            }
        }
        
        //.................升级...............
        foreach (var each in IDCount)
        {
            if (each.Value == 2)
            {
                foreach (var eachSpawner in MainRoleManager.Instance.CurBulletSpawners)
                {
                    if (eachSpawner.bulletID == each.Key)
                    {
                        //Upgrade
                        MainRoleManager.Instance.SubStandebyBullet(each.Key);//DelAll
                        foreach (var eachBullet in MainRoleManager.Instance.CurBullets)
                        {
                            if (eachBullet.bulletID == eachSpawner.bulletID)
                                eachBullet.bulletID += 100;
                        }
                        eachSpawner.bulletID += 100;
                    }
                }
            }
            if (each.Value == 3)
            {
                MainRoleManager.Instance.SubStandebyBullet(each.Key);//DelAll
                MainRoleManager.Instance.AddStandbyBulletMat(each.Key+100);
                BulletUpgrade();
            }
        }
        Debug.Log("Upgrade !!!!");
    }

    #region 给Tooltip用
    public BulletTooltipInfo GetBulletInfo(int bulletID)
    {
        BulletDataJson curDsData = TrunkManager.Instance.GetBulletDesignData(bulletID);
        BulletTooltipInfo tInfo = new BulletTooltipInfo();
        tInfo.bulletImage = ResManager.instance.GetAssetCache<Sprite>(
            PathConfig.GetBulletImagePath(bulletID, BulletInsMode.Thumbnail));
        tInfo.name = curDsData.name;
        string ElementalType = ((ElementalTypes)curDsData.elementalType).ToString();
        tInfo.description = string.Format("Lv: {0}\nDamage: {1}\nElement: {2}",
            curDsData.Level,curDsData.damage,ElementalType);
        return tInfo;
    }

    #endregion
}