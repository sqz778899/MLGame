using System;
using System.Collections.Generic;
using System.Linq;
using Spine.Unity;
using Spine.Unity.Editor;
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
        if (bulletInsMode == BulletInsMode.Spawner)
        {
            DraggableBulletSpawner bulletSpawner = Bullet.GetComponentInChildren<DraggableBulletSpawner>();
            bulletSpawner.ID = ID;
        }
        Bullet bulletbase = Bullet.GetComponentInChildren<Bullet>();
        //curAniSC?.Initialize(true);
        bulletbase.Ins = Bullet;
        bulletbase.ID = ID;
        bulletbase.InstanceID = Bullet.GetInstanceID();
        bulletbase.BulletInsMode = bulletInsMode;
        SkeletonAnimation curAniSC = Bullet.GetComponentInChildren<SkeletonAnimation>();
        if (curAniSC != null)
            SpineEditorUtilities.ReloadSkeletonDataAssetAndComponent(curAniSC);
        return Bullet;
    }
    
    public GameObject InstanceBullet(BulletJson curBulletJson,BulletInsMode bulletInsMode
        ,Vector3 pos = new Vector3())
    {
        //先把地址上这个变量记一下，不然下面就变了
        int _finalDamage = curBulletJson.FinalDamage;
        int _finalPiercing = curBulletJson.FinalPiercing;
        int _finalResonance = curBulletJson.FinalResonance;
        
        GetIns(bulletInsMode, out GameObject Bullet,pos);
        if (bulletInsMode == BulletInsMode.Spawner)
        {
            DraggableBulletSpawner bulletSpawner = Bullet.GetComponentInChildren<DraggableBulletSpawner>();
            bulletSpawner.ID = curBulletJson.ID;
        }
        Bullet bulletbase = Bullet.GetComponentInChildren<Bullet>();
        SkeletonAnimation curAniSC = Bullet.GetComponentInChildren<SkeletonAnimation>();
        //curAniSC?.Initialize(true);
        bulletbase.Ins = Bullet;
        bulletbase.ID = curBulletJson.ID;
        bulletbase.InstanceID = Bullet.GetInstanceID();
        bulletbase.BulletInsMode = bulletInsMode;
        //同步宝石数据
        bulletbase.FinalDamage = _finalDamage;
        bulletbase.FinalPiercing = _finalPiercing;
        bulletbase.FinalResonance = _finalResonance;
        
        if (curAniSC != null)
            SpineEditorUtilities.ReloadSkeletonDataAssetAndComponent(curAniSC);
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

    #region 给Tooltip用
    public BulletTooltipInfo GetBulletInfo(int bulletID)
    {
        BulletJson curDsData = TrunkManager.Instance.GetBulletDesignData(bulletID);
        if (curDsData == null) return null;
      
        BulletTooltipInfo tInfo = new BulletTooltipInfo();
        tInfo.bulletImage = ResManager.instance.GetAssetCache<Sprite>(
            PathConfig.GetBulletImageOrSpinePath(bulletID, BulletInsMode.Thumbnail));
        tInfo.name = curDsData.Name;    
        //tInfo.name = curDsData.Name;
        string ElementalType = ((ElementalTypes)curDsData.ElementalType).ToString();
        tInfo.description = string.Format("Lv: {0}\nDamage: {1}\nElement: {2}",
            curDsData.Level,curDsData.Damage,ElementalType);
        return tInfo;
    }

    #endregion
}