using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLogic : MonoBehaviour
{
    public Transform MapGroup;
    void Start()
    {
        int curMapID = MSceneManager.Instance.MapID;
        string curMapName = string.Format("P_Map_{0}.prefab", curMapID.ToString("D2"));
        Instantiate(
            ResManager.instance.GetAssetCache<GameObject>(PathConfig.LevelAssetDir + curMapName),
            MapGroup);
    }
}
