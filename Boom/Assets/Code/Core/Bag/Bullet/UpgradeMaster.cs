using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class UpgradeMaster
{
    public static List<int> PreBulletSpawners;
    

    public static void UpgradeBullets()
    {
        //先记录一下之前的子弹状态
        PreBulletSpawners = new List<int>();
        foreach (var each in InventoryManager.Instance._BulletInvData.BagBulletSpawners)
        {
            PreBulletSpawners.Add(each.ID);
        }

        if (IsUpgrade())
        {
            //................弹出升级窗口.............
            GameObject UIIns = ResManager.instance.CreatInstance(PathConfig.BulletUPPB);
            BulletUPMono curUISC = UIIns.GetComponent<BulletUPMono>();
            int bulletID = FindNeedUpgradeBullet();
            curUISC.InitData(bulletID);
            UIIns.transform.SetParent(EternalCavans.Instance.RewardRoot.transform,false);
        }
    }

    static int FindNeedUpgradeBullet()
    {
        int bulletID = -1;
        for (int i = 0; i < PreBulletSpawners.Count; i++)
        {
            if (InventoryManager.Instance._BulletInvData.BagBulletSpawners[i].ID != PreBulletSpawners[i])
            {
                bulletID = PreBulletSpawners[i];
                break;
            }
        }
        return bulletID;
    }
    
    
    static bool IsUpgrade()
    {
        bool _isUpgrade = false;
        //.................检查一下是否有子弹需要升级...............
        /*Dictionary<int, int> IDCount = new Dictionary<int, int>();
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
                    if (eachSpawner.ID == each.Key)
                    {
                        _isUpgrade = true;
                        //Upgrade
                        //MainRoleManager.Instance.SubStandebyBullet(each.Key);//DelAll
                        foreach (var eachBullet in MainRoleManager.Instance.CurBullets)
                        {
                            if (eachBullet.ID == eachSpawner.ID)
                                eachBullet.ID += 100;
                        }
                        eachSpawner.ID += 100;
                    }
                }
            }
            if (each.Value == 3)
            {
                //MainRoleManager.Instance.SubStandebyBullet(each.Key);//DelAll
                //MainRoleManager.Instance.AddStandbyBulletMat(each.Key+100);
                _isUpgrade = IsUpgrade();
            }
        }*/
        
        Debug.Log("Upgrade !!!!");
        return _isUpgrade;
    }
}