using System;
using UnityEngine;

public class MainSceneMono:MonoBehaviour
{
    //GUIEditScene
    [Header("GUIEditScene")]
    public GameObject GUIEditScene;
    //MapScene
    [Header("MapScene")]
    MapLogic _mapLogic;
    public GameObject GUIMapScene;
    public GameObject MapScene;
    //FightScene
    [Header("FightScene")] 
    FightLogic _fightLogic;
    public GameObject GUIFightScene;
    public GameObject FightScene;

    void Awake()
    {
        UIManager.Instance.InitMainScene();
        //.................Global...........................
        TrunkManager.Instance.LoadSaveFile();
        //.................Local...........................
        //UIManager.Instance.InitMainScene();
        MainRoleManager.Instance.InitData();
    }

    void Start()
    {
        if (_mapLogic==null)
            _mapLogic = UIManager.Instance.MapLogicGO.GetComponent<MapLogic>();
        if (_fightLogic==null)
            _fightLogic = UIManager.Instance.FightLogicGO.GetComponent<FightLogic>();
    }

    public void SwitchEditScene()
    {
        FightSceneOff();
        MapSceneOff();
        EditSceneOn();
    }
    
    public void SwitchMapScene()
    {
        EditSceneOff();
        FightSceneOff();
        MapSceneOn();
    }
    
    public void SwitchFightScene()
    {
        EditSceneOff();
        MapSceneOff();
        FightSceneOn();
    }

    #region 独立小开关
    void EditSceneOn()
    {
        Camera.main.transform.position = new Vector3(0,1,-10);
        GUIEditScene.SetActive(true);
    }
    void EditSceneOff()
    {
        GUIEditScene.SetActive(false);
    }
    void FightSceneOn()
    {
        GUIFightScene.SetActive(true);
        for (int i = 0; i < FightScene.transform.childCount; i++)
            FightScene.transform.GetChild(i).gameObject.SetActive(true);
       
        _fightLogic?.InitData();
        UIManager.Instance.G_CurBulletIcon?.SetActive(false);
        UIManager.Instance.G_StandbyIcon?.SetActive(false);
    }
    void FightSceneOff()
    {
        GUIFightScene.SetActive(false);
        for (int i = 0; i < FightScene.transform.childCount; i++)
            FightScene.transform.GetChild(i).gameObject.SetActive(false);
        _fightLogic?.UnloadData();
        UIManager.Instance.G_CurBulletIcon?.SetActive(true);
        UIManager.Instance.G_StandbyIcon?.SetActive(true);
    }
    
    void MapSceneOn()
    {
        GUIMapScene.SetActive(true);
        MapScene.SetActive(true);
        _mapLogic?.InitMapData();
    }
    
    void MapSceneOff()
    {
        GUIMapScene.SetActive(false);
        MapScene.SetActive(false);
    }
    #endregion
}