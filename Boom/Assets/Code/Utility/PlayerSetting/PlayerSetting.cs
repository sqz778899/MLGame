#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using UnityEngine;

public static class PlayerSetting
{
    #region 分辨率设置
    public static void SetScreenResolution(int value)
    {
        TrunkManager.Instance._userConfig.UserScreenResolution = value;
        ScreenRes CurResolution = (ScreenRes)value;
        FullScreenMode currentMode = Screen.fullScreenMode;
        
        switch (CurResolution)
        {
            case ScreenRes.Set3840_2160: Screen.SetResolution(3840,2160,currentMode); break;
            case ScreenRes.Set2560_1660: Screen.SetResolution(2560, 2160, currentMode); break;
            case ScreenRes.Set2560_1440: Screen.SetResolution(2560,1440,currentMode); break;
            case ScreenRes.Set2048_1440: Screen.SetResolution(2048, 1440, currentMode); break;
            case ScreenRes.Set1920_1200: Screen.SetResolution(1920,1200,currentMode); break;
            case ScreenRes.Set1920_1080: Screen.SetResolution(1920,1080,currentMode); break;
            case ScreenRes.Set1680_1050: Screen.SetResolution(1680, 1050, currentMode); break;
            case ScreenRes.Set1600_1200: Screen.SetResolution(1600, 1200, currentMode); break;
            case ScreenRes.Set1600_900: Screen.SetResolution(1600,900,currentMode); break;
            case ScreenRes.Set1440_1080: Screen.SetResolution(1440, 1080, currentMode); break;
            case ScreenRes.Set1366_768: Screen.SetResolution(1366,768,currentMode); break;
            case ScreenRes.Set1280_960: Screen.SetResolution(1280, 960, currentMode); break;
            case ScreenRes.Set1280_800: Screen.SetResolution(1280, 800, currentMode); break;
            case ScreenRes.Set1280_720: Screen.SetResolution(1280, 720, currentMode); break;
            case ScreenRes.Set1024_768: Screen.SetResolution(1024, 720, currentMode); break;
        }
        SaveManager.SaveUserConfig();
    }
    #endregion

    #region 语言设置
    public static Dictionary<LanguageType, string> LanguageTypeDict = new()
    {
        {LanguageType.ChineseSimplified, "简体中文"},
        {LanguageType.English, "English"},
        {LanguageType.Japanese, "日本語"},
    };
    public static void SetLanguage(LanguageType CurLanguage) => Loc.Load(CurLanguage);
    #endregion
    
    #region 窗口模式设置
    //FullScreenMode.ExclusiveFullScreen;
    //FullScreenMode.MaximizedWindow;
    //FullScreenMode.Windowed;
    public static void SetScreenMode(FullScreenMode CurScreenMode) => Screen.fullScreenMode = CurScreenMode;
    public static void SetScreenMode(int value) => Screen.fullScreenMode = (FullScreenMode)value;
    #endregion
    
    public static void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}