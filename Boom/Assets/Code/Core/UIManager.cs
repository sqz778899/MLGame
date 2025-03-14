using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : ScriptableObject
{
    //Global Control
    public bool IsLockedClick = false;

    //0.StartGame
    //1.MainEnv
    //2.LevelScene
    [Header("全局通用")]
    //............CommonRoot.........
    GameObject TooltipsRoot; //外部不关心
    public GameObject TooltipsGO;
    GameObject RightClickMenuRoot; //外部不关心
    public GameObject RightClickGO;
    
    public GameObject StandbyRoot;
    public GameObject EffectRoot; //特效根节点
    public GameObject Level; //放置关卡的节点
    public GameObject WarReportGO;//战报根GUI
   
    public GameObject ShopRoot;
    public GameObject RewardRoot;
    public GameObject DialogueRoot;
    
    //............GroupTitle.........
    public GameObject TitleRoot;
  
    public GameObject TitleGold;
    public GameObject G_SideBar;    //侧边栏
    public GameObject G_CurBulletIcon; //侧边栏当前子弹图标
    public GameObject G_StandbyIcon; //侧边栏待机图标
    
    //............Bag.........
    //Bag Bullet
    public GameObject SpawnerSlotRoot;  //子弹孵化器的Group
    public GameObject SpawnerSlotRootMini;  //子弹孵化器的Group
    //Bag Item
    public GameObject BagItemRoot;           //道具根节点
    public GameObject EquipItemRoot;         //元素均衡槽根节点
    //Bag Gem
    public GameObject BagGemRootGO;            //宝石根节点
    //Bag Common
    public GameObject DragObjRoot;            //拖动物品时候的悬浮父节点
    public GameObject BagReadySlotRootGO;     //子弹在人物右侧的Group
    
    [Header("LevelScene专属")]
    public GameObject MapFightRoot;           //关卡根节点
    public GameObject MainSceneGO;            //切换总闸 
    public GameObject MapManagerGO;
    
    public GameObject BattleLogicGO;
    public GameObject G_BulletInScene; //场景内的子弹的父节点
    public GameObject RoleIns;

    #region 0.StartGame
    public void InitStartGame()
    {
        InitComon();
    }
    #endregion
    
    #region 1.MainEnv
    
    
    #endregion
    
    #region 2.LevelScene
    public void InitMainScene()
    {
        InitComon();
        if (MapFightRoot == null)
        {
            MapFightRoot = GameObject.Find("MapFightRoot");
            Level = MapFightRoot.transform.GetChild(0).gameObject;
        }
        
        if (MainSceneGO == null)
            MainSceneGO = GameObject.Find("MainScene");
        MainSceneMono curMainSC = MainSceneGO.GetComponent<MainSceneMono>();
        
        GUIFightRoot curFighRootSc = curMainSC.GUIFightScene.GetComponent<GUIFightRoot>();
        BattleLogicGO = curFighRootSc.FightLogicGO;
        G_BulletInScene = curFighRootSc.G_BulletInScene;
        RoleIns = curFighRootSc.CharILIns;
    }
    #endregion
    

    
    void InitComon()
    {
        IsLockedClick = false;
        InitGUIBag();
        
        #region InitTileRoot相关
        if (TitleRoot == null)
            TitleRoot = GameObject.Find("TitleRoot");
        if (TitleRoot == null)
            return;
        
        TitleRootMono titleRootMono = TitleRoot.GetComponent<TitleRootMono>();
        TitleGold = titleRootMono.TitleGold;
        G_SideBar = titleRootMono.G_SideBar;
        G_CurBulletIcon = titleRootMono.G_CurBulletIcon;
        G_StandbyIcon = titleRootMono.G_StandbyIcon;
        TooltipsRoot = titleRootMono.TooltipsRoot;
        if(TooltipsGO == null)  TooltipsGO = ResManager.instance.CreatInstance(PathConfig.TooltipAsset);
        TooltipsGO.transform.SetParent(TooltipsRoot.transform,false);
        TooltipsGO.SetActive(false);
        RightClickMenuRoot = titleRootMono.RightClickMenuRoot;
        if (RightClickGO == null) RightClickGO = ResManager.instance.CreatInstance(PathConfig.RightClickMenu);
        RightClickGO.transform.SetParent(RightClickMenuRoot.transform,false);
        RightClickGO.SetActive(false);
        StandbyRoot = titleRootMono.StandeByRoot;
        #endregion

        #region GUIMap相关
        GUIMap curGUIMapSc = EternalCavans.Instance.GUIMap.GetComponent<GUIMap>();
        ShopRoot = curGUIMapSc.ShopRoot;
        RewardRoot = curGUIMapSc.RewardRoot;
        DialogueRoot = curGUIMapSc.DialogueRoot;
        #endregion
        
        //............CommonRoot.........
        if (DragObjRoot == null)
            DragObjRoot = GameObject.Find("DragObjRoot");
        
        if(EffectRoot == null)
            EffectRoot = GameObject.Find("EffectRoot");
        
        if (WarReportGO == null)
            WarReportGO = GameObject.Find("WarReportRoot").transform.GetChild(0).gameObject;
    }

    void InitGUIBag()
    {
        BagRoot bagRootSC = EternalCavans.Instance.BagRoot.GetComponent<BagRoot>();
        BagReadySlotRootGO = bagRootSC.BagReadySlotGO;
        DragObjRoot = bagRootSC.DragObjRootGO;
        
        #region 初始化背包子弹编辑界面的GUI逻辑
        SpawnerSlotRoot = bagRootSC.GroupBulletSpawnerSlot;
        SpawnerSlotRootMini = EternalCavans.Instance.BagRootMini.GetComponent<BagRootMini>().GroupBulletSpawnerSlot;
        #endregion

        #region 初始化BagItem界面
        BagItemRoot = bagRootSC.BagItemRootGO;
        BagItemRoot BagItemRootSC = BagItemRoot.GetComponent<BagItemRoot>();
        EquipItemRoot = BagItemRootSC.ElementSlotRootGO; //元素均衡槽根节点
        #endregion

        BagGemRootGO = bagRootSC.BagGemRootGO;
    }
    
    #region 单例
    static UIManager s_instance;
    public static UIManager Instance
    {
        get
        {
            s_instance??= ResManager.instance.GetAssetCache<UIManager>(PathConfig.UIManagerOBJ);
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
