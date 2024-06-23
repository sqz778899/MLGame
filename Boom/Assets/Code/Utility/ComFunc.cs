using System.Collections.Generic;
using UnityEngine;

public static class ComFunc
{
    public static T GetSingle<T>(List<T> CurList,int CurID) where T : HaveID
    {
        T curResult = null;
        foreach (var each in CurList)
        {
            if (each.ID == CurID)
            {
                curResult = each;
                break;
            }
        }
        return curResult;
    }
}