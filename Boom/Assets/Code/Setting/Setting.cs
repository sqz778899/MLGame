using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;

public class Setting:MonoBehaviour
{
    [Header("依赖资产")]
    [SerializeField]GameObject MainMenu;
    CustomButton[] _mainmenuButtons;
    CustomButton[] _settingMenuButtons;
    
    [Header("设置菜单")]
    [SerializeField]GameObject SettingMenu;
    [SerializeField]GameObject SettingPannel;
    [SerializeField]float SettingPannelOffset;
    
    [Header("显示相关")]
    [SerializeField]GameObject DisplayChildMenu;
    [SerializeField]GameObject ResolutionChildMenu;
    [SerializeField]GameObject selBoxFullScreenCheckmark;
    [SerializeField]GameObject selBoxBorderlessCheckmark;
    [SerializeField]GameObject selBoxWindowedCheckmark;
    
    [Header("语言相关")]
    [SerializeField]TextMeshProUGUI txtCurLanguage;
    [SerializeField]GameObject LanChildMenu;
    [SerializeField]GameObject SelLanChildMenu;
    void Start() => SetBoxCheckmark(Screen.fullScreenMode);

    public void OpenMainMenu()
    {
        Show();
        _mainmenuButtons.ForEach(c => c.Reset());
    }

    public void OpenSettingMenu()
    {
        MainMenu.SetActive(false);
        SettingMenu.SetActive(true);
        _settingMenuButtons.ForEach(c => c.Reset());
    }
    
    public void Continue() => Hide();
    
    
    #region 显示设置
    public void ShowDisplayChildMenu()
    {
        CloseChildMenu();
        OffsetSettingPannel();
        DisplayChildMenu.SetActive(!DisplayChildMenu.activeSelf);
    }

    public void ShowResolutionChildMenu() 
        => ResolutionChildMenu.SetActive(!ResolutionChildMenu.activeSelf);

    //分辨率设置
    public void SetScreenResolution(int value) => PlayerSetting.SetScreenResolution(value);

    //窗口模式设置
    public void SetScreenMode(int value)
    {
        FullScreenMode CurScreenMode = (FullScreenMode)value;
        SetBoxCheckmark(CurScreenMode);
        PlayerSetting.SetScreenMode(CurScreenMode);
    }
    
    void SetBoxCheckmark(FullScreenMode CurScreenMode)
    {
        switch (CurScreenMode)
        {
            case FullScreenMode.ExclusiveFullScreen:
                selBoxFullScreenCheckmark.SetActive(true);
                selBoxBorderlessCheckmark.SetActive(false);
                selBoxWindowedCheckmark.SetActive(false);
                break;
            case FullScreenMode.MaximizedWindow:
                selBoxFullScreenCheckmark.SetActive(false);
                selBoxBorderlessCheckmark.SetActive(true);
                selBoxWindowedCheckmark.SetActive(false);
                break;
            case FullScreenMode.Windowed:
                selBoxFullScreenCheckmark.SetActive(false);
                selBoxBorderlessCheckmark.SetActive(false);
                selBoxWindowedCheckmark.SetActive(true);
                break;
        }
    }
    #endregion

    #region 语言设置
    public void ShowLanguageChildMenu()
    {
        CloseChildMenu();
        OffsetSettingPannel();
        LanChildMenu.SetActive(!LanChildMenu.activeSelf);
    }

    public void ShowSelLanguageChildMenu() =>
        SelLanChildMenu.SetActive(!SelLanChildMenu.activeSelf);

    public void SetLanguage(int value)
    {
        LanguageType CurLanguage = (LanguageType)value;
        txtCurLanguage.text = PlayerSetting.LanguageTypeDict[CurLanguage];
        PlayerSetting.SetLanguage(CurLanguage);
    }
    #endregion
    
    #region 点击外部关闭的接口
    void Show()
    {
        MainMenu.SetActive(true);
    }
    void Hide()
    {
        CloseSettingMenu();
        MainMenu.SetActive(false);
    }
    
    public void CloseSettingMenu()
    {
        ResetSettingPannel();
        MainMenu.SetActive(true);
        SettingMenu.SetActive(false);
        CloseChildMenu();
        _mainmenuButtons.ForEach(c => c.Reset());
    }
    
    //关闭设置页面的子菜单
    void CloseChildMenu()
    {
        DisplayChildMenu.SetActive(false);
        ResolutionChildMenu.SetActive(false);
        LanChildMenu.SetActive(false);
        SelLanChildMenu.SetActive(false);
    }
    
    void OffsetSettingPannel()=>
        SettingPannel.GetComponent<RectTransform>().anchoredPosition = new Vector2(-SettingPannelOffset, 0);
    
    void ResetSettingPannel() =>
        SettingPannel.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    
    public void OnClickOutside() => Hide();
    #endregion

    #region 数据初始化相关
    public void InitData()
    {
        _mainmenuButtons = MainMenu.GetComponentsInChildren<CustomButton>();
        _settingMenuButtons = SettingMenu.GetComponentsInChildren<CustomButton>();
        GM.Root.HotkeyMgr.OnEscapePressed -= Hide;
        GM.Root.HotkeyMgr.OnEscapePressed += Hide;//注册快捷键
    }
    
    void OnDestroy() => GM.Root.HotkeyMgr.OnEscapePressed -= Hide;
    #endregion
}