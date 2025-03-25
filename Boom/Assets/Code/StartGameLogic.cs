using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameLogic : KeyBoardBase
{
    internal void Start()
    {
        base.Start();
        TrunkManager.Instance.ForceRefresh();//强制刷新策划数据
        UIManager.Instance.InitStartGame(); //初始化UIManager
        SaveManager.LoadSaveFile();          //读取存档数据
        EternalCavans.Instance.InStartGame(); //初始化UI显隐状态
    }
    
    public void NewGame()
    {
        TrunkManager.Instance.SetSaveFileTemplate();
        MSceneManager.Instance.LoadScene(1);
    }
    
    public void ContinueGame()
    {
        MSceneManager.Instance.LoadScene(1);
        SceneManager.sceneLoaded += OnContinueGame;
    }
    
    public void Setting()
    {
        EternalCavans.Instance.OpenSettingLv2();
    }

    void OnContinueGame(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnContinueGame;
        SaveManager.LoadSaveFile();
    }
    
    public void ExitGame()
    {
        MSceneManager.Instance.ExitGame();
    }
}
