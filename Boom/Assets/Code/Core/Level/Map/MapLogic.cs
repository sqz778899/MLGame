using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLogic : MonoBehaviour
{
    public Transform MapGroup;
    MapNode[] _allNodes;
    void Start()
    {
        int curMapID = MSceneManager.Instance.CurMapSate.MapID;
        string curMapName = string.Format("P_Map_{0}.prefab", curMapID.ToString("D2"));
        GameObject curMapIns = Instantiate(
            ResManager.instance.GetAssetCache<GameObject>(PathConfig.LevelAssetDir + curMapName),
            MapGroup);
        curMapIns.transform.SetSiblingIndex(0);

        _allNodes = MapGroup.GetComponentsInChildren<MapNode>();
        RefreshMapNodeState();
        //.............Global..................
        TrunkManager.Instance.LoadSaveFile();
        //.............Local...................
        UIManager.Instance.InitSelectLevel();
        CharacterManager.Instance.InstanceStandbyBullets();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (RollManager.Instance.ShopActive)
                RollManager.Instance.OnOffShop();
            else
                return;
        }
    }

    public void RefreshMapNodeState()
    {
        List<int> IsFinishedLevels = MSceneManager.Instance.CurMapSate.IsFinishedLevels;
        foreach (MapNode eachNode in _allNodes)
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
            MapNode NextNode = null;
            foreach (MapNode eachNode in _allNodes)
            {
                if (eachNode.LevelID == nextLevelID)
                    NextNode = eachNode;
            }

            if (NextNode != null)
            {
                NextNode.State = MapNodeState.UnLocked;
                NextNode.ChangeState();
            }
        }
    }
}
