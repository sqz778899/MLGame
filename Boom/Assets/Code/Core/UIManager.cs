using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
    
    //0.StartGame
    //1.CharacterScene
    //2.Level
    //3.SelectLevel
    //4.SelectRole
    //GroupRoleDes
    public GameObject TooltipsRoot;
    public GameObject StandByRoot;
    
    public GameObject G_BulletStandby;
    public GameObject G_SlotStandby;
    
    public GameObject G_Bullet;
    public GameObject G_Setting;
    public GameObject G_Help;
    //............GroupTitle.........
    public GameObject TitleRoot;
    public GameObject TitleGold;
    public GameObject G_CurBulletIcon; //侧边栏当前子弹图标
    public GameObject G_StandbyIcon; //侧边栏待机图标
    
    #region 1.StartGame

    public void InitStartGame()
    {
        InitComon();
    }
    #endregion

    #region 1.CharacterScene
    //1.CharacterScene
    public GameObject SlotRoot;
    public GameObject BulletRoot;
    public GameObject GroupCharacter;
    public GameObject G_BulletSpawnerSlot;
    public GameObject G_BulletRoleSlot;

    public void InitCharacterScene()
    {
        InitComon();
        if (SlotRoot == null)
            SlotRoot = GameObject.Find("SlotRoot");
        if (BulletRoot == null)
            BulletRoot = GameObject.Find("BulletRoot");

        if (SlotRoot == null)
        {
            Debug.LogError("Error : InitCharacterScene");
            return;
        }
        
        G_BulletSpawnerSlot = SlotRoot.transform.GetChild(1).gameObject;
        G_BulletRoleSlot = SlotRoot.transform.GetChild(2).gameObject;
        G_Bullet = BulletRoot.transform.GetChild(0).gameObject;
    }
    #endregion

    #region 2.Level
    public GameObject GroupBuffRogue;
    public LevelLogicMono LevelLogic;
    //childs
    public GameObject EnemyILIns;
    public GameObject CharILIns;
    public void InitCharacterLevel()
    {
        InitComon();

        if (GroupBuffRogue == null)
            GroupBuffRogue = GameObject.Find("GroupBuffRogue");

        if (LevelLogic == null)
            LevelLogic = GameObject.Find("LevelLogic").GetComponent<LevelLogicMono>();
        
        //...................Childs............................
        GameObject GroupEnemy = GameObject.Find("GroupEnemy");
        EnemyILIns = GroupEnemy.transform.GetChild(0).gameObject;
        GameObject GroupCharacter = GameObject.Find("GroupCharacter");
        CharILIns = GroupCharacter.transform.GetChild(0).gameObject;
        
    }
    #endregion

    #region 3.MiniMap
    public GameObject ShopRoot;
    public GameObject REventRoot;
    public GameObject RewardRoot;
    
    public void InitSelectLevel()
    {
        InitComon();
        ShopRoot = GameObject.Find("ShopRoot");
        REventRoot = GameObject.Find("REventRoot");
        RewardRoot = GameObject.Find("RewardRoot");
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
        if (TooltipsRoot == null)
            TooltipsRoot = GameObject.Find("TooltipsRoot");
        
        TitleRootMono titleRootMono = TitleRoot.GetComponent<TitleRootMono>();
        TitleGold = titleRootMono.TitleGold;
        G_CurBulletIcon = titleRootMono.G_CurBulletIcon;
        G_StandbyIcon = titleRootMono.G_StandbyIcon;
        G_Help = titleRootMono.G_Help;
        G_Setting = titleRootMono.G_Setting;
        if (G_Setting == null)
            G_Setting = GameObject.Find("GroupSetting");
        
        //StandByRoot
        if (StandByRoot == null)
            StandByRoot = GameObject.Find("StandByRoot");
        if (G_SlotStandby == null)
            G_SlotStandby = StandByRoot.transform.GetChild(0).gameObject;
        if (G_BulletStandby == null)
            G_BulletStandby = StandByRoot.transform.GetChild(1).gameObject;
        
        if (G_Bullet == null)
            G_Bullet = GameObject.Find("GroupBullet");
    }
    
}
