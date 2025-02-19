using UnityEngine;

public static class ShopUtility
{
    //选中一颗Roll出来的子弹    外部调用所以写这里
    public static void SelOne(GameObject SelGO)
    {
        RollBase curSCBase = SelGO.GetComponent<RollBase>();
        //............Cost Money.................
        int curCost = MainRoleManager.Instance.ShopCost;
        if (MainRoleManager.Instance.Coins < curCost)
        {
            Debug.Log("No Money");
            return;
        }
        MainRoleManager.Instance.Coins -= curCost;
        
        //............Deal Data.................
        switch (curSCBase.CurType)
        {
            case RollBulletMatType.Mat:
                RollBulletMat curSCM = curSCBase as RollBulletMat;
                bool isAdd = MainRoleManager.Instance.AddStandbyBulletMat(curSCM.ID);
                if (!isAdd)
                {
                    Debug.Log("没有位置了");
                    return;
                }
                UpgradeMaster.UpgradeBullets(); //子弹升级
                break;
            case RollBulletMatType.Score:
                RollScore curSCS = curSCBase as RollScore;
                MainRoleManager.Instance.Score +=  curSCS.Score;
                break;
        }
    }

    public static bool SelOne(GemInShop SelGem)
    {
        //............Cost Money.................
        if (MainRoleManager.Instance.Coins < SelGem.Price)
        {
            Debug.Log("No Money");
            return false;
        }
        MainRoleManager.Instance.Coins -= SelGem.Price;
        
        //...........Buy This One................
        MainRoleManager.Instance.AddGem(SelGem.ID);
        return true;
    }

    //.获取目标槽位
    public static Vector3 GetTargetSlotPos()
    {
        SlotStandbyMat[] SDSlots = UIManager.Instance.
            G_StandbyIcon.GetComponentsInChildren<SlotStandbyMat>();

        foreach (var each in SDSlots)
        {
            if (each.MainID == 0)
                return each.transform.position;
        }
        return Vector3.zero;
    }
}