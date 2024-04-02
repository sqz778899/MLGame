using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CalculateDamageManager : ScriptableObject
{
    #region 单例
    static CalculateDamageManager s_instance;
    
    public static CalculateDamageManager Instance
    {
        get
        {
            if (s_instance == null)
                s_instance = ResManager.instance.GetAssetCache<CalculateDamageManager>(PathConfig.CalculateDamageManagerOBJ);
            return s_instance;
        }
    }
    #endregion
}
