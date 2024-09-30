using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapNodeBase : MonoBehaviour
{
    public MapNodeState State;
    public MapNodeType Type;
    
    public GameObject Node_Main;
    public GameObject Bubble;
    public GameObject Node_Locked;
    public GameObject Node_Finish;
    
    public GameObject Node_FX;
    ParticleSystem[] _fxs;

    void Awake()
    {
        _fxs = Node_FX.GetComponentsInChildren<ParticleSystem>(true);
    }

    internal virtual void Start()
    {
        ChangeState();
    }
    
    public void ChangeState()
    {
        if (_fxs == null)
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
            case MapNodeState.Locked:
                Bubble.SetActive(false);
                Node_Locked.SetActive(true);
                Node_Main.SetActive(true);
                foreach (var each in _fxs)
                    each.Stop();
                Node_Finish.SetActive(false);
        
                break;
            case MapNodeState.UnLocked:
                Bubble.SetActive(true);
                Node_Locked.SetActive(false);
                Node_Main.SetActive(true);
                foreach (var each in _fxs)
                    each.Play();
                Node_Finish.SetActive(false);
                break;
            case MapNodeState.IsFinish:
                Bubble.SetActive(false);
                Node_Locked.SetActive(false);
                Node_Main.SetActive(false);
                foreach (var each in _fxs)
                    each.Stop();
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
                Bubble.SetActive(true);
                Node_Locked.SetActive(false);
                curMainBlock.SetBlockDefault();
                foreach (var each in _fxs)
                    each.Play();
                break;
            case MapNodeState.IsFinish:
                Bubble.SetActive(false);
                curMainBlock.SetBlockFinish();
                Node_Locked.SetActive(true);
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
                Bubble.SetActive(true);
                Node_Main.SetActive(true);
                Node_Finish.SetActive(false);
                foreach (var each in _fxs)
                    each.Play();
                break;
            case MapNodeState.IsFinish:
                Bubble.SetActive(false);
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
