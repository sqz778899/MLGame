using System;
using System.Collections.Generic;
using System.Linq;
using Spine.Unity;
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
    
    public GameObject InstanceStandbyMat(int bulletID,GameObject curSlot = null)
    {
        GetIns(BulletInsMode.Standby, out GameObject StandbyMatIns);
        StandbyBulletMat curSC = StandbyMatIns.GetComponent<StandbyBulletMat>();
        curSC.InitData(bulletID);
        StandbyMatIns.transform.SetParent(
            UIManager.Instance.CommonUI.StandbyRoot.transform,false);
        return StandbyMatIns;
    }
    #endregion
}