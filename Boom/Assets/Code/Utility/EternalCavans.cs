using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public GameObject BagButtonGO;
    [Header("Common")]
    public GameObject DragObjRootGO;
    public GameObject DialogueRoot;
    public GameObject EffectRoot;
    public GameObject StandbyRoot;
    
    public GameObject TooltipsRoot;
    public GameObject RightClickMenuRoot;
    
    [Header("侧栏相关")]
    public GameObject G_SideBar;    //侧边栏
    public GameObject G_CurBulletIcon; //侧边栏当前子弹图标
    public GameObject G_StandbyIcon; //侧边栏待机图标
    
    [Header("GUIMap")]
    public GameObject GUIMapRootGO;
    public GameObject ShopRoot;
    public GameObject RewardRoot;
    
    [Header("战斗地图相关")]
    public GameObject GUIFightMapRootGO;
    public GameObject WarReportGO;
    public GameObject WinGUI;
    public GameObject FailGUI;
    public GameObject ConquerTheLevel;
    public GameObject EnemyMiniMapGO;

    [Header("UI根据各个场景切换表现")] 
    public GameObject Bag;
    public GameObject MagicDust;
    public GameObject TitleRoot;
    public GameObject MapFrame;
    public GameObject BtnSetting;
    
    [Header("内部UI交互操作相关")]
    public GUIBase SettingUILv1;
    public GUIBase SettingUILv2;
    public GameObject btnBag;
    
    [Header(("对话系统"))]
    public Dialogue DialogueSC;

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
    
    public SceneState CurSceneState { get; private set; }
    public BagRoot BagRootSC => BagRoot.GetComponent<BagRoot>();
    float _preCameraOrthographicSize;
    public event Action OnOpenBag;
    public event Action OnCloseBag;
    public event Action OnFightContinue; //战报后面那个继续的按钮
    public event Action OnWinToNextRoom; //成功返回塔楼
    public event Action OnFailToThisRoom; //失败返回塔楼
    
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
        BagRootSC.RefreshBulletSlotLockedState();
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
    public void OpendBag()
    {
        if(UIManager.Instance.IsLockedClick) return;
        UIManager.Instance.BagUI.ShowBag();
        BagRootSC.RefreshBulletSlotLockedState();
        btnBag.SetActive(false);
        TitleRoot.SetActive(true);
        _preCameraOrthographicSize = Camera.main.orthographicSize;
        Camera.main.orthographicSize = 5;
        OnOpenBag?.Invoke();
    }
    
    public void CloseBag()
    {
        if (TutorialCloseBagLock) return; //教程锁
        if(UIManager.Instance.IsLockedClick) return;
        UIManager.Instance.BagUI.HideBag();
        if(CurSceneState == SceneState.MainEnv)
            TitleRoot.SetActive(false);
        btnBag.SetActive(true);
        Camera.main.orthographicSize = _preCameraOrthographicSize;
        OnCloseBag?.Invoke();
    }
    #endregion

    #region 战斗结束的界面响应
    public void Continue() => OnFightContinue?.Invoke();
    public void WinToNextRoom() => OnWinToNextRoom?.Invoke();
    public void FailToThisRoom() => OnFailToThisRoom?.Invoke();
    public void GameOver() => QuestManager.Instance.FailQuest();
    
    public void ShowConquerTheLevelGUI()
    {
        ConquerTheLevelGUI curGUISC = ConquerTheLevel.GetComponent<ConquerTheLevelGUI>();
        curGUISC.gameObject.SetActive(true);
        curGUISC.ConquerTheLevel();
    }
    public void ReturnTown()
    {
        QuestManager.Instance.CompleteQuest();
        ConquerTheLevel.SetActive(false);
    }
    
    public void ReturnTownMidWay() => QuestManager.Instance.CompleteQuest(true);
    #endregion
    
    public void OpenSettingLv2()
    {
        SettingUILv1.CloseWindow();
        SettingUILv2.OnOffWindow();
    }
    
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
        {
            Debug.LogWarning($"[EternalCavans] Duplicate detected, destroying: {gameObject.name}");
            Destroy(gameObject);
        }
    }

    void Awake()
    {
        InitData();
    }

    void OnDestroy() => SceneManager.sceneLoaded -= OnSceneLoadedCavans;
    void OnSceneLoadedCavans(Scene scene, LoadSceneMode mode) =>MCanvas.worldCamera = Camera.main;
    #endregion
}
