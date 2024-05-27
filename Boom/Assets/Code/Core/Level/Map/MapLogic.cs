using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteAlways]
public class MapLogic : MonoBehaviour
{
    public Transform MapGroup;
    MapNodeBase[] _allMainNodes;

    Vector4[] _UnLockNodeCenters = new Vector4[30];
    float[] _UnLockNodeRadiuss = new float[30];
    float[] _FadeRanges = new float[30];
    void Start()
    {
        /*int curMapID = MSceneManager.Instance.CurMapSate.MapID;
        string curMapName = string.Format("P_Map_{0}.prefab", curMapID.ToString("D2"));
        GameObject curMapIns = Instantiate(
            ResManager.instance.GetAssetCache<GameObject>(PathConfig.LevelAssetDir + curMapName),
            MapGroup);
        curMapIns.transform.SetSiblingIndex(0);*/

        _allMainNodes = MapGroup.GetComponentsInChildren<MainNode>();
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

        SetOpenFogPara();
    }

    void SetOpenFogPara()
    {
        for (int i = 0; i < 30; i++)
        {
            if (i < _allMainNodes.Length)
            {
                NodeOpenFog openFog = _allMainNodes[i].OpenFog;
                _UnLockNodeCenters[i] = _allMainNodes[i].transform.position;
                _UnLockNodeRadiuss[i] = openFog.openFogRadius;
                _FadeRanges[i] = openFog.openFogFadeRange;
            }
            else
            {
                _UnLockNodeCenters[i] = Vector4.zero;
                _UnLockNodeRadiuss[i] = 0;
                _FadeRanges[i] = 0;
            }
        }
        Shader.SetGlobalVectorArray("_UnLockNodeCenters", _UnLockNodeCenters);
        Shader.SetGlobalFloatArray("_UnLockNodeRadiuss", _UnLockNodeRadiuss);
        Shader.SetGlobalFloatArray("_FadeRanges", _FadeRanges);
    }

    public void RefreshMapNodeState()
    {
        List<int> IsFinishedLevels = MSceneManager.Instance.CurMapSate.IsFinishedLevels;
        foreach (MainNode eachNode in _allMainNodes)
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
            foreach (MainNode eachNode in _allMainNodes)
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
