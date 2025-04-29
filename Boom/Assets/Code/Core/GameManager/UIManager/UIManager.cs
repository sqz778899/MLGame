using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : ScriptableObject
{
    //Global Control
    public bool IsLockedClick = false;
    
    public void InitStartGame()
    {
        IsLockedClick = false;
    }
    
    #region 单例
    static UIManager s_instance;
    public static UIManager Instance
    {
        get
        {
            s_instance??= ResManager.instance.GetAssetCache<UIManager>(PathConfig.UIManagerOBJ);
            if (s_instance != null) { DontDestroyOnLoad(s_instance); }
            return s_instance;
        }
    }
    
    void OnEnable()
    {
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(this);
        }
    }
    #endregion
}
