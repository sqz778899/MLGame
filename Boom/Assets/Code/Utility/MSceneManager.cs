using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

public class MSceneManager: ScriptableObject
{
    [SerializeField]
    public int CurrentSceneIndex;
    const int LoadingScene = 3;

    public void LoadScene(int SceneID)
    {
        CurrentSceneIndex = SceneID;
        SceneManager.LoadScene(LoadingScene);
    }
    
    #region 单例
    static MSceneManager s_instance;
    public static MSceneManager Instance
    {
        get
        {
            s_instance ??= ResManager.instance.GetAssetCache<MSceneManager>(PathConfig.MSceneManagerOBJ);
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
