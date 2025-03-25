using System;
using UnityEngine;
using System.Collections.Generic;

public class PlayerData: ScriptableObject
{
    public int MaxHP;
    public int HP;
    public int Coins;
    public int RoomKeys;
    public int Score;
    
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
    public int PerfectDamageBonus = 100;
    public int OverflowDamageBonus = 2;

    public event Action OnHPChanged;

    public void ModifyHP(int amount)
    {
        HP = Mathf.Clamp(HP + amount, 0, MaxHP);
        OnHPChanged?.Invoke();
    }
    
    public void ModifyCoins(int amount) =>Coins += amount;
    public void ModifyRoomKeys(int amount) => RoomKeys += amount;
    public void ModifyScore(int amount) => Score += amount;
    
    public void ClearData()
    {
        MaxHP = 3;
        HP = 3;
        Coins = 0;
        RoomKeys = 0;
        Score = 0;
        //Buff类不清理
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
    ItemAttribute _attrInfo;
    #endregion
}