using System.Collections.Generic;
using UnityEngine;

public class ShopNode: MapNodeBase
{
    //Shop
    public void EnterShop()
    {
        Debug.Log("Shop !!");
        UIManager.Instance.SetOtherUIPause();
        GameObject ShopIns = Instantiate(ResManager.instance.GetAssetCache<GameObject>(PathConfig.ShopAsset));
        ShopIns.transform.SetParent(UIManager.Instance.ShopRoot.transform,false);
        //建立链接
        Shop curShopSC = ShopIns.GetComponent<Shop>();
        curShopSC.CurShopNode = this;
    }
}
