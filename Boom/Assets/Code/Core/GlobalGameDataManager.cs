using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class GlobalGameDataManager: ScriptableObject
{
    #region 单例
    static GlobalGameDataManager s_instance;
    
    public static GlobalGameDataManager Instance
    {
        get
        {
            if (s_instance == null)
                s_instance = ResManager.instance.GetAssetCache<GlobalGameDataManager>(PathConfig.GLGameDataManagerOBJ);
            return s_instance;
        }
    }
    #endregion


    #region 可以升级的Buff之类的参数
    
    public float openFogRadiusAdd = 0; 
    public float openFogFadeRangeAdd = 0; 
    
    #endregion
    
    public NodeOpenFog LockedPara = new NodeOpenFog
    {
        openFogRadius = 5.17f,
        openFogFadeRange = 4.19f
    };
    
    const float openFogRadius = 16.2f;
    const float openFogFadeRange = 30.6f;
    
    public NodeOpenFog GetUnlockedPara()
    {
        NodeOpenFog UnlockedPara = new NodeOpenFog
        {
            openFogRadius = openFogRadius + openFogRadiusAdd,
            openFogFadeRange = openFogFadeRange + openFogFadeRangeAdd
        };
        return UnlockedPara;
    }
    
}