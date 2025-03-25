using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerManager : MonoBehaviour
{
    public GameObject RoleInFightGO;
    public GameObject RoleInMapGO;
    public RoleInMap RoleInMapSC;
    [Header("重要数据")]
    public PlayerData _PlayerData;
    public QuestData _QuestData;
    
    public void ClearPlayerData()
    {
        _PlayerData.ClearData();
    }
    
    #region 单例的加载卸载
    public static PlayerManager Instance { get; private set; }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            // 确保只加载一次，使用单例缓存
            _PlayerData = GameDataCache.PlayerData;
            _QuestData = GameDataCache.QuestData;
        }
        else
            Destroy(gameObject);
    }
    #endregion 
}

public static class GameDataCache
{
    static QuestData _questData;
    static PlayerData _playerData;

    public static QuestData QuestData => 
        _questData ??= ResManager.instance.GetAssetCache<QuestData>(PathConfig.QuestDataPath);

    public static PlayerData PlayerData => 
        _playerData ??= ResManager.instance.GetAssetCache<PlayerData>(PathConfig.PlayerDataPath);
}