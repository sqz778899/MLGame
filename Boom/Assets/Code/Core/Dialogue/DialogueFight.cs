using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueFight : MonoBehaviour
{
    public int MapNodeID;
    public void EnterFight()
    {
        MainSceneMono _mainSceneMono = UIManager.Instance.MainSceneGO.GetComponent<MainSceneMono>();
        MainRoleManager.Instance.CurMapSate.CurMapID = MapNodeID;
        _mainSceneMono.SwitchFightScene();
    }
}
