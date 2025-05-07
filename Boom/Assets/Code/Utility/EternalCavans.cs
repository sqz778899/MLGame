using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class EternalCavans : MonoBehaviour
{
    public Canvas MCanvas;
    public bool TutorialCloseBagLock = false;
    public bool TutorialFightLock = false;
    public bool TutoriaSwichBulletLock = false;
    public bool TutorialSwichGemLock = false;
    public bool TutorialDragGemLock = false;
    
    [Header("永不凋零的GUI资产")]
    [Header("Bag")]
    public GameObject BagRoot;
    public GameObject BagRootMini;
    public BagRootMini BagRootMiniSC;
    public GameObject BagButtonGO;

    public GameObject GemRoot;
    public GameObject GemRootInner;
    public GameObject EquipBulletSlotRoot;
    public GameObject SpawnerSlotRoot;
    public GameObject SpawnerSlotRootMini;
    public GameObject ItemRoot;
    
    [Header("Common")]
    public GameObject DialogueRoot;
    public GameObject StandbyRoot;
    public GameObject FloatingTextRoot;
    
    [Header("侧栏相关")]
    public GameObject G_SideBar;    //侧边栏
    public GameObject G_CurBulletIcon; //侧边栏当前子弹图标
    
    [Header("GUIMap")]
    public GameObject GUIMapRootGO;
    public GameObject ShopRoot;
    public GameObject RewardRoot;
    
    [Header("战斗地图相关")]
    public GameObject GUIFightMapRootGO;
    public GameObject WarReportGO;
    public Simulator SimulatorSC;
    public GameObject WinGUI;
    public GameObject FailGUI;
    public GameObject ConquerTheLevel;
    public EnemyMiniMapView EnemyMiniMapSC;

    [Header("UI根据各个场景切换表现")] 
    public GameObject Bag;
    public GameObject MagicDust;
    public GameObject TitleRoot;
    public GameObject MapFrame;
    public GameObject BtnSetting;
    
    [Header("内部UI交互操作相关")]
    public GameObject SettingUILv1;
    public GameObject SettingUILv2;
    
    [Header(("对话系统"))]
    public Dialogue DialogueSC;
    public DialogueFight DialogueFightSC;
    [Header("奖励相关")]
    public RollAward RollAwardSC;
    
    [Header("新手引导使用的资产")] 
    public Transform btnBag_Apos;
    public GameObject btnSWBullet;
    public Transform btnSWBullet_Apos;
    public GameObject btnSWGem;
    public Transform btnSWGem_Apos;
    public GameObject btnStart;
    public Transform btnStart_Apos;
    
    [Header("需要数据同步的GUIText")]
    public TextSync txtCoins;
    public TextSync txtRoomKeys;
    public TextSync txtScore;
    public TextSync txtMagicDust;
    public TextSync txtScoreEnd;
    
    [Header("Managers")]
    public DragManager DragManager;
    public TooltipsManager TooltipsManager;
    public RightClickMenuManager RightClickMenuManager;
    public EffectManager _EffectManager;
    
    SceneState CurSceneState { get; set; }
    float _preCameraOrthographicSize;
    bool _isBagOpen = false;
    public event Action OnOpenBag;
    public event Action OnCloseBag;
    public event Action OnFightContinue; //战报后面那个继续的按钮
    public event Action OnWinToNextRoom; //成功返回塔楼
    public event Action OnFailToThisRoom; //失败返回塔楼
    
    void Start()
    {
        GM.Root.HotkeyMgr.OnEscapePressed += CloseBag;//注册快捷键
        GM.Root.HotkeyMgr.OnEscapePressed += CloseSetting;
        _preCameraOrthographicSize = Camera.main.orthographicSize;
    }

    #region 初始化各个场景中的UI显示状态
    public void InStartGame()
    {
        //背包相关
        Bag.SetActive(false);
        MagicDust.SetActive(false);
        TitleRoot.SetActive(false);
        BtnSetting.SetActive(false);
        MapFrame.SetActive(false);
        CurSceneState = SceneState.StartGame;
    }
    
    public void InMainEnv()
    {
        //背包相关
        Bag.SetActive(true);
        BagRoot.SetActive(false);
        BagRootMini.SetActive(false);
        BagButtonGO.SetActive(true);
        
        MagicDust.SetActive(true);
        GUIFightMapRootGO.SetActive(false);
        TitleRoot.SetActive(false);
        MapFrame.SetActive(false);
        BtnSetting.SetActive(true);
        CurSceneState = SceneState.MainEnv;
        InitTextSync();
    }
    
    public void InMapScene()
    {
        //背包相关
        Bag.SetActive(true);
        BagRoot.SetActive(false);
        BagRootMini.SetActive(false);
        BagButtonGO.SetActive(true);
        
        TitleRoot.SetActive(true);
        //G_SideBar.SetActive(true);
        //MapFrame.SetActive(true);
        MagicDust.SetActive(false);
        CurSceneState = SceneState.MapScene;
        InitTextSync();
    }
    
    public void InLoadingScene()
    {
        Bag.SetActive(false);
        MagicDust.SetActive(false);
        GUIFightMapRootGO.SetActive(false);
        TitleRoot.SetActive(false);
        MapFrame.SetActive(false);
        BtnSetting.SetActive(true);
        CurSceneState = SceneState.LoadingScene;
    }
    
    void InitTextSync()
    {
        txtCoins.InitData();
        txtRoomKeys.InitData();
        txtScore.InitData();
        txtScoreEnd.InitData();
        txtMagicDust.InitData();
    }
    #endregion
    
    #region 开关背包
    public void OnOffBag()
    {
        if (TutorialCloseBagLock) return; //教程锁
        if (UIManager.Instance.IsLockedClick) return;
        if (_isBagOpen)
            CloseBag();
        else
            OpendBag();
    }
    void OpendBag()
    {
        if(UIManager.Instance.IsLockedClick) return;
        BagRoot.SetActive(true);
        TitleRoot.SetActive(true);
        _preCameraOrthographicSize = Camera.main.orthographicSize;
        Camera.main.orthographicSize = 5;
        _isBagOpen = true;
        OnOpenBag?.Invoke();
    }

    void CloseBag()
    {
        if (TutorialCloseBagLock) return; //教程锁
        if(UIManager.Instance.IsLockedClick) return;
        
        BagRoot.SetActive(false);
        if(CurSceneState == SceneState.MainEnv)
            TitleRoot.SetActive(false);
        Camera.main.orthographicSize = _preCameraOrthographicSize;
        _isBagOpen = false;
        OnCloseBag?.Invoke();
    }
    
    public void ShowMiniBag()
    {
        BagRootMini.SetActive(true);
        BagRootMini.GetComponent<BagRootMini>().InitData();
    }
    
    public void HideMiniBag() => BagRootMini.SetActive(false);
    #endregion

    #region 战斗结束的界面响应
    public void Continue() => OnFightContinue?.Invoke();
    public void WinToNextRoom() => OnWinToNextRoom?.Invoke();
    public void FailToThisRoom() => OnFailToThisRoom?.Invoke();
    public void GameOver() => GM.Root.QuestMgr.FailQuest();
    
    public void ShowConquerTheLevelGUI()
    {
        ConquerTheLevelGUI curGUISC = ConquerTheLevel.GetComponent<ConquerTheLevelGUI>();
        curGUISC.gameObject.SetActive(true);
        curGUISC.ConquerTheLevel();
    }
    public void ReturnTown()
    {
        GM.Root.QuestMgr.CompleteQuest();
        ConquerTheLevel.SetActive(false);
    }
    
    public void ReturnTownMidWay() =>GM.Root.QuestMgr.CompleteQuest(true);
    #endregion

    #region Setting界面相关
    public void OpenSettingLv2()
    {
        SettingUILv1.GetComponent<ICloseOnClickOutside>().Hide();
        SettingUILv2.SetActive(true);
    }

    public void OpenSettingLv1()
    {
        SettingUILv1.GetComponent<ICloseOnClickOutside>().Show();
        SettingUILv2.SetActive(false);
    }

    public void CloseSetting()
    {
        SettingUILv1.GetComponent<ICloseOnClickOutside>().Hide();
        SettingUILv2.SetActive(false);
    }
    #endregion
    
    public void ExitGame() =>MSceneManager.Instance.ExitGame();
    
    #region 单例的加载卸载
    public static EternalCavans Instance { get; private set; }
    public void InitData()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoadedCavans;
        }
        else if (Instance != this)
            Destroy(gameObject);
    }
    void Awake() =>  InitData();
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoadedCavans;
        GM.Root.HotkeyMgr.OnEscapePressed -= CloseBag; //注册快捷键
        GM.Root.HotkeyMgr.OnEscapePressed -= CloseSetting;
    }

    void OnSceneLoadedCavans(Scene scene, LoadSceneMode mode) =>MCanvas.worldCamera = Camera.main;
    #endregion
}
