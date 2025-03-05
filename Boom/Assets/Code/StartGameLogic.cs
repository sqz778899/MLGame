using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameLogic : KeyBoardBase
{
    internal void Start()
    {
        base.Start();
        UIManager.Instance.InitStartGame();
        SaveManager.LoadSaveFile();
    }
}
