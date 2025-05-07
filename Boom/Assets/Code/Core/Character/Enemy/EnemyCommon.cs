using System;
using System.Collections.Generic;
using UnityEngine;

#region 敌人
public class EnemyData:IDamageable
{
    public int ID;
    public int MaxHP{ get; set;}
    public int CurHP{ get; set; }
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
    
    public EnemyData(int _id,int _hp,List<ShieldData> _shields = null,Award _award = default)
    {
        ID = _id;
        MaxHP = _hp;
        CurHP = _hp;
        CurAward = _award;
        Shields = new List<ShieldData>();
        for (int i = 0; i < _shields.Count; i++)
        {
            ShieldData newShield = new ShieldData(_shields[i].MaxHP, i);
            Shields.Add(newShield);
        }
        EState = EnemyState.live;
    }
    
    public DamageResult TakeDamage(BulletData source)
    {
        int damage = source.FinalDamage;
        if (IsDead)
            return new DamageResult(0, 0, 0, true, -1);
        int overflow = Mathf.Max(0, damage - CurHP);
        int effective = damage - overflow;
        CurHP = Mathf.Clamp(CurHP - damage, 0, MaxHP);
        EState = IsDead ? EnemyState.dead : EnemyState.hit;
        OnTakeDamage?.Invoke();
        return new DamageResult(damage, effective, overflow, IsDead, /* target index */ -1);
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
    public string RollPoolName;
    public int Coin;
    public List<int> SupremeCharms;
    public List<int> Items;

    public Award(int baseScore = 0,int _coin = 0,List<int> _supremeCharms = null,List<int> _items = null)
    {
        BaseScore = baseScore;
        Coin = _coin;
        SupremeCharms = _supremeCharms ?? new List<int>();
        Items = _items ?? new List<int>();
        RollPoolName = "草怪掉落01";
    }
}

[Serializable]
public class EnemyConfigData
{
    public int ID;
    public int HP;
    public EnemyType _EnemyType;
    public ShieldConfigData ShieldConfig;
    public Award CurAward;
}
#endregion