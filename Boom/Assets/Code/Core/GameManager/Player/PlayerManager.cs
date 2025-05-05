using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject RoleInFightGO;
    public GameObject RoleInMapGO;
    public RoleInMap RoleInMapSC;
    public RoleInner RoleInFightSC;
    [Header("重要数据")]
    public PlayerData _PlayerData;
    public QuestData _QuestData;
    
    public event Action OnTalentLearned;
    public void ClearPlayerData()
    {
        _PlayerData.ClearData();
    }

    public void InitData()
    {
        _PlayerData = new PlayerData();
        _QuestData = new QuestData();
    }
    /// 读取天赋数据
    public void LoadTalent()
    {
        ClearPlayerData();
        InventoryManager.Instance.ClearInventoryData();
        foreach (var each in _PlayerData.Talents)
        {
            if (!each.IsLearned) continue;
            TalentFunctions.LearnTalent(each.ID);
        }
        OnTalentLearned?.Invoke();
    }
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