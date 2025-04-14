using System.Collections.Generic;
using UnityEngine;

public class ShopNode: MapNodeBase
{
    [Header("重要属性")]
    public ShopType CurShopType;
    public List<RollPR> RollPRs = new List<RollPR>();
    public int ShopCost;
    
    public bool IsFirstOpen;
    public GameObject ShopIns;
    internal override void Start()
    {
        base.Start();
        IsFirstOpen = true;
        ShopIns = ResManager.instance.CreatInstance(PathConfig.ShopAsset);
        ShopIns.transform.SetParent(UIManager.Instance.MapUI.ShopRoot.transform,false);
        ShopIns.GetComponent<Shop>().InitData(this);//建立链接
        ShopIns.GetComponent<ICloseOnClickOutside>().Hide();
    }
    
    public void EnterShop()
    {
        if (UIManager.Instance.IsLockedClick) return;
        ShopIns.GetComponent<ICloseOnClickOutside>().Show();
    }
}
