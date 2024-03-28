using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    public MapNodeState State;
    public GameObject imLocked;
    public GameObject imNode;
    public GameObject imIsFinish;

    void Start()
    {
        ChangeState();
    }

    void ChangeState()
    {
        switch (State)
        {
            case MapNodeState.Locked:
                imLocked.SetActive(true);
                imNode.SetActive(false);
                imIsFinish.SetActive(true);
                break;
            case MapNodeState.UnLocked:
                imLocked.SetActive(false);
                imNode.SetActive(true);
                imIsFinish.SetActive(false);
                break;
            case MapNodeState.IsFinish:
                imLocked.SetActive(false);
                imNode.SetActive(false);
                imIsFinish.SetActive(true);
                break;
        }
    }
}
