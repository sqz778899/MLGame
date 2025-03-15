using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameLogic : KeyBoardBase
{
    internal void Start()
    {
        base.Start();
        TrunkManager.Instance.ForceRefresh();
        UIManager.Instance.InitStartGame();
        SaveManager.LoadSaveFile();
        MainRoleManager.Instance.InitData();
        EternalCavans.Instance.InStartGame();
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
