using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : ScriptableObject
{
    //Global Control
    public bool IsLockedClick = false;
    
    public GUICommonManager CommonUI { get; private set; }
    public GUIBagManager BagUI { get; private set; }
    public GUIMapManager MapUI { get; private set; }
    public LogicManager Logic { get; private set; }  //只有在LevelScene中才会用到
    
    public void InitStartGame()
    {
        IsLockedClick = false;
        CommonUI = new GUICommonManager();
        BagUI = new GUIBagManager();
        MapUI = new GUIMapManager();
        Logic = new LogicManager();
    }
    
    public void InitLogic()
    {
        Logic??=new LogicManager();
        Logic.InitData();
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
