using System;
using System.Collections.Generic;
using UnityEngine;

public class ShieldData
{
    public int MaxHP { get; private set; }
    public int CurHP { get; private set; }
    public EnemyState EState;
    public int ShieldIndex { get; private set; }
    public bool IsDestroyed => CurHP <= 0;
    public event Action OnTakeDamage;
    public ShieldData(int maxHP, int index)
    {
        MaxHP = maxHP;
        CurHP = maxHP;
        ShieldIndex = index;
    }
    public void TakeDamage(int damage)
    {
        CurHP = Mathf.Clamp(CurHP - damage, 0, MaxHP);
        OnTakeDamage?.Invoke();
    }
}

[Serializable]
public class ShieldConfigData
{
    public List<int> ShieldsHPs = new List<int>();

    public List<ShieldData> ToData()
    {
        List<ShieldData> curShields = new List<ShieldData>();
        for (int i = 0; i < ShieldsHPs.Count; i++)
            curShields.Add(new ShieldData(ShieldsHPs[i], i));
        return curShields;
    }
}
