using System;
using System.Collections.Generic;


[Serializable]
public class BulletEntry:HaveID
{
    public string Name;
    public string Description;

    public void InvokeEntry()
    {
        EntryFunc.InvokeEntry(ID);
    }
}

public class BulletBuff:HaveID
{
    public Dictionary<int, int> bulletIdToDamage;
    public Dictionary<int, int> indexToSettleDamage;  //结算的伤害

    public BulletBuff()
    {
        bulletIdToDamage = new Dictionary<int, int>();
        indexToSettleDamage = new Dictionary<int, int>();
    }
}