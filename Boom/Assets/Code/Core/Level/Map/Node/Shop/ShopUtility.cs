using UnityEngine;

public static class ShopUtility
{
    //选中一颗Roll出来的子弹    外部调用所以写这里
    public static void SelOne(GameObject SelGO)
    {
        RollBase curSCBase = SelGO.GetComponent<RollBase>();
        //............Cost Money.................
        int curCost = 5;
        if (PlayerManager.Instance._PlayerData.Coins < curCost)
        {
            Debug.Log("No Money");
            return;
        }
        PlayerManager.Instance._PlayerData.ModifyCoins(-curCost);
        
        //............Deal Data.................
        switch (curSCBase.CurType)
        {
            case RollBulletMatType.Mat:
                RollBulletMat curSCM = curSCBase as RollBulletMat;
                bool isAdd = false;
                //bool isAdd = MainRoleManager.Instance.AddStandbyBulletMat(curSCM.ID);
                if (!isAdd)
                {
                    Debug.Log("没有位置了");
                    return;
                }
                UpgradeMaster.UpgradeBullets(); //子弹升级
                break;
            case RollBulletMatType.Score:
                RollScore curSCS = curSCBase as RollScore;
                PlayerManager.Instance._PlayerData.Score +=  curSCS.Score;
                break;
        }
    }

    public static bool SelOne(GemInShop SelGem)
    {
        //............Cost Money.................
        if (PlayerManager.Instance._PlayerData.Coins < SelGem.Price)
        {
            Debug.Log("No Money");
            return false;
        }
        PlayerManager.Instance._PlayerData.ModifyCoins(-SelGem.Price);
        
        //...........Buy This One................
        InventoryManager.Instance.AddGemToBag(SelGem._data.ID);
        return true;
    }

    //.获取目标槽位
    public static Vector3 GetTargetSlotPos()
    {
        SlotStandbyMat[] SDSlots = UIManager.Instance.
            CommonUI.G_StandbyIcon.GetComponentsInChildren<SlotStandbyMat>();

        foreach (var each in SDSlots)
        {
            if (each.MainID == 0)
                return each.transform.position;
        }
        return Vector3.zero;
    }
}