using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ShopNode: MapNodeBase
{
    [Header("重要属性")]
    public ShopType CurShopType;
    public List<RollPR> RollPRs = new List<RollPR>();
    public Dictionary<int, int> ShopIndexToGemId =
        new Dictionary<int, int>();//为了关闭商店再打开，商店物品依然还原，构建了一个存商店物品的地方
    public bool IsFirstOpen;
    
    internal override void Start()
    {
        base.Start();
        IsFirstOpen = true;
    }
    
    public void EnterShop()
    {
        UIManager.Instance.IsLockedClick = true;
        GameObject ShopIns = ResManager.instance.CreatInstance(PathConfig.ShopAsset);
        ShopIns.transform.SetParent(UIManager.Instance.ShopRoot.transform,false);
        //建立链接
        Shop curShopSC = ShopIns.GetComponent<Shop>();
        curShopSC.InitData(this);
    }
}
