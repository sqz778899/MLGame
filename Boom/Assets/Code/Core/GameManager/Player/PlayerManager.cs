using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerManager : MonoBehaviour
{
    public GameObject RoleInFightGO;
    public GameObject RoleInMapGO;
    public PlayerData _PlayerData;
    
    #region 单例的加载卸载
    public static PlayerManager Instance { get; private set; }
    
    void Awake()
    {
        _PlayerData =  ResManager.instance.GetAssetCache<PlayerData>(PathConfig.PlayerDataPath);
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    #endregion 
}