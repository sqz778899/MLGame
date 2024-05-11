using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

public class MSceneManager: ScriptableObject
{
    #region 单例
    static MSceneManager s_instance;
    
    public static MSceneManager Instance
    {
        get
        {
            if (s_instance == null)
                s_instance = ResManager.instance.GetAssetCache<MSceneManager>(PathConfig.MSceneManagerOBJ);
            return s_instance;
        }
    }
    #endregion

    public MapSate CurMapSate = new MapSate();
    public int CurrentSceneIndex;
    

    public void WinThisLevel()
    {
        CurMapSate.IsFinishedLevels.Add(CurMapSate.LevelID);
    }
    
    public void LoadScene(int SceneID)
    {
        CurrentSceneIndex = SceneID;
        SceneManager.LoadScene(CurrentSceneIndex);
        TrunkManager.Instance.SaveFile();
    }

    public void NewGame()
    {
        TrunkManager.Instance.SetSaveFileTemplate();
        CurrentSceneIndex = 1;
        SceneManager.LoadScene(CurrentSceneIndex);
    }

    public void ContinueGame()
    {
        CurrentSceneIndex = 1;
        SceneManager.LoadScene(CurrentSceneIndex);
    }

    #region Help
    public void Help()
    {
        UIManager.Instance.G_Help.GetComponent<HelpMono>().OnOffWindow();
    }
    #endregion

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
}
