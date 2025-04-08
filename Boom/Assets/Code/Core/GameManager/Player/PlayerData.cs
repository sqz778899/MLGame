using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class PlayerData: ScriptableObject
{
    //单局会清理的数据
    public int MaxHP;
    public int HP;
    public int Coins;
    public int RoomKeys;
    public int Score;
    
    //局外数据
    public int MagicDust;//魔尘
      
    #region 子弹槽锁定状态
    public event Action BulletSlotStateChanged;
    Dictionary<int, bool> _curBulletSlotLockedState = new();
    public Dictionary<int, bool> CurBulletSlotLockedState
    {
        get => _curBulletSlotLockedState;
        set
        {
            if (_curBulletSlotLockedState != value)
            {
                _curBulletSlotLockedState = value;
                BulletSlotStateChanged?.Invoke(); // 通知变化
            }
        }
    }
    #endregion
    
    //Buff类
    public int PerfectDamageBonus = 100;  // 完美击杀加成
    public int OverflowDamageBonus = 2;   // 溢出伤害加成
    public float ScoreToDustRate = 0.1f;  // 分数转魔尘比例
    public float CoinToDustRate = 2f;     // 金币转魔尘比例
    public int CoinAdd = 0;               // 局内金币加成
    
    //talent
    public List<TalentData> Talents;  // 天赋数据
    //新手教程完成情况
    public TutorialCompletionStatus _TutorialCompletionStatus;
    
    // 宝石伤害加成
    public List<TalentGemBonus> TalentGemBonuses = new();

    #region 各类事件
    public event Action OnHPChanged;
    public event Action OnCoinsAdd;
    public event Action OnCoinsSub;
    public event Action OnRoomKeysChanged;
    public event Action OnScoreChanged;
    public event Action OnMagicDustAdd;
    public event Action OnMagicDustSub;
    #endregion

    #region 对外的数据变动接口
    public void ModifyHP(int amount)
    {
        HP = Mathf.Clamp(HP + amount, 0, MaxHP);
        OnHPChanged?.Invoke();
    }
    
    public void ModifyCoins(int amount)
    {
        Coins += amount;
        //金币的加减采取不同的动画
        if (amount >= 0)
            OnCoinsAdd?.Invoke();
        else
            OnCoinsSub?.Invoke();
    }

    public void ModifyRoomKeys(int amount)
    {
        RoomKeys += amount;
        OnRoomKeysChanged?.Invoke();
    }
    
    public void ModifyScore(int amount)
    {
        Score += amount;
        OnScoreChanged?.Invoke();
    }

    public bool CostMagicDust(int amount)
    {
        if (amount > MagicDust) return false;
        ModifyMagicDust(-amount);
        return true;
    }
    
    public void ModifyMagicDust(int amount)
    {
        MagicDust += amount;
        if (amount >= 0)
            OnMagicDustAdd?.Invoke();
        else
            OnMagicDustSub?.Invoke();
    }

    //任务完成后，结算魔尘奖励
    public void LevelRewards()
    {
        int scoreDustAmount = ScoreCalculator.ScoreToDust(Score);
        int coinDustAmount = ScoreCalculator.CoinToDust(Coins);
        int totalDustAmount = scoreDustAmount + coinDustAmount;
        ModifyMagicDust(totalDustAmount);
    }
    
    public TalentData GetTalent(int talentID) => Talents.Find(t => t.ID == talentID);
    #endregion

    public void ClearData()
    {
        MaxHP = 3;
        HP = 3;
        Coins = 0;
        RoomKeys = 0;
        Score = 0;
        //Buff类初始化
        PerfectDamageBonus = 100;  // 完美击杀加成
        OverflowDamageBonus = 2;   // 溢出伤害加成
        ScoreToDustRate = 0.1f;  // 分数转魔尘比例
        CoinToDustRate = 2f;     // 金币转魔尘比例
        CoinAdd = 0;               // 局内金币加成
        TalentGemBonuses = new();
    }
    
    #region 人物属性
    [Header("人物属性")] 
    public int WaterElement;
    public int FireElement;
    public int ThunderElement;
    public int LightElement;
    public int DarkElement;

    public int DebuffMaxDamage;
    [Header("伤害倍率")]
    public int WaterDamage;
    public int FireDamage;
    public int ThunderDamage;
    public int LightDamage;
    public int DarkDamage;
    public int MaxDamage;
    #endregion
}