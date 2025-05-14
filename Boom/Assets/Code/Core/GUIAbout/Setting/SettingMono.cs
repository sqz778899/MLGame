using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingMono : MonoBehaviour
{
    public TMP_Dropdown MultiLaDP;
    public TMP_Dropdown ScreenResolutionDP;

    public Toggle FullScreen;
    public Toggle MaximizedWindow;
    public Toggle Windowed;

    [Header("依赖资产")] 
    public TMP_Dropdown multiLaDP;
    

    void Start()
    {
        MultiLaDP.value = (int)MultiLa.Instance.CurLanguage;
        ScreenResolutionDP.value = TrunkManager.Instance._userConfig.UserScreenResolution;
        ScreenMode(TrunkManager.Instance._userConfig.UserScreenMode);
        
        multiLaDP.onValueChanged.AddListener(SwichLanguage);
    }

    public void ScreenMode(int value)
    {
        if (value == 1)
        {
            FullScreen.SetIsOnWithoutNotify(true);

            MaximizedWindow.SetIsOnWithoutNotify(false);
            Windowed.SetIsOnWithoutNotify(false);
            MSceneManager.Instance.SetScreenMode(FullScreenMode.FullScreenWindow);
        }

        if (value == 2)
        {
            MaximizedWindow.SetIsOnWithoutNotify(true);

            FullScreen.SetIsOnWithoutNotify(false);
            Windowed.SetIsOnWithoutNotify(false);
            MSceneManager.Instance.SetScreenMode(FullScreenMode.MaximizedWindow);
        }

        if (value == 3)
        {
            Windowed.SetIsOnWithoutNotify(true);

            FullScreen.SetIsOnWithoutNotify(false);
            MaximizedWindow.SetIsOnWithoutNotify(false);
            MSceneManager.Instance.SetScreenMode(FullScreenMode.Windowed);
        }
    }


    //切换多语言
    public void SwichLanguage(int value) => Loc.Load((LanguageType)value);
}