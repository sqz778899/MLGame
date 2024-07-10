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

    public List<RollPR> BuffProbabilityPool;

    public TextMeshPro txtTitle;
    //解锁迷雾的参数
    public NodeOpenFog OpenFog;
    //撒点排除的碰撞体
    public CircleCollider2D ColExclude;

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
                OpenFog = GlobalGameDataManager.Instance.LockedPara;
                break;
            case MapNodeState.UnLocked:
                Bubble.SetActive(true);
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
                Bubble.SetActive(false);
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
                OpenFog = GlobalGameDataManager.Instance.LockedPara;
                break;
            case MapNodeState.IsFinish:
                Bubble.SetActive(false);
                curMainBlock.SetBlockFinish();
                Node_Locked.SetActive(true);
                foreach (var each in _fxs)
                    each.Stop();
                OpenFog = GlobalGameDataManager.Instance.LockedPara;
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
                OpenFog = GlobalGameDataManager.Instance.LockedPara;
                break;
            case MapNodeState.IsFinish:
                Bubble.SetActive(false);
                Node_Main.SetActive(false);
                Node_Finish.SetActive(true);
                foreach (var each in _fxs)
                    each.Stop();
                OpenFog = GlobalGameDataManager.Instance.LockedPara;
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
