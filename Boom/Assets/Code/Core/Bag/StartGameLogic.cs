using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameLogic : KeyBoardBase
{
    void Start()
    {
        UIManager.Instance.InitStartGame();
        TrunkManager.Instance.LoadSaveFile();
    }
}
