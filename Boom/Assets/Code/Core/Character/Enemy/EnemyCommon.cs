using System;
using System.Collections.Generic;
using UnityEngine;

public class DamageState
{
    public int Ice;
    public int Fire;
    public int Electric;

    public DamageState()
    {
        Ice = 0;
        Fire = 0;
        Electric = 0;
    }
}

#region 敌人
public class EnemyData
{
    public int ID;
    public int MaxHP;
    public int CurHP;
    public EnemyState EState;
    public Award CurAward;
    public List<ShieldData> Shields;
    public bool IsDead => CurHP <= 0;
    public event Action OnTakeDamage;

    public EnemyData(int _id,int _hp,ShieldConfigData _shieldsConfig = null,Award _award = default)
    {
        ID = _id;
        MaxHP = _hp;
        CurHP = _hp;
        CurAward = _award;
        if (_shieldsConfig == null)
            Shields = new List<ShieldData>();
        else
            Shields = _shieldsConfig.ToData();
        EState = EnemyState.live;
    }
    
    public EnemyData(){}

    public void TakeDamage(int damage)
    {
        CurHP = Mathf.Clamp(CurHP - damage, 0, MaxHP);
        OnTakeDamage?.Invoke();
    }
}

public enum EnemyState
{
    live = 1,
    hit = 2,
    dead = 3
}

[Serializable]
public struct Award
{
    public int BaseScore;
    public int Coin;
    public List<int> SupremeCharms;
    public List<int> Items;

    public Award(int baseScore = 0,int _coin = 0,List<int> _supremeCharms = null,List<int> _items = null)
    {
        BaseScore = baseScore;
        Coin = _coin;
        SupremeCharms = _supremeCharms ?? new List<int>();
        Items = _items ?? new List<int>();
    }
}

[Serializable]
public class EnemyConfigData
{
    public int ID;
    public int HP;
    public ShieldConfigData ShieldConfig;
    public Award CurAward;
}
#endregion