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

    public static bool ContainByID<T>(List<T> CurList,T CurObject)where T : HaveID
    {
        bool isContain = false;
        foreach (var each in CurList)
        {
            if (each.ID == CurObject.ID)
            {
                isContain = true;
                break;
            }
        }
        return isContain;
    }

    public static void RemoveByID<T>(ref List<T> CurList, T CurObject) where T : HaveID
    {
        int Index = -1;
        for (int i = 0; i < CurList.Count; i++)
        {
            if (CurList[i].ID == CurObject.ID)
            {
                Index = i;
                break;
            }
        }

        if (Index != -1)
            CurList.RemoveAt(Index);
    }
}