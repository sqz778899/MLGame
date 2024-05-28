using UnityEngine;

public class ShopNode: MapNodeBase
{
    //Shop
    public void EnterShop()
    {
        Debug.Log("Shop !!");
        GameObject GO = Instantiate(ResManager.instance.GetAssetCache<GameObject>(PathConfig.ShopAsset));
        GO.transform.SetParent(UIManager.Instance.CanvasShop.transform,false);
        Debug.Log("Shop !!");
    }
}
