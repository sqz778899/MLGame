using System.Collections;
using System.Collections.Generic;
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
    
    void Start()
    {
        MultiLaDP.value = (int)MultiLa.Instance.CurLanguage;
        ScreenResolutionDP.value = TrunkManager.Instance._userConfig.UserScreenResolution;
        ScreenMode(TrunkManager.Instance._userConfig.UserScreenMode);
        CloseSetting();
    }

    public void CloseSetting()
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
    }
    
    
    public void OnOffSetting()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject curGO = transform.GetChild(i).gameObject;
            curGO.SetActive(!curGO.activeSelf);
        }
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
}
