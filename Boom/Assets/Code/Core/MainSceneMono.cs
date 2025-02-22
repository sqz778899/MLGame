using System;
using UnityEngine;

public class MainSceneMono:MonoBehaviour
{
    //GUIEditScene
    [Header("BagRoot")]
    public GameObject GUIBagRoot;
    //MapScene
    [Header("MapScene")]
    public GameObject MapScene;
    public GameObject GUIMap;
    //FightScene
    [Header("FightScene")] 
    public GameObject GUIFightScene;
    public GameObject FightScene;

    
    BagRoot _bagRootSC;
    FightLogic _fightLogic;
    MapLogic _mapLogic;
    
    void Awake()
    {
        UIManager.Instance.InitMainScene();
        //.................Global...........................
        TrunkManager.Instance.LoadSaveFile();
        //.................Local...........................
        //UIManager.Instance.InitMainScene();
        MainRoleManager.Instance.InitData();
        _bagRootSC = GUIBagRoot.GetComponent<BagRoot>();
    }

    void Start()
    {
        if (_mapLogic==null)
            _mapLogic = UIManager.Instance.MapLogicGO.GetComponent<MapLogic>();
        if (_fightLogic==null)
            _fightLogic = UIManager.Instance.FightLogicGO.GetComponent<FightLogic>();
    }

    public void SwitchBag()
    {
        if (UIManager.Instance.IsLockedClick) return;
        
        FightSceneOff();
        MapSceneOff();
        BagOn();
    }
    
    public void SwitchMapScene()
    {
        if (UIManager.Instance.IsLockedClick) return;
        
        BagOff();
        FightSceneOff();
        MapSceneOn();
    }
    
    public void SwitchFightScene()
    {
        if (UIManager.Instance.IsLockedClick) return;
        
        BagOff();
        MapSceneOff();
        FightSceneOn();
    }

    #region 独立小开关
    void BagOn()
    {
        //Camera.main.transform.position = new Vector3(0,1,-10);
        GUIBagRoot.SetActive(true);
        _bagRootSC?.InitData();
    }
    void BagOff()
    {
        GUIBagRoot.SetActive(false);
    }
    void FightSceneOn()
    {
        GUIFightScene.SetActive(true);
        for (int i = 0; i < FightScene.transform.childCount; i++)
            FightScene.transform.GetChild(i).gameObject.SetActive(true);
        try
        {
            _fightLogic.InitData();
            UIManager.Instance.G_CurBulletIcon.SetActive(false);
            UIManager.Instance.G_StandbyIcon.SetActive(false);
        }
        catch (Exception e)
        {
            Debug.LogError("FightSceneOn  Erro！！！");
        }
    }
    void FightSceneOff()
    {
        GUIFightScene.SetActive(false);
        for (int i = 0; i < FightScene.transform.childCount; i++)
            FightScene.transform.GetChild(i).gameObject.SetActive(false);
        
        try
        {
            _fightLogic.UnloadData();
            UIManager.Instance.G_CurBulletIcon.SetActive(true);
            UIManager.Instance.G_StandbyIcon.SetActive(true);
        }
        catch (Exception e)
        {
            Debug.LogError("FightSceneOff  Erro！！！");
        }
    }
    
    void MapSceneOn()
    {
        MapScene.SetActive(true);
        GUIMap.SetActive(true);
        _mapLogic?.InitMapData();
    }
    
    void MapSceneOff()
    {
        GUIMap.SetActive(false);
        MapScene.SetActive(false);
    }
    #endregion
}