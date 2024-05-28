using UnityEngine;

public class ShopNode: MapNodeBase
{
    //Shop
    public void EnterShop()
    {
        RollManager.Instance.OnOffShop();
        Debug.Log("Shop !!");
    }
}
