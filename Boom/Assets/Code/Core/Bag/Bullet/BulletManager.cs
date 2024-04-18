using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

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
        bulletbase.InstanceID = Bullet.GetInstanceID();
        bulletbase.bulletInsMode = bulletInsMode;
        bulletbase.InitBulletData();
        return Bullet;
    }
    
    public GameObject InstanceBullet(BulletData bulletData,
        BulletInsMode bulletInsMode,Vector3 pos = new Vector3())
    {
        GetIns(bulletInsMode, out GameObject Bullet,pos);
        BulletBase bulletbase = Bullet.GetComponentInChildren<BulletBase>();
        bulletbase.InstanceID = Bullet.GetInstanceID();
        bulletbase._bulletData = bulletData;
        bulletbase.bulletInsMode = bulletInsMode;
        bulletbase.InitBulletData();
        return Bullet;
    }

    public GameObject InstanceStandByBullet(int bulletID,GameObject curSlot = null)
    {
        GameObject curSDIns = null;
        if (curSlot != null)
        {
            curSDIns = InstanceBullet(bulletID, BulletInsMode.Standby);
            curSDIns.transform.SetParent(UIManager.Instance.GroupBulletStandby.transform);
            curSDIns.transform.position = Vector3.zero;
            curSDIns.transform.localScale = Vector3.one;
            curSDIns.GetComponent<RectTransform>().anchoredPosition3D =
                curSlot.GetComponent<RectTransform>().anchoredPosition3D;
        }
        else
        {
            GameObject SlotGroup = UIManager.Instance.GroupSlotStandby;
            for (int i = 0; i < SlotGroup.transform.childCount; i++)
            {
                GameObject tmpSlot = SlotGroup.transform.GetChild(i).gameObject;
                BulletSlotStandby curSlotSC = tmpSlot.GetComponent<BulletSlotStandby>();
                if (curSlotSC.BulletID == 0)
                {
                    curSlotSC.BulletID = bulletID;
                    curSDIns = InstanceBullet(bulletID, BulletInsMode.Standby);
                    curSDIns.transform.SetParent(UIManager.Instance.GroupBulletStandby.transform);
                    curSDIns.transform.position = Vector3.zero;
                    curSDIns.transform.localScale = Vector3.one;
                    curSDIns.GetComponent<RectTransform>().anchoredPosition3D =
                        tmpSlot.GetComponent<RectTransform>().anchoredPosition3D;
                    break;
                }
            }
        }
        return curSDIns;
    }
    #endregion
  
    public void BulletUpgrade()
    {
        GameObject curSD = UIManager.Instance.GroupBulletStandby;
        Dictionary<int, int> IDCount = new Dictionary<int, int>();
        List<StandbyBullet> bulletFlags = new List<StandbyBullet>();
        for (int i = 0; i < curSD.transform.childCount; i++)
        {
            GameObject curBullet = curSD.transform.GetChild(i).gameObject;
            StandbyBullet curSC = curBullet.GetComponentInChildren<StandbyBullet>();
            if (!IDCount.ContainsKey(curSC._bulletData.ID))
                IDCount.Add(curSC._bulletData.ID,1);
            else
                IDCount[curSC._bulletData.ID] += 1;
            bulletFlags.Add(curSC);
        }

        foreach (var each in IDCount)
        {
            if (each.Value == 2)
            {
                foreach (var eachSpawner in CharacterManager.Instance.CurBulletSpawners)
                {
                    if (eachSpawner.bulletID == each.Key)
                    {
                        //Upgrade
                        CharacterManager.Instance.SubStandebyBullet(eachSpawner.bulletID);
                        eachSpawner.bulletID += 100;
                    }
                }
            }
            if (each.Value == 3)
            {
                CharacterManager.Instance.SubStandebyBullet(each.Key);
                InstanceStandByBullet(each.Key+100);
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