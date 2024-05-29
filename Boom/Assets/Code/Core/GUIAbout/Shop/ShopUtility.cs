using UnityEngine;

public static class ShopUtility
{
    //选中一颗Roll出来的子弹    外部调用所以写这里
    public static void SelOne(GameObject SelGO)
    {
        RollBullet curSC = SelGO.GetComponentInChildren<RollBullet>();
        //............Cost Money.................
        int curCost = curSC.Cost;
        if (CharacterManager.Instance.Gold < curCost)
        {
            Debug.Log("No Money");
            return;
        }
        CharacterManager.Instance.Gold -= curCost;
        
        //............Deal Data.................
        if (curSC._bulletData.ID == 0)//Score
        {
            CharacterManager.Instance.Score +=  curSC.Score;
        }
        else
        {
            bool isAdd = CharacterManager.Instance.AddStandbyBullet(curSC._bulletData.ID,curSC.InstanceID);
            if (!isAdd)
            {
                Debug.Log("qweqwesxas");
                return;
            }

            BulletManager.Instance.BulletUpgrade();
        }
        TrunkManager.Instance.SaveFile();
    }
}