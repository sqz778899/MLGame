using Sirenix.Utilities;
using TMPro;
using UnityEngine;

public class WonderWorkshop : MonoBehaviour,ICloseOnClickOutside
{
    [Header("基本资产")] 
    [SerializeField] GameObject BaseMenu;
    [SerializeField] GameObject UpgradeBullet;

    [Header("UpgradeButton")] 
    [SerializeField] GameObject btnUpClay;
    [SerializeField] GameObject btnUpIce;
    [SerializeField] GameObject btnUpFire;
    [SerializeField] GameObject btnUpThunder;
    
    CustomButton[] BaseMenuButtons;
    CustomButton[] UpgradeBulletButtons;
    UpgradeBulletInfo clayInfo;
    UpgradeBulletInfo iceInfo;
    UpgradeBulletInfo fireInfo;
    UpgradeBulletInfo thunderInfo;
    
    [Header("UI 通用交互相关")]
    [SerializeField] RectTransform ClickRoot;
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
        SetupUpgradeButton(btnUpClay,GM.Root.InventoryMgr.GetBulletClayInfo());
        SetupUpgradeButton(btnUpIce,GM.Root.InventoryMgr.GetBulletIceInfo());
        SetupUpgradeButton(btnUpFire,GM.Root.InventoryMgr.GetBulletFireInfo());
        SetupUpgradeButton(btnUpThunder,GM.Root.InventoryMgr.GetBulletThunderInfo());
    }
    
    void SetupUpgradeButton(GameObject btnObj, UpgradeBulletInfo info)
    {
        var button = btnObj.GetComponent<CustomButton>();
        var label = btnObj.GetComponentInChildren<TextMeshProUGUI>();

        button.OnClick.RemoveAllListeners(); // 清除旧的监听
        if (info == null) return;

        if (!info.IsCanUpgrade)
        {
            label.text = $"{info.Name}(Max)";
            return;
        }

        label.text = $"升级 {info.Name}";
        button.OnClick.AddListener(() =>
        {
            GM.Root.InventoryMgr.UngradeBullet(info.ID);
            GetNeedUpgradeBulletInfo(); //刷新 UI
            CurWorkshopController.UseCount();//减少升级次数,默认为1
            Hide();
        });
    }


    #region 数据初始化&&绑定
    public void Bind(MapNodeController curController)
        => CurWorkshopController = curController;
    public void Init()
    {
        BaseMenuButtons = BaseMenu.GetComponentsInChildren<CustomButton>();
        UpgradeBulletButtons = UpgradeBullet.GetComponentsInChildren<CustomButton>();
    }
    #endregion
    
    #region Hide/Show
    public void Show()
    {
        gameObject.SetActive(true);
        UIClickOutsideManager.Register(this);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
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