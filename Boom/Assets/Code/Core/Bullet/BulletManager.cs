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
}