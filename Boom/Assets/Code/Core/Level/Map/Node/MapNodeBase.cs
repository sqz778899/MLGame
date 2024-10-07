using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapNodeBase : MonoBehaviour
{
    public int MapNodeID;
    public MapNodeState State;
    public MapNodeType Type;
    
    public GameObject Node_Main;
    public GameObject Node_Finish;
    
    public GameObject Node_FX;
    ParticleSystem[] _fxs;

    void Awake()    
    {
        if (Node_FX !=null) 
            _fxs = Node_FX.GetComponentsInChildren<ParticleSystem>(true);
    }

    internal virtual void Start()
    {
        ChangeState();
    }
    
    public void ChangeState()
    {
        if (_fxs == null && Node_FX!= null)
            _fxs = Node_FX.GetComponentsInChildren<ParticleSystem>(true);

        switch (Type)
        {
            case MapNodeType.Shop:
                ChangeStateShop();
                break;
            case MapNodeType.Event:
                ChangeStateEvent();
                break;
            case MapNodeType.TreasureBox:
                ChangeStateTreasureBox();
                break;
            default:
                ChangeStateMain();
                break;
        }
    }

    void ChangeStateMain()
    {
        switch (State)
        {
            case MapNodeState.UnLocked:
                Node_Main.SetActive(true);
                Node_Finish.SetActive(false);
                break;
            case MapNodeState.IsFinish:
                Node_Main.SetActive(false);
                Node_Finish.SetActive(true);
                break;
        }
    }

    void ChangeStateShop()
    {
        MatPropertyBlock curMainBlock = Node_Main.GetComponent<MatPropertyBlock>();
        switch (State)
        {
            case MapNodeState.UnLocked:
                curMainBlock.SetBlockDefault();
                foreach (var each in _fxs)
                    each.Play();
                break;
            case MapNodeState.IsFinish:
                curMainBlock.SetBlockFinish();
                foreach (var each in _fxs)
                    each.Stop();
                break;
        }
    }
    
    void ChangeStateEvent()
    {
        //和Shop逻辑一样
        ChangeStateShop();
    }
    
    void ChangeStateTreasureBox()
    {
        switch (State)
        {
            case MapNodeState.UnLocked:
                Node_Main.SetActive(true);
                Node_Finish.SetActive(false);
                foreach (var each in _fxs)
                    each.Play();
                break;
            case MapNodeState.IsFinish:
                Node_Main.SetActive(false);
                Node_Finish.SetActive(true);
                foreach (var each in _fxs)
                    each.Stop();
                break;
        }
    }
    
    public void QuitNode()
    {
        State = MapNodeState.IsFinish;
        UIManager.Instance.ResetOtherUIPause();
        ChangeState();
    }
}
