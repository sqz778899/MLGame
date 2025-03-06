using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Shop:GUIBase
{
    [Header("基本资产")] 
    public TextMeshProUGUI Title;
    public TextMeshProUGUI TextRollCost;
    public GameObject RollInsRoot;
    public GameObject ShopSlotRoot;
    public GameObject BarRoot;
    public SkeletonGraphic Ani;
    
    [Header("基本属性")]
    public ShopNode CurShopNode;
    public ShopType CurShopType;
    public List<RollPR> RollProbs;
    
    const int rowOffet = 756;
    const int columnOffet = -125;

    void Update()
    {
        switch (CurShopType)
        {
            case ShopType.All:
                Title.text = "杂货商店";
                break;
            case ShopType.GemShop:
                Title.text = "宝石商店";
                break;
            case ShopType.BulletShop:
                Title.text = "子弹商店";
                break;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) { QuitSelf(); }

        if (CurShopNode.IsFirstOpen)
        {
            TextRollCost.text = "0";
        }
        else
        {
            TextRollCost.text = MainRoleManager.Instance.ShopCost.ToString();
        }
    }

    public void InitData(ShopNode _curShopNode)
    {
        CurShopNode = _curShopNode;
        CurShopType = _curShopNode.CurShopType;
        RollProbs = RollManager.Instance.DealProb(_curShopNode.RollPRs);

        if (_curShopNode.ShopIndexToGemId.Count != 0)
        {
            AniUtility.UIPlayIdle2(Ani);
            foreach (var each in _curShopNode.ShopIndexToGemId)
            {
                GameObject curRollIns = ResManager.instance.CreatInstance(PathConfig.RollGemPB);
                GemInShop curGem = curRollIns.GetComponent<GemInShop>();
                curGem._data.ID = each.Value;
                curGem.CurShopNode = _curShopNode;
                SetChildIns(curRollIns,each.Key);
            }
        }
    }

    #region Design
    //.........temp
    //90概率 Score
    //10概率 Bullet
    Vector2Int ScoreRange = new Vector2Int(3, 9);
    #endregion

    #region Roll
    public void OnceRefreshShop()
    {
        switch (CurShopType)
        {
            case ShopType.All:
                break;
            case ShopType.GemShop:
                OnceRollGem();
                break;
            case ShopType.BulletShop:
                OnceRollBullet();
                break;
        }
    }

    bool ReadyToRoll()
    {
        //Cal gold
        if (!CurShopNode.IsFirstOpen) //商店第一次打开免费
        {
            int curCost = MainRoleManager.Instance.ShopCost;
            int curGold = MainRoleManager.Instance.Coins;
            if (curGold < curCost) return false;
            MainRoleManager.Instance.Coins -= curCost;
        }
        else
            CurShopNode.IsFirstOpen = false;
        
        //Clean Ins
        int preRollIns = RollInsRoot.transform.childCount;
        for (int i = preRollIns - 1; i >= 0; i--)
            DestroyImmediate(RollInsRoot.transform.GetChild(i).gameObject);
        return true;
    }

    void SetChildIns(GameObject _child, int _index)
    {
        Transform curPivot = ShopSlotRoot.transform.GetChild(_index);
        _child.transform.SetParent(RollInsRoot.transform,false);
        _child.transform.position = curPivot.position;
    }
    
    void OnceRollGem()
    {
        //处理概率
        if(!ReadyToRoll()) return;
        
        //播放动画
        float aniTime = 0;
        AniUtility.UIPlayRoll(Ani,ref aniTime);
        StartCoroutine(RollAfterAnimation(aniTime));
    }

    IEnumerator RollAfterAnimation(float aniTime)
    {
        yield return new WaitForSeconds(aniTime);
        
        CurShopNode.ShopIndexToGemId.Clear();
        //New Ins
        for (int i = 0; i < 5; i++)
        {
            RollPR curProb = RollManager.Instance.SingleRoll(RollProbs);//抽一发
            GameObject curRollIns = ResManager.instance.CreatInstance(PathConfig.RollGemPB);
            GemInShop curGem = curRollIns.GetComponent<GemInShop>();
            curGem._data.ID = curProb.ID;
            curGem.CurShopNode = CurShopNode;
            curGem.ShopSlotIndex = i;
            CurShopNode.ShopIndexToGemId.Add(i,curGem._data.ID);
            SetChildIns(curRollIns, i);
        }
    }
    
    public void OnceRollBullet()
    {
        //处理概率
        RollProbs = RollManager.
            Instance.DealProb(MainRoleManager.Instance.CurRollPR);
        if(!ReadyToRoll()) return;
        //New Ins
        for (int i = 0; i < 5; i++)
        {
            RollPR curProb = RollManager.Instance.SingleRoll(RollProbs);
            //GetIns
            GameObject curRollIns = null;
            if (curProb.ID == 0)
            {
                curRollIns = ResManager.instance.CreatInstance(PathConfig.RollScorePB);
                RollScore curSC = curRollIns.GetComponent<RollScore>();
                int curScore = Random.Range(ScoreRange.x, ScoreRange.y+1);
                curSC.Score = curScore;
            }
            else
                curRollIns = BulletManager.Instance.InstanceRollBulletMat(curProb.ID,BulletInsMode.Mat);

            SetChildIns(curRollIns, i);
        }
    }
    #endregion

    #region Bar
    //1086 -151
    //1086 -276
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
            Vector2 curPos = new Vector2(1086 + row*rowOffet, -151 + column*columnOffet);
            
            GameObject curBar = ResManager.instance.CreatInstance(PathConfig.PRDisplayBarPB);
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
        base.QuitSelf();
        UIManager.Instance.IsLockedClick = false;
    }
}