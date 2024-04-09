using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject GroupBullet;
    public GameObject GroupSlotStandby;
    public GameObject GroupBulletStandby;

    
    #region 1.CharacterScene
    //1.CharacterScene
    public GameObject GroupCharacterScene;
    public GameObject GroupCharacter;
    public GameObject GroupBulletSlot;
    public GameObject GroupBulletSlotRole;

    public void InitCharacterScene()
    {
        if (GroupCharacterScene == null)
            GroupCharacterScene = GameObject.Find("GroupCharacterScene");

        if (GroupCharacterScene == null)
        {
            Debug.LogError("Error : InitCharacterScene");
            return;
        }

        GroupCharacter = GroupCharacterScene.transform.GetChild(0).gameObject;
        GroupBulletSlot = GroupCharacterScene.transform.GetChild(1).gameObject;
        GroupBulletSlotRole = GroupCharacterScene.transform.GetChild(2).gameObject;
        GroupSlotStandby = GroupCharacterScene.transform.GetChild(3).gameObject;
        GroupBullet = GroupCharacterScene.transform.GetChild(4).gameObject;
        GroupBulletStandby = GroupCharacterScene.transform.GetChild(5).gameObject;
    }
    #endregion

    #region 3.SelectLevel
    public GameObject GroupSelectLevel;
    public GameObject ShopCanvas;
    public GameObject GroupRoll;
    public void InitSelectLevel()
    {
        GroupSelectLevel = GameObject.Find("GroupSelectLevel");
        ShopCanvas = GameObject.Find("CanvasRoll");
        if (GroupSelectLevel == null || ShopCanvas == null)
        {
            Debug.LogError("Erro: InitSelectLevel");
            return;
        }
        
        GroupSlotStandby = GroupSelectLevel.transform.GetChild(0).gameObject;
        GroupBulletStandby = GroupSelectLevel.transform.GetChild(1).gameObject;
        GroupRoll = ShopCanvas.transform.GetChild(1).gameObject;
    }
    #endregion
}
