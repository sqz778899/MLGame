using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FightNode:MapNodeBase
{
    public GameObject LevelMap;
    MainSceneMono _mainSceneMono;

    public void EnterFight()
    {
        if (_mainSceneMono==null)
            _mainSceneMono = UIManager.Instance.MainSceneGO.GetComponent<MainSceneMono>();
        
        MainRoleManager.Instance.CurMapSate.CurMapNodeID = MapNodeID;
        _mainSceneMono.SwitchFightScene();
    }
}