using System;
using UnityEngine;

public class MainSceneMono:MonoBehaviour
{
    public Camera MainCamera;
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
        //.................Global...........................
        TrunkManager.Instance.LoadSaveFile();
        //.................Local...........................
        UIManager.Instance.InitMainScene();
        MainRoleManager.Instance.InitData();
    }

    void Start()
    {
        if (_mapLogic==null)
            _mapLogic = UIManager.Instance.MapLogicGO.GetComponent<MapLogic>();
        if (_fightLogic==null)
            _fightLogic = UIManager.Instance.MapLogicGO.GetComponent<FightLogic>();
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
        MainCamera.orthographic = true;
        GUIEditScene.SetActive(true);
    }
    void EditSceneOff()
    {
        GUIEditScene.SetActive(false);
    }
    void FightSceneOn()
    {
        MainCamera.orthographic = true;
        GUIFightScene.SetActive(true);
        FightScene.SetActive(true);
        _fightLogic.InitData();
    }
    void FightSceneOff()
    {
        GUIFightScene.SetActive(false);
        FightScene.SetActive(false);
    }
    
    void MapSceneOn()
    {
        MainCamera.orthographic = false;
        GUIMapScene.SetActive(true);
        MapScene.SetActive(true);
        _mapLogic.InitData();
    }
    
    void MapSceneOff()
    {
        GUIMapScene.SetActive(false);
        MapScene.SetActive(false);
    }
    #endregion
}