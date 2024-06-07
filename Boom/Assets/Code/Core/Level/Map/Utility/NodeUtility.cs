using UnityEngine;

public static class NodeUtility
{
    public static GameObject InstanceMapNode(MapNodeType mdType)
    {
        GameObject MapNodeIns = null;
        switch (mdType)
        {
            case MapNodeType.Event:
                MapNodeIns = ResManager.instance.IntanceAsset(PathConfig.MapNodeEvent);
                break;
            case MapNodeType.Shop:
                MapNodeIns = ResManager.instance.IntanceAsset(PathConfig.MapShop);
                break;
            case MapNodeType.GoldPile:
                MapNodeIns = ResManager.instance.IntanceAsset(PathConfig.MapGoldPile);
                break;
            case MapNodeType.TreasureBox:
                MapNodeIns = ResManager.instance.IntanceAsset(PathConfig.MapTreasureBox);
                break;
        }
        return MapNodeIns;
    }
}