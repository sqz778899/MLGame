using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameLogic : KeyBoardBase
{
    internal void Start()
    {
        base.Start();
        UIManager.Instance.InitStartGame();
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
