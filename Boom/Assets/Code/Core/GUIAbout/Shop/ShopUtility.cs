using UnityEngine;

public static class ShopUtility
{
    //选中一颗Roll出来的子弹    外部调用所以写这里
    public static void SelOne(GameObject SelGO)
    {
        RollBase curSCBase = SelGO.GetComponent<RollBase>();
        //............Cost Money.................
        int curCost = MainRoleManager.Instance.ShopCost;
        if (MainRoleManager.Instance.Gold < curCost)
        {
            Debug.Log("No Money");
            return;
        }
        MainRoleManager.Instance.Gold -= curCost;
        
        //............Deal Data.................
        switch (curSCBase.CurType)
        {
            case RollBulletMatType.Mat:
                RollBulletMat curSCM = curSCBase as RollBulletMat;
                bool isAdd = MainRoleManager.Instance.AddStandbyBullet(curSCM.ID);
                if (!isAdd)
                {
                    Debug.Log("没有位置了");
                    return;
                }
                break;
            case RollBulletMatType.Score:
                RollScore curSCS = curSCBase as RollScore;
                MainRoleManager.Instance.Score +=  curSCS.Score;
                break;
        }

        //BulletManager.Instance.BulletUpgrade();
        TrunkManager.Instance.SaveFile();
    }
}