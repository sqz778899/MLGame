using System;
using System.Collections.Generic;
using UnityEngine;

public class ShieldData:IDamageable
{
    public int MaxHP { get; private set; }
    public int CurHP { get;set; }
    public EnemyState EState;
    public int ShieldIndex { get; private set; }
    public bool IsDead => CurHP <= 0;
    public event Action OnTakeDamage;
    public ShieldData(int maxHP, int index)
    {
        MaxHP = maxHP;
        CurHP = maxHP;
        ShieldIndex = index;
    }
    
    #region 伤害计算相关
    public DamageResult TakeDamage(BulletData source) => DealDamage(source.FinalDamage);
    public DamageResult TakeReactionDamage(int damage) => DealDamage(damage);
    
    public void ModifyHP(int amount)
    {
        MaxHP = Mathf.Clamp(MaxHP + amount, 1, 999); // 注意最少为 1
        CurHP = MaxHP;
        OnTakeDamage?.Invoke();
    }
    DamageResult DealDamage(int damage)
    {
        if (IsDead)
            return new DamageResult(0, 0, 0, true, -1);
        int overflow = Mathf.Max(0, damage - CurHP);
        int effective = damage - overflow;
        CurHP = Mathf.Clamp(CurHP - damage, 0, MaxHP);
        EState = IsDead ? EnemyState.dead : EnemyState.hit;
        OnTakeDamage?.Invoke();
        return new DamageResult(damage, effective, overflow, IsDead, -1);
    }
    #endregion
}

[Serializable]
public class ShieldConfigData
{
    public List<int> ShieldsHPs = new List<int>();

    public List<ShieldData> ToData()
    {
        List<ShieldData> curShields = new List<ShieldData>();
        for (int i = ShieldsHPs.Count - 1; i >= 0; i--)
            curShields.Add(new ShieldData(ShieldsHPs[i], i));
        return curShields;
    }
}
