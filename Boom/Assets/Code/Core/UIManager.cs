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
    [FormerlySerializedAs("GroupBullet")] public GameObject G_Bullet;
    [FormerlySerializedAs("GroupSlotStandby")] public GameObject G_SlotStandby;
    [FormerlySerializedAs("GroupBulletStandby")] public GameObject G_BulletStandby;
    public GameObject TooltipsRoot;
    public GameObject GroupSetting;
    
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
        G_SlotStandby = SlotRoot.transform.GetChild(3).gameObject;

        G_Bullet = BulletRoot.transform.GetChild(0).gameObject;
        G_BulletStandby = BulletRoot.transform.GetChild(1).gameObject;
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

    #region 3.SelectLevel
    public GameObject GroupSelectLevel;
    public GameObject ShopCanvas;
    public GameObject GroupRoll;
    
    public void InitSelectLevel()
    {
        InitComon();
        GroupSelectLevel = GameObject.Find("GroupSelectLevel");
        ShopCanvas = GameObject.Find("CanvasRoll");
        if (GroupSelectLevel == null || ShopCanvas == null)
        {
            Debug.LogError("Erro: InitSelectLevel");
            return;
        }
        
        G_SlotStandby = GroupSelectLevel.transform.GetChild(0).gameObject;
        G_BulletStandby = GroupSelectLevel.transform.GetChild(1).gameObject;
        GroupRoll = ShopCanvas.transform.GetChild(1).gameObject;
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
        if (G_Bullet == null)
            G_Bullet = GameObject.Find("GroupBullet");
        
        if (TooltipsRoot == null)
            TooltipsRoot = GameObject.Find("TooltipsRoot");

        if (GroupSetting == null)
            GroupSetting =GameObject.Find("GroupSetting");
    }

}
