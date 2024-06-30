using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class TalentMannager:ScriptableObject
{
    #region 单例
    static TalentMannager s_instance;
    
    public static TalentMannager Instance
    {
        get
        {
            if (s_instance == null)
                s_instance = ResManager.instance.GetAssetCache<TalentMannager>(PathConfig.BuffMannagerOBJ);
            return s_instance;
        }
    }
    #endregion

    #region GetInfo
    public TalentDataJson GetBuffDataByID(int ID)
    {
        TalentDataJson curData = null;
        foreach (TalentDataJson each in TrunkManager.Instance.BuffDesignJsons)
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
        TalentDataJson curData = GetBuffDataByID(ID);

        if (curData == null)
            return null;

        Sprite curSprite = ResManager.instance.GetAssetCache<Sprite>(
            PathConfig.GetBufftImagePath(curData.ID, curData.name));
        return curSprite;
    }
    #endregion
}