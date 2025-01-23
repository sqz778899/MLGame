using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GemManager
{
    #region 重要功能
    public static void AddGem(int GemID)
    {
        
    }
    
    public static void DeleteGem(GameObject GemIns)
    {
        GameObject.DestroyImmediate(GemIns);
        MainRoleManager.Instance.RefreshAllItems();
        TrunkManager.Instance.SaveFile();
    }
    #endregion
    
    #region 私有方法
    static void InitGemIns(int GemID,ref GameObject gemIns)
    {
        //实例化GO
        gemIns = ResManager.instance.CreatInstance(PathConfig.GemTemplate);
        gemIns.transform.SetParent(UIManager.Instance.ItemRoot.transform,false);
        Gem curGemSC = gemIns.GetComponent<Gem>();
        curGemSC.ID = GemID;
        curGemSC.InstanceID = gemIns.GetInstanceID();
        curGemSC.SyncData();
    }
    
    static GemJson GetGemJsonByID(int GemID)
    {
        GemJson curGemJson = null;
        List<GemJson> itemDesignJsons = TrunkManager.Instance.GemDesignJsons;
        foreach (var each in itemDesignJsons)
        {
            if (each.ID == GemID)
            {
                curGemJson = each;
                break;
            }
        }
        return curGemJson;
    }
    #endregion
}
