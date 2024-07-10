using System.Collections.Generic;
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

    public static List<Vector3> CreateLayoutPoints(float radius,float step,float Z)
    {
        List<Vector3> LayoutPoints = new List<Vector3>();
        float startY = radius;
        float startX = radius;

        int Count = (int)(radius * 2 / step) + 1;
        for (int i = 0; i < Count; i++)
        {
            for (int j = 0; j < Count; j++)
            {
                float x = startX - step * i;
                float y = startY - step * j;
                if (CheckIden(x, y,radius))
                    LayoutPoints.Add(new Vector3(x, y,Z));
            }
        }
        return LayoutPoints;
    }

    public static void ExcludePointsPool(ref List<Vector3> LayoutPoints, Collider ColExclude)
    {
        for (int i = LayoutPoints.Count - 1; i >= 0; i--)
        {
            if (ColExclude.bounds.Contains(LayoutPoints[i]))
                LayoutPoints.RemoveAt(i);
        }
    }
    
    static bool CheckIden(float x, float y,float radius)
    {
        bool s = false;
        if ((x * x + y * y) <= radius*radius)
            s = true;
        return s;
    }
}