using System.Collections.Generic;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;

public class WonderWorkshop : MonoBehaviour,ICloseOnClickOutside
{
    [Header("基本资产")] 
    [SerializeField] GameObject BaseMenu;
    [SerializeField] GameObject UpgradeBullet;
    [SerializeField] UpgradeUIDisplay UpgradeUIDisplaySC;
    [SerializeField] UpgradeSuccessPopUI UpgradeSuccessPopUISC;

    [Header("UpgradeButton")] 
    [SerializeField] GameObject btnUpClay;
    [SerializeField] GameObject btnUpIce;
    [SerializeField] GameObject btnUpFire;
    [SerializeField] GameObject btnUpThunder;
    
    CustomButton[] BaseMenuButtons;
    CustomButton[] UpgradeBulletButtons;
    InventoryManager _inventoryMgr => GM.Root.InventoryMgr;
    Dictionary<GameObject, UpgradeBulletInfo> _upgradeBulletInfoMap = new();//存储升级按钮和对应的子弹信息
    
    [Header("UI 通用交互相关")]
    [SerializeField] RectTransform ClickRoot;
    [SerializeField] UIPopAnimator _UIPopAnimator;
    
    public RectTransform ClickArea => ClickRoot;

    [Header("和地图事件相关")] 
    public MapNodeController CurWorkshopController;

    public void SelUpgradeBullet()
    {
        GetNeedUpgradeBulletInfo(); //获取需要升级的子弹信息
        UpgradeBulletButtons.ForEach(b=>b.Reset());
        BaseMenu.SetActive(false);
        UpgradeBullet.SetActive(true);
    }
    
    public void ReturnBaseMenu()
    {
        BaseMenuButtons.ForEach(b=>b.Reset());
        BaseMenu.SetActive(true);
        UpgradeBullet.SetActive(false);
    }

    //获取需要升级的子弹信息
    void GetNeedUpgradeBulletInfo()
    {
        _upgradeBulletInfoMap[btnUpClay] = _inventoryMgr.GetBulletClayInfo();
        _upgradeBulletInfoMap[btnUpIce] = _inventoryMgr.GetBulletIceInfo();
        _upgradeBulletInfoMap[btnUpFire] = _inventoryMgr.GetBulletFireInfo();
        _upgradeBulletInfoMap[btnUpThunder] = _inventoryMgr.GetBulletThunderInfo();
        foreach (var each in _upgradeBulletInfoMap)
            SetupUpgradeButton(each.Key, each.Value);
        //Show UpgradeUIDisplay
    }
    
    void SetupUpgradeButton(GameObject btnObj, UpgradeBulletInfo info)
    {
        CustomButton button = btnObj.GetComponent<CustomButton>();
        TextMeshProUGUI label = btnObj.GetComponentInChildren<TextMeshProUGUI>();

        button.OnClick.RemoveAllListeners(); // 清除旧的监听
        if (info == null) return;

        if (!info.IsCanUpgrade)
        {
            label.text = $"{info.Name}(Max)";
            return;
        }

        label.text = $"{Loc.Get("ui.upg")} {info.Name}";
        button.OnClick.AddListener(() =>
        {
            GM.Root.InventoryMgr.UngradeBullet(info.ID);
            GetNeedUpgradeBulletInfo(); //刷新 UI
            CurWorkshopController.UseCount();//减少升级次数,默认为1
            UpgradeSuccessPopUISC.InitData(info); //弹出升级成功窗口
            Hide();
        });
    }

    void ShowUpgradeUIDisplay(CustomButton btn)
    {
        if (_upgradeBulletInfoMap.ContainsKey(btn.gameObject))
        {
            UpgradeUIDisplaySC.GetComponent<UIPopAnimator>().PlayShow();
            UpgradeUIDisplaySC.InitData(_upgradeBulletInfoMap[btn.gameObject]);
        }
        else
            Debug.Log("没有对应的升级信息");
    }

    void HideUpgradeUIDisplay(CustomButton btn)
    {
        UpgradeUIDisplaySC.ClearData();
        UpgradeUIDisplaySC.GetComponent<UIPopAnimator>().PlayHide();
    }
    
    #region 数据初始化&&绑定
    public void Bind(MapNodeController curController)
        => CurWorkshopController = curController;
    public void Init()
    {
        BaseMenuButtons = BaseMenu.GetComponentsInChildren<CustomButton>();
        UpgradeBulletButtons = UpgradeBullet.GetComponentsInChildren<CustomButton>();
        //绑定UpgradeUIDisplay事件
        CustomButton _btnUpClay = btnUpClay.GetComponent<CustomButton>();
        _btnUpClay.OnPointerEnterAction -= ShowUpgradeUIDisplay;
        _btnUpClay.OnPointerEnterAction += ShowUpgradeUIDisplay;
        _btnUpClay.OnPointerExitAction -= HideUpgradeUIDisplay;
        _btnUpClay.OnPointerExitAction += HideUpgradeUIDisplay;
        CustomButton _btnUpIce = btnUpIce.GetComponent<CustomButton>();
        _btnUpIce.OnPointerEnterAction -= ShowUpgradeUIDisplay;
        _btnUpIce.OnPointerEnterAction += ShowUpgradeUIDisplay;
        _btnUpIce.OnPointerExitAction -= HideUpgradeUIDisplay;
        _btnUpIce.OnPointerExitAction += HideUpgradeUIDisplay;
        CustomButton _btnUpFire = btnUpFire.GetComponent<CustomButton>();
        _btnUpFire.OnPointerEnterAction -= ShowUpgradeUIDisplay;
        _btnUpFire.OnPointerEnterAction += ShowUpgradeUIDisplay;
        _btnUpFire.OnPointerExitAction -= HideUpgradeUIDisplay;
        _btnUpFire.OnPointerExitAction += HideUpgradeUIDisplay;
        CustomButton _btnUpThunder = btnUpThunder.GetComponent<CustomButton>();
        _btnUpThunder.OnPointerEnterAction -= ShowUpgradeUIDisplay;
        _btnUpThunder.OnPointerEnterAction += ShowUpgradeUIDisplay;
        _btnUpThunder.OnPointerExitAction -= HideUpgradeUIDisplay;
        _btnUpThunder.OnPointerExitAction += HideUpgradeUIDisplay;

        Hide();
    }
    #endregion
    
    #region Hide/Show
    public void Show()
    {
        _UIPopAnimator.PlayShow();
        UIClickOutsideManager.Register(this);
    }

    public void Hide()
    {
        _UIPopAnimator.PlayHide();
        UIClickOutsideManager.Unregister(this);
    }
    public void OnClickOutside() => Hide();
    #endregion
    
    #region 事件注册注销
    void Start() => GM.Root.HotkeyMgr.OnEscapePressed += Hide;
    void OnDestroy() => GM.Root.HotkeyMgr.OnEscapePressed -= Hide; //注册快捷键
    #endregion
}

public class UpgradeBulletInfo
{
    public int ID;
    public string Name;
    public bool IsCanUpgrade;

    public UpgradeBulletInfo(int _id,
        string _name, bool _isCanUpgrade)
    {
        ID = _id;
        Name = _name;
        IsCanUpgrade = _isCanUpgrade;
    }
}