using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class BuffMannager:ScriptableObject
{
    #region 单例
    static BuffMannager s_instance;
    
    public static BuffMannager Instance
    {
        get
        {
            if (s_instance == null)
                s_instance = ResManager.instance.GetAssetCache<BuffMannager>(PathConfig.BuffMannagerOBJ);
            return s_instance;
        }
    }
    #endregion

    #region GetInfo
    public BuffDataJson GetBuffDataByID(int ID)
    {
        BuffDataJson curData = null;
        foreach (BuffDataJson each in TrunkManager.Instance.BuffDesignJsons)
        {
            if (each.ID == ID)
            {
                curData = each;
                break;
            }
        }
        return curData;
    }

    public Sprite GetBuffImageByID(int ID)
    {
        BuffDataJson curData = GetBuffDataByID(ID);

        if (curData == null)
            return null;

        Sprite curSprite = ResManager.instance.GetAssetCache<Sprite>(
            PathConfig.GetBufftImagePath(curData.ID, curData.name));
        return curSprite;
    }
    #endregion
}