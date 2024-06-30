using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Shop:GUIBase
{
    public GameObject RollInsRoot;
    public GameObject ShopSlotRoot;
    public GameObject BarRoot;
    public ShopNode CurShopNode;

    const int rowOffet = 756;
    const int columnOffet = -108;
    void Start()
    {
        GetCurPRBarDisplay();
    }
    
    #region Design
    //.........temp
    //90概率 Score
    //10概率 Bullet
    Vector2Int ScoreRange = new Vector2Int(3, 9);
    #endregion

    #region Roll
    public void OnceRollBullet()
    {
        //GetProbabilitys
        List<RollPR> rollProbs = RollManager.
            Instance.DealProb(MainRoleManager.Instance.CurRollPR);
        
        //Cal gold
        int curCost = MainRoleManager.Instance.ShopCost;
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
            RollPR curProb = RollManager.Instance.SingleRoll(rollProbs);
            
            if (curProb == null)
            {
                Debug.LogError("Roll Errro");
                return;
            }

            //GetIns
            GameObject curRollIns = null;
            if (curProb.ID == 0)
            {
                curRollIns = ResManager.instance.IntanceAsset(PathConfig.RollScorePB);
                RollScore curSC = curRollIns.GetComponent<RollScore>();
                int curScore = Random.Range(ScoreRange.x, ScoreRange.y+1);
                curSC.Score = curScore;
            }
            else
                curRollIns = BulletManager.Instance.InstanceRollBulletMat(curProb.ID,BulletInsMode.Mat);

            Transform curPivot = ShopSlotRoot.transform.GetChild(i);
            curRollIns.transform.SetParent(RollInsRoot.transform,false);
            curRollIns.transform.position = curPivot.position;
        }
        
        TrunkManager.Instance.SaveFile();
    }
    #endregion

    #region Bar
    //-1118 -200 //-362 -200
    //-1118 -308
    void GetCurPRBarDisplay()
    {
        List<RollPR> CurRollPR = MainRoleManager.Instance.CurRollPR;
        int column = 0;
        int row = 0;
        for (int i = 0; i < CurRollPR.Count; i++)
        {
            if (i % 4 == 0 && i!=0)
            {
                row++;
                column = 0;
            }
            Vector2 curPos = new Vector2(-1118 + row*rowOffet, -200 + column*columnOffet);
            
            GameObject curBar = ResManager.instance.IntanceAsset(PathConfig.PRDisplayBarPB);
            curBar.transform.SetParent(BarRoot.transform,false);
            curBar.transform.GetComponent<RectTransform>().anchoredPosition = curPos;
            PRDisplayBar curBarSC = curBar.GetComponent<PRDisplayBar>();
            curBarSC.ID = CurRollPR[i].ID;
            curBarSC.InitDataByID();
            column++;
        }
    }
    

    #endregion
    public override void QuitSelf()
    {
        CurShopNode.QuitNode();
        base.QuitSelf();
    }
}