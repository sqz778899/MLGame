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
    public int ShopCost = 5;
    public ShopNode CurShopNode;
    public ShopType CurShopType;
    public List<RollPR> RollProbs;
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
            TextRollCost.text = "0";
        else
            TextRollCost.text = ShopCost.ToString();
    }

    public void InitData(ShopNode _curShopNode)
    {
        CurShopNode = _curShopNode;
        CurShopType = _curShopNode.CurShopType;
        ShopCost = _curShopNode.ShopCost;
        RollProbs = RollManager.Instance.DealProb(_curShopNode.RollPRs);
    }

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
                break;
        }
    }

    bool ReadyToRoll()
    {
        //Cal gold
        if (!CurShopNode.IsFirstOpen) //商店第一次打开免费
        {
            int curGold = PlayerManager.Instance._PlayerData.Coins;
            if (curGold < ShopCost) return false;
            PlayerManager.Instance._PlayerData.ModifyCoins(-ShopCost);
        }
        else
            CurShopNode.IsFirstOpen = false;
        
        //Clean Ins
        int preRollIns = RollInsRoot.transform.childCount;
        for (int i = preRollIns - 1; i >= 0; i--)
            Destroy(RollInsRoot.transform.GetChild(i).gameObject);
        return true;
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
        
        for (int i = 0; i < 5; i++)
        {
            //抽一发
            RollPR curProb = RollManager.Instance.SingleRoll(RollProbs);
            //实例化商店宝石
            GemData tempData = new GemData(curProb.ID, null);
            GameObject curShopGem = BagItemTools<GemInShop>.CreateTempObjectGO(tempData,CreateItemType.ShopGem);
            GemInShop curGem = curShopGem.GetComponent<GemInShop>();
            curGem.CurShopNode = CurShopNode;
            Transform curPivot = ShopSlotRoot.transform.GetChild(i);
            curShopGem.transform.SetParent(RollInsRoot.transform,false);
            curShopGem.transform.position = curPivot.position;
        }
    }
    #endregion

    #region Bar
    //1086 -151
    //1086 -276
    /*void GetCurPRBarDisplay()
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
    }*/
    

    #endregion
    
    public override void QuitSelf() => gameObject.SetActive(false);
}