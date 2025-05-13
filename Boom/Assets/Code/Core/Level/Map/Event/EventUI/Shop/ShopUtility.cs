using UnityEngine;

public static class ShopUtility
{
    //选中一颗Roll出来的子弹    外部调用所以写这里
    public static void SelOne(GameObject SelGO)
    {
        RollBase curSCBase = SelGO.GetComponent<RollBase>();
        //............Cost Money.................
        int curCost = 5;
        if (GM.Root.PlayerMgr._PlayerData.Coins < curCost)
        {
            Debug.Log("No Money");
            return;
        }
        GM.Root.PlayerMgr._PlayerData.ModifyCoins(-curCost);
        
        //............Deal Data.................
        switch (curSCBase.CurType)
        {
            
            case RollBulletMatType.Score:
                RollScore curSCS = curSCBase as RollScore;
                GM.Root.PlayerMgr._PlayerData.Score +=  curSCS.Score;
                break;
        }
    }

    public static bool SelOne(GemShopPreview SelGem)
    {
        //............Cost Money.................
        if (GM.Root.PlayerMgr._PlayerData.Coins < SelGem.Data.Price)
        {
            Debug.Log("No Money");
            return false;
        }
        GM.Root.PlayerMgr._PlayerData.ModifyCoins(-SelGem.Data.Price);
        
        //...........Buy This One................
        InventoryManager.Instance.AddGemToBag(SelGem.Data.ID);
        return true;
    }

    //.获取目标槽位
    public static Vector3 GetTargetSlotPos()
    {
        return Vector3.zero;
    }
}