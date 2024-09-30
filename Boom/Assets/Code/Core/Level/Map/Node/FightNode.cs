using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FightNode:MapNodeBase
{
    public int LevelID;
    MainSceneMono _mainSceneMono;

    public void EnterFight()
    {
        if (_mainSceneMono==null)
            _mainSceneMono = UIManager.Instance.MainSceneGO.GetComponent<MainSceneMono>();
        
        MSceneManager.Instance.CurMapSate.LevelID = LevelID;
        _mainSceneMono.SwitchFightScene();
    }
}