using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class MapNode : MonoBehaviour
{
    public int LevelID;
    public MapNodeState State;
    public MapNodeType Type;
    
    public GameObject Node_Main;
    public GameObject Node_Locked;
    public GameObject Node_Finish;
    
    public GameObject Node_FX;
    ParticleSystem[] _fxs;

    public List<RollProbability> BuffProbabilityPool;

    public TextMeshPro txtTitle;
    //解锁迷雾的参数
    public NodeOpenFog OpenFog;

    void Awake()
    {
        _fxs = Node_FX.GetComponentsInChildren<ParticleSystem>(true);
    }

    void Start()
    {
        
        ChangeState();
    }
    
    public void ChangeState()
    {
        if (_fxs == null)
            _fxs = Node_FX.GetComponentsInChildren<ParticleSystem>(true);
       
        txtTitle.text = string.Format("LV{0}", LevelID);
        switch (State)
        {
            case MapNodeState.Locked:
                Node_Locked.SetActive(true);
                Node_Main.SetActive(false);
                foreach (var each in _fxs)
                    each.Stop();
                Node_Finish.SetActive(false);
                OpenFog = GlobalGameDataManager.Instance.LockedPara;
                break;
            case MapNodeState.UnLocked:
                Node_Locked.SetActive(false);
                Node_Main.SetActive(true);
                foreach (var each in _fxs)
                    each.Play();
                Node_Finish.SetActive(false);
                if (Type == MapNodeType.Main)
                {
                    OpenFog = GlobalGameDataManager.Instance.GetUnlockedPara();
                }
                break;
            case MapNodeState.IsFinish:
                Node_Locked.SetActive(false);
                Node_Main.SetActive(false);
                foreach (var each in _fxs)
                    each.Stop();
                Node_Finish.SetActive(true);
                if (Type == MapNodeType.Main)
                {
                    OpenFog = GlobalGameDataManager.Instance.GetUnlockedPara();
                }
                break;
        }
    }

    //Fight
    public void EnterFight()
    {
        MSceneManager.Instance.CurMapSate.LevelID = LevelID;
        MSceneManager.Instance.LoadScene(2);
    }
    
    //Event
    public void RandomEvent()
    {
        Debug.Log("Random Event");
    }
    
    //Shop
    public void EnterShop()
    {
        Debug.Log("Shop !!");
    }
    
    //Gold
    public void GetGold()
    {
        Debug.Log("Gold !!");
    }
    
    //Open
    public void OpenTressureBox()
    {
        Debug.Log("Open Tressure Box !!");
    }
}
