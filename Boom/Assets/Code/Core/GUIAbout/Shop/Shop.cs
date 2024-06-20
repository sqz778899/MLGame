using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Shop:GUIBase
{
    public GameObject RollInsRoot;
    public GameObject ShopSlotRoot;
    public ShopNode CurShopNode;
    #region Design
    //.........temp
    //90概率 Score
    //10概率 Bullet
    Vector2Int ScoreRange = new Vector2Int(3, 9);
    int Cost = 5;
    #endregion

    #region Roll
    public void OnceRollBullet()
    {
        //GetProbabilitys
        List<RollProbability> rollProbs = TrunkManager.Instance.GetRollProbability();
        RollManager.Instance.DealProb(ref rollProbs);
        
        //Cal gold
        int curCost = MainRoleManager.Instance.Cost;
        int curGold = MainRoleManager.Instance.Gold;
        if (curGold < curCost)
            return;

        MainRoleManager.Instance.Gold -= curCost;
        //Clean Ins
        int preRollIns = RollInsRoot.transform.childCount;
        for (int i = preRollIns - 1; i >= 0; i--)
            DestroyImmediate(RollInsRoot.transform.GetChild(i).gameObject);
        
        //New Ins
        for (int i = 0; i < 5; i++)
        {
            RollProbability curProb = RollManager.Instance.SingleRoll(rollProbs);
            
            if (curProb == null)
            {
                Debug.LogError("Roll Errro");
                return;
            }

            //GetIns
            GameObject curRollIns = null;
            if (curProb.ID == 0)
                curRollIns = ResManager.instance.IntanceAsset(PathConfig.RollScorePB);
            else
                curRollIns = BulletManager.Instance.InstanceBullet(curProb.ID,BulletInsMode.Roll);

            Transform curPivot = ShopSlotRoot.transform.GetChild(i);
            curRollIns.transform.SetParent(RollInsRoot.transform,false);
            curRollIns.transform.position = curPivot.position;
            RollBullet curSC = curRollIns.GetComponentInChildren<RollBullet>();
            int curScore = Random.Range(ScoreRange.x, ScoreRange.y+1);
            curSC.Score = curScore;
            curSC.Cost = Cost;
            
            //SetRollInsAttri(curRollIns, i);
        }
        
        TrunkManager.Instance.SaveFile();
    }
    #endregion

    public override void QuitSelf()
    {
        CurShopNode.QuitShop();
        base.QuitSelf();
    }
}