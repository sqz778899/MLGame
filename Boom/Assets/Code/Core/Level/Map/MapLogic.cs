using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//[ExecuteAlways]
public class MapLogic : MonoBehaviour
{
    public Transform MapGroup;
    
    List<MapNodeBase> _allNodes;
    List<FightNode> _mainNodes;
    List<MapNodeBase> _otherNodes;

    Vector4[] _UnLockNodeCenters = new Vector4[30];
    float[] _UnLockNodeRadiuss = new float[30];
    float[] _FadeRanges = new float[30];
    void Start()
    {
        _allNodes = MapGroup.GetComponentsInChildren<MapNodeBase>().ToList();
        _mainNodes = new List<FightNode>();
        _otherNodes = new List<MapNodeBase>();
        foreach (var each in _allNodes)
        {
            if (each.Type == MapNodeType.Main)
                _mainNodes.Add(each as FightNode);
            else
                _otherNodes.Add(each);
        }
        RefreshMapNodeState();
    }

    public void InitData()
    {
        _allNodes = MapGroup.GetComponentsInChildren<MapNodeBase>().ToList();
        _mainNodes = new List<FightNode>();
        _otherNodes = new List<MapNodeBase>();
        foreach (var each in _allNodes)
        {
            if (each.Type == MapNodeType.Main)
                _mainNodes.Add(each as FightNode);
            else
                _otherNodes.Add(each);
        }
        RefreshMapNodeState();
    }

    void Update()
    {
    }
    
    public void RefreshMapNodeState()
    {
        List<int> IsFinishedLevels = MSceneManager.Instance.CurMapSate.IsFinishedLevels;
        foreach (FightNode eachNode in _mainNodes)
        {
            foreach (int eachIsFinishedLevel in IsFinishedLevels)
            {
                if (eachNode.LevelID == eachIsFinishedLevel)
                {
                    eachNode.State = MapNodeState.IsFinish;
                    eachNode.ChangeState();
                }
            }
        }
        
        //..................下一关.......................
        if (IsFinishedLevels.Contains(MSceneManager.Instance.CurMapSate.LevelID))
        {
            int nextLevelID = MSceneManager.Instance.CurMapSate.LevelID + 1;
            MapNodeBase nextNodeBase = null;
            foreach (FightNode eachNode in _mainNodes)
            {
                if (eachNode.LevelID == nextLevelID)
                    nextNodeBase = eachNode;
            }

            if (nextNodeBase != null)
            {
                nextNodeBase.State = MapNodeState.UnLocked;
                nextNodeBase.ChangeState();
            }
        }
    }
}
