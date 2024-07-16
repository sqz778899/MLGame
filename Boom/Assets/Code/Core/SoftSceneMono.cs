using UnityEngine;

public class SoftSceneMono:MonoBehaviour
{
    public Camera MainCamera;
    //GUIEditScene
    [Header("GUIEditScene")]
    public GameObject GUIEditScene;
    //MapScene
    [Header("MapScene")]
    public GameObject GUIMapScene;
    public GameObject MapScene;
    //FightScene
    [Header("FightScene")]
    public GameObject GUIFightScene;
    public GameObject FightScene;
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
    }
    
    void MapSceneOff()
    {
        GUIMapScene.SetActive(false);
        MapScene.SetActive(false);
    }
    #endregion
}