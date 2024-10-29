using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : ScriptableObject
{
    #region 单例
    static UIManager s_instance;
    public static UIManager Instance
    {
        get
        {
            if (s_instance == null)
                s_instance = ResManager.instance.GetAssetCache<UIManager>(PathConfig.UIManagerOBJ);
            return s_instance;
        }
    }
    #endregion
    
    //Global Control
    public bool IsPauseClick = false;

    public void SetOtherUIPause()
    {
        TitleRootMono curSC = TitleRoot.GetComponent<TitleRootMono>();
        List<Image> btnImgs = curSC.NeedToControl;
        foreach (var each in btnImgs)
            each.raycastTarget = false;

        IsPauseClick = true;
    }

    public void ResetOtherUIPause()
    {
        TitleRootMono curSC = TitleRoot.GetComponent<TitleRootMono>();
        List<Image> btnImgs = curSC.NeedToControl;
        foreach (var each in btnImgs)
            each.raycastTarget = true;

        IsPauseClick = false;
    }

    //0.StartGame
    //1.CharacterScene
    //2.Level
    //3.SelectLevel
    //4.SelectRole
    //GroupRoleDes
    //............CommonRoot.........
    public GameObject TooltipsRoot;
    public GameObject RightClickMenuRoot;
    public GameObject G_Bullet;
    public GameObject G_StandbyMat;

    public GameObject G_Bag; //背包根节点
    public GameObject ElementSlotRoot; //元素均衡槽根节点
    public GameObject ItemRoot; //道具根节点
    
    //............GroupTitle.........
    public GameObject TitleRoot;
    public GameObject G_Setting;
    public GameObject G_Help;
    public GameObject TitleGold;
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
    [Header("EditBulletSecne")]
    public GameObject G_BulletSpawnerSlot;  //子弹孵化器的Group
    public GameObject G_BulletRoleSlot;     //子弹在人物右侧的Group
    
    [Header("MiniMap")]
    public GameObject MapLogicGO;
    public GameObject MapRoot;
    public GameObject ShopRoot;
    public GameObject REventRoot;
    public GameObject RewardRoot;
    
    [Header("FightScene")]
    public GameObject FightLogicGO;
    public GameObject G_BulletInScene; //场景内的子弹的父节点
    public GameObject G_Buff;
    public GameObject RoleIns;

    public void InitMainScene()
    {
        InitComon();
        if (MainSceneGO == null)
            MainSceneGO = GameObject.Find("MainScene");
        MainSceneMono curMainSC = MainSceneGO.GetComponent<MainSceneMono>();
        
        EditBulletScene CurEditBulletSC = curMainSC.GUIEditScene.GetComponent<EditBulletScene>();
        G_BulletSpawnerSlot = CurEditBulletSC.G_BulletSpawnerSlot;
        G_BulletRoleSlot = CurEditBulletSC.G_BulletRoleSlot;
        G_Bullet = CurEditBulletSC.G_Bullet;
        
        MapScene CurMapSC = curMainSC.GUIMapScene.GetComponent<MapScene>();
        MapLogicGO = CurMapSC.MapLogicGO;
        MapRoot = CurMapSC.MapNode;
        ShopRoot = CurMapSC.ShopRoot;
        REventRoot = CurMapSC.REventRoot;
        RewardRoot = CurMapSC.RewardRoot;
        
        FightScene CurFighSceneSC = curMainSC.GUIFightScene.GetComponent<FightScene>();
        FightLogicGO = CurFighSceneSC.FightLogicGO;
        G_BulletInScene = CurFighSceneSC.G_BulletInScene;
        G_Buff = CurFighSceneSC.G_Buff;
        RoleIns = CurFighSceneSC.CharILIns;
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
        IsPauseClick = false;
        
        #region InitTileRoot相关
        if (TitleRoot == null)
            TitleRoot = GameObject.Find("TitleRoot");
        if (TitleRoot == null)
            return;
        
        TitleRootMono titleRootMono = TitleRoot.GetComponent<TitleRootMono>();
        TitleGold = titleRootMono.TitleGold;
        G_CurBulletIcon = titleRootMono.G_CurBulletIcon;
        G_StandbyIcon = titleRootMono.G_StandbyIcon;
        G_Help = titleRootMono.G_Help;
        G_Setting = titleRootMono.G_Setting;
        G_Bag = titleRootMono.G_Bag;
        if (G_Setting == null)
            G_Setting = GameObject.Find("GroupSetting");
        #endregion
        
        //............CommonRoot.........
        if (ItemRoot == null)
            ItemRoot = G_Bag.GetComponent<Bag>().ItermRoot;
        if (ElementSlotRoot == null)
            ElementSlotRoot = G_Bag.GetComponent<Bag>().ElementSlotRoot;
        
        if (TooltipsRoot == null)
            TooltipsRoot = GameObject.Find("TooltipsRoot");
        
        if (RightClickMenuRoot == null)
            RightClickMenuRoot = GameObject.Find("RightClickMenuRoot");
        
        if (G_StandbyMat == null)
            G_StandbyMat = GameObject.Find("G_StandbyMat");
        
        if (G_Bullet == null)
            G_Bullet = GameObject.Find("G_Bullet");
    }
}
