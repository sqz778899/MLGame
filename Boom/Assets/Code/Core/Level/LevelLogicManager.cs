using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LevelLogicalManager : ScriptableObject
{
    #region 单例
    static LevelLogicalManager s_instance;
        
    public static LevelLogicalManager Instance
    {
        get
        {
            if (s_instance == null)
                s_instance = ResManager.instance.GetAssetCache<LevelLogicalManager>(PathConfig.LevelManagerOBJ);
            return s_instance;
        }
    }
    #endregion

    GameObject GroupBullet;
    //..................键盘响应相关.................
    public void CheckForKeyPress(Vector3 pos)
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Fire(pos);
    }
    public void Fire(Vector3 pos)
    {
        //...............CheckBullet......................
        //...............FireBullet.......................
        Debug.Log("fire");
        GameObject bullet = Instantiate(ResManager.instance.GetAssetCache<GameObject>(PathConfig.BulletAssetDir + "P_Bullet_Inner_01.prefab"),pos,quaternion.identity);
        if (GroupBullet == null)
            GroupBullet = GameObject.Find("GroupBullet");

        if (GroupBullet!=null)
            bullet.transform.SetParent(GroupBullet.transform);
    }
}