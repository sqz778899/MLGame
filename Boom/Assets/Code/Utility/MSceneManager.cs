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

    public void NewGame()
    {
        TrunkManager.Instance.SetSaveFileTemplate();
        CurrentSceneIndex = 2;
        SceneManager.LoadScene(LoadingScene);
    }
    
    public void ContinueGame()
    {
        CurrentSceneIndex = 1;
        SceneManager.LoadScene(LoadingScene);
    }

    #region PlayerSetting
    public void Setting()
    {
        UIManager.Instance.G_Setting.GetComponent<SettingMono>().OnOffWindow();
    }
    
    public void SetScreenResolution(int value)
    {
        TrunkManager.Instance._userConfig.UserScreenResolution = value;
        ScreenRes CurResolution = (ScreenRes)value;
        switch (CurResolution)
        {
            case ScreenRes.Set3840_2160:
                Screen.SetResolution(3840,2160,true);
                break;
            case ScreenRes.Set2560_1440:
                Screen.SetResolution(2560,1440,true);
                break;
            case ScreenRes.Set1920_1080:
                Screen.SetResolution(1920,1080,true);
                break;
            case ScreenRes.Set1366_768:
                Screen.SetResolution(1366,768,true);
                break;
        }
        TrunkManager.Instance.SaveUserConfig();
    }

    public void SetScreenMode(FullScreenMode CurScreenMode)
    {
        //FullScreenMode.ExclusiveFullScreen;
        //FullScreenMode.MaximizedWindow;
        //FullScreenMode.Windowed;
        Screen.fullScreenMode = CurScreenMode;
    }
    
    public void SetScreenMode(int value)
    {
        TrunkManager.Instance._userConfig.UserScreenMode = value;
        Screen.fullScreenMode = (FullScreenMode)value;
        TrunkManager.Instance.SaveUserConfig();
    }
    #endregion
    
    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
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
