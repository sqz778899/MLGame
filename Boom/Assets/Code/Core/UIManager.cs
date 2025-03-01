using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : ScriptableObject
{
    //Global Control
    public bool IsLockedClick = false;

    //0.StartGame
    //1.CharacterScene
    //2.Level
    //3.SelectLevel
    //4.SelectRole
    //GroupRoleDes
    //............CommonRoot.........
    GameObject TooltipsRoot; //外部不关心
    public GameObject TooltipsGO;
    GameObject RightClickMenuRoot; //外部不关心
    public GameObject RightClickGO;
    
    public GameObject G_StandbyMat;
    
    public GameObject EffectRoot; //特效根节点
    public GameObject MapFightRoot;//关卡根节点
    public GameObject Level; //放置关卡的节点
    public GameObject WarReportGO;//战报根GUI
    
    //............GroupTitle.........
    public GameObject TitleRoot;
  
    public GameObject TitleGold;
    public GameObject G_SideBar;    //侧边栏
    public GameObject G_CurBulletIcon; //侧边栏当前子弹图标
    public GameObject G_StandbyIcon; //侧边栏待机图标

    #region 1.StartGame
    public void InitStartGame()
    {
        InitComon();
    }
    #endregion

    #region 2.MainScene
    public GameObject MainSceneGO;            //切换总闸 
    //1.CharacterScene
    [Header("Bag")]
    //Bag Bullet
    public GameObject G_BulletSpawnerSlot;  //子弹孵化器的Group
    //Bag Item
    public GameObject BagItemRootGO;           //道具根节点
    public GameObject EquipItemRootGO;       //元素均衡槽根节点
    //Bag Gem
    public GameObject BagGemRootGO;            //宝石根节点
    //Bag Common
    public GameObject DragObjRoot;            //拖动物品时候的悬浮父节点
    public GameObject BagReadySlotRootGO;     //子弹在人物右侧的Group
    
    [Header("MiniMap")]
    public GameObject MapLogicGO;
    public GameObject ShopRoot;
    public GameObject RewardRoot;
    public GameObject DialogueRoot;
    
    [Header("FightScene")]
    public GameObject FightLogicGO;
    public GameObject G_BulletInScene; //场景内的子弹的父节点
    public GameObject RoleIns;

    public void InitMainScene()
    {
        InitComon();
        if (MainSceneGO == null)
            MainSceneGO = GameObject.Find("MainScene");
        MainSceneMono curMainSC = MainSceneGO.GetComponent<MainSceneMono>();
        
        GUIMap curGUIMapSc = curMainSC.GUIMap.GetComponent<GUIMap>();
        MapLogicGO = curGUIMapSc.MapLogicGO;
        ShopRoot = curGUIMapSc.ShopRoot;
        RewardRoot = curGUIMapSc.RewardRoot;
        DialogueRoot = curGUIMapSc.DialogueRoot;
        
        GUIFightRoot curFighRootSc = curMainSC.GUIFightScene.GetComponent<GUIFightRoot>();
        FightLogicGO = curFighRootSc.FightLogicGO;
        G_BulletInScene = curFighRootSc.G_BulletInScene;
        RoleIns = curFighRootSc.CharILIns;
    }
    #endregion

    #region SelectRole
    public GameObject GroupRoleDes;
    public GameObject SelRoleLogic;
    public void InitSelectRole()
    {
        InitComon();
        GroupRoleDes = GameObject.Find("GroupRoleDes");
        SelRoleLogic = GameObject.Find("SelRoleLogic");
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
        #endregion
        
        //............CommonRoot.........
        if(TooltipsRoot == null) TooltipsRoot = GameObject.Find("TooltipsRoot");
        if(TooltipsGO == null)  TooltipsGO = ResManager.instance.CreatInstance(PathConfig.TooltipAsset);
        TooltipsGO.transform.SetParent(TooltipsRoot.transform,false);
        TooltipsGO.SetActive(false);

        if(RightClickMenuRoot == null) RightClickMenuRoot = GameObject.Find("RightClickMenuRoot");
        if (RightClickGO == null) RightClickGO = ResManager.instance.CreatInstance(PathConfig.RightClickMenu);
        RightClickGO.transform.SetParent(RightClickMenuRoot.transform,false);
        RightClickGO.SetActive(false);
        
        if (G_StandbyMat == null)
            G_StandbyMat = GameObject.Find("G_StandbyMat");
        
        if (DragObjRoot == null)
            DragObjRoot = GameObject.Find("G_Bullet");
        
        if(EffectRoot == null)
            EffectRoot = GameObject.Find("EffectRoot");

        if (MapFightRoot == null)
        {
            MapFightRoot = GameObject.Find("MapFightRoot");
            Level = MapFightRoot.transform.GetChild(0).gameObject;
        }
        
        if (WarReportGO == null)
            WarReportGO = GameObject.Find("WarReportRoot").transform.GetChild(0).gameObject;
    }

    void InitGUIBag()
    {
        if(MainSceneGO == null)
            MainSceneGO = GameObject.Find("MainScene");
        if(MainSceneGO == null)
            return;
        
        MainSceneMono curMainSC = MainSceneGO.GetComponent<MainSceneMono>();
        BagRoot bagRootSC = curMainSC.GUIBagRoot.GetComponent<BagRoot>();
        BagReadySlotRootGO = bagRootSC.BagReadySlotGO;
        DragObjRoot = bagRootSC.DragObjRootGO;
        
        #region 初始化背包子弹编辑界面的GUI逻辑
        BagBulletRoot bagBulletRootSC = bagRootSC.BagBulletRootGO.GetComponent<BagBulletRoot>();
        G_BulletSpawnerSlot = bagBulletRootSC.G_BulletSpawnerSlot;
        #endregion

        #region 初始化BagItem界面
        BagItemRootGO = bagRootSC.BagItemRootGO;
        BagItemRoot BagItemRootSC = BagItemRootGO.GetComponent<BagItemRoot>();
        EquipItemRootGO = BagItemRootSC.ElementSlotRootGO; //元素均衡槽根节点
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
