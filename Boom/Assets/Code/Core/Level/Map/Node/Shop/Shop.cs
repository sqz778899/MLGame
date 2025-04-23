using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using TMPro;
using UnityEngine;

public class Shop:MonoBehaviour,ICloseOnClickOutside
{
    [Header("基本资产")] 
    public TextMeshProUGUI Title;
    public TextMeshProUGUI TextRollCost;
    public GameObject RollInsRoot;
    public List<ShopSlotView> ShopSlots;
    public SkeletonGraphic Ani;

    [Header("基本属性")] 
    public bool IsFirstOpen;
    public int ShopCost = 5;
    public ShopType CurShopType;
    public List<RollPR> RollProbs;
    
    [Header("UI 面板根节点（点击内部）")]
    public RectTransform ClickRoot;
    public RectTransform ClickArea => ClickRoot;

    
    void Start()
    {
        //注册快捷键
        GM.Root.HotkeyMgr.OnEscapePressed += Hide;
        //初始化槽位Controllar
        ShopSlots.ForEach(s=>s.Init());
    }

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

        if (IsFirstOpen)
            TextRollCost.text = "0";
        else
            TextRollCost.text = ShopCost.ToString();
    }
    
    public void InitData(ShopEventRuntimeData runtimeData)
    {
        // 适配新地图事件系统
        CurShopType = runtimeData.ShopType;
        ShopCost = runtimeData.ShopCost;
        RollProbs = RollManager.Instance.DealProb(runtimeData.RollPRs);
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
        if (!IsFirstOpen) //商店第一次打开免费
        {
            int curGold = PlayerManager.Instance._PlayerData.Coins;
            if (curGold < ShopCost) return false;
            PlayerManager.Instance._PlayerData.ModifyCoins(-ShopCost);
        }
        else
            IsFirstOpen = false;
        
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
        
        for (int i = 0; i < ShopSlots.Count; i++)
        {
            //抽一发
            RollPR curProb = RollManager.Instance.SingleRoll(RollProbs);
            //实例化商店宝石
            GemData tempData = new GemData(curProb.ID, null);
            GameObject curShopGem = BagItemTools<GemShopPreview>.CreateTempObjectGO(tempData, CreateItemType.ShopGem);
            ShopSlots[i].Controller.Assign(tempData, curShopGem);
        }
    }
    #endregion

    #region Hide/Show
    public void Show()
    {
        gameObject.SetActive(true);
        UIClickOutsideManager.Register(this);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        UIClickOutsideManager.Unregister(this);
    }
    public void OnClickOutside() => Hide();
    
    void OnDestroy() => GM.Root.HotkeyMgr.OnEscapePressed -= Hide; //注册快捷键
    #endregion
}