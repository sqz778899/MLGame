using System;
using UnityEngine;
using UnityEngine.Serialization;

public class MainSceneMono:MonoBehaviour
{
    //GUIEditScene
    [Header("BagRoot")]
    public GameObject GUIBagRoot;
    public GameObject GUIBagRoot_Mini;
    
    public BagRoot BagRootSC;
    public BagRootMini BagRootMiniSC;
    //MapScene
    [Header("MapScene")]
    public GameObject MapScene;
    public GameObject GUIMap;
    //FightScene
    [Header("FightScene")] 
    public GameObject GUIFightScene;
    public GameObject FightScene;
    BattleLogic _battleLogic;
    MapLogic _mapLogic;
    
    //总Init接口
    void Awake()
    {
        UIManager.Instance.InitMainScene();
        //.................Global...........................
        SaveManager.LoadSaveFile();
        //.................Local...........................
        //UIManager.Instance.InitMainScene();
        MainRoleManager.Instance.InitData();
        BagRootSC.InitData();
    }

    void Start()
    {
        if (_mapLogic==null)
            _mapLogic = UIManager.Instance.MapLogicGO.GetComponent<MapLogic>();
        if (_battleLogic==null)
            _battleLogic = UIManager.Instance.BattleLogicGO.GetComponent<BattleLogic>();
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

    public void WinThisLevel()
    {
        SwitchMapScene();
        MainRoleManager.Instance.WinThisLevel();
    }
    
    public void FailThisLevel()
    {
        SwitchMapScene();
        MainRoleManager.Instance.FailThisLevel();
    }
    
    public void EndGame()
    {
        MSceneManager.Instance.LoadScene(0);
    }

    #region 独立小开关
    void BagOn()
    {
        //Camera.main.transform.position = new Vector3(0,1,-10);
        GUIBagRoot.SetActive(true);
        BagRootSC?.InitData();
    }
    void BagOff()
    {
        GUIBagRoot.SetActive(false);
    }
    void FightSceneOn()
    {
        GUIFightScene.SetActive(true);
        try { _battleLogic.enabled = true; }catch (Exception e) {}
        for (int i = 0; i < FightScene.transform.childCount; i++)
            FightScene.transform.GetChild(i).gameObject.SetActive(true);
        try
        {
            _battleLogic.InitData();
            UIManager.Instance.G_SideBar.SetActive(false);
        }
        catch (Exception e)
        {
            Debug.LogError("FightSceneOn  Erro！！！");
        }
    }
    void FightSceneOff()
    {
        GUIFightScene.SetActive(false);
        try { _battleLogic.enabled = false; }catch (Exception e) {}
        for (int i = 0; i < FightScene.transform.childCount; i++)
            FightScene.transform.GetChild(i).gameObject.SetActive(false);
        
        try
        {
            _battleLogic.UnloadData();
            UIManager.Instance.G_SideBar.SetActive(true);
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
        try { _mapLogic.enabled = true; }catch (Exception e) {}
    }
    
    void MapSceneOff()
    {
        GUIMap.SetActive(false);
        MapScene.SetActive(false);
        try { _mapLogic.enabled = false; }catch (Exception e) {}
    }
    #endregion
}