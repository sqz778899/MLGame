using UnityEngine;
using System;
using System.Collections.Generic;

public static class EntryFunc
{
    public static void InvokeEntry(int ID)
    {
        switch (ID)
        {
            case 1:
                ClayClay();
                break;
            case 2:
                IceFire();
                break;
            case 3:
                UniqueBullet();
                break;
            case 4:
                ClayMaster();
                break;
        }
    }
    
    //黏土黏土
    static void ClayClay()
    {
        int ID = 1;
        
        MainRoleManager.Instance.RemoveBulletBuff(ID);
        bool IsPassed = true;
        foreach (var each in MainRoleManager.Instance.CurBullets)
        {
            if (each.ElementalType != (int)ElementalTypes.Non)
                IsPassed = false;
        }

        if (!IsPassed)
            return;
        
        BulletBuff curBuff = new BulletBuff();
        curBuff.ID = ID;
        Dictionary<int, int> bulletIdToDamage = new Dictionary<int, int>();
        
        HashSet<int> set = new HashSet<int>();
        foreach (var each in MainRoleManager.Instance.CurBullets)
            set.Add(each.ID);

        foreach (var each in set)
            bulletIdToDamage.Add(each,1);
      
        curBuff.bulletIdToDamage = bulletIdToDamage;
        MainRoleManager.Instance.CurBulletBuffs.Add(curBuff);
    }

    //冰火共振
    static void IceFire()
    {
        int ID = 2;
        
        MainRoleManager.Instance.RemoveBulletBuff(ID);
        List<BulletJson> CurBullets = MainRoleManager.Instance.CurBullets;
        for (int i = 0; i < CurBullets.Count; i++)
        {
            if (CurBullets.Count > i + 1)
            {
                if (CurBullets[i].ElementalType == (int)ElementalTypes.Ice &&
                    CurBullets[i + 1].ElementalType == (int)ElementalTypes.Fire)
                {
                    BulletBuff curBuff = new BulletBuff();
                    BulletJson curBullet = CurBullets[i];
                    BulletJson nextBullet = CurBullets[i + 1];
                    int allDamage = curBullet.Damage + nextBullet.Damage;
                    curBuff.indexToSettleDamage.Add(i + 1,allDamage);
                }
            }
        }
    }

    //特立独行
    static void UniqueBullet()
    {
        int ID = 3;

        MainRoleManager.Instance.RemoveBulletBuff(ID);
        bool IsPassed = true;
        List<int> numbers = new List<int>();
        List<BulletJson> CurBullets = MainRoleManager.Instance.CurBullets;
        for (int i = 0; i < CurBullets.Count; i++)
            numbers.Add(CurBullets[i].ID);
      
        HashSet<int> set = new HashSet<int>();
        foreach (var num in numbers)
        {
            if (!set.Add(num))
                IsPassed = false;
        }

        if (IsPassed)
        {
            BulletBuff curBuff = new BulletBuff();
            curBuff.ID = ID;
            Dictionary<int, int> bulletIdToDamage = new Dictionary<int, int>();
            foreach (var each in MainRoleManager.Instance.CurBullets)
                bulletIdToDamage.Add(each.ID,5);
            curBuff.bulletIdToDamage = bulletIdToDamage;
            MainRoleManager.Instance.CurBulletBuffs.Add(curBuff);
        }
    }
    
    //黏土大师
    static void ClayMaster()
    {
        int ID = 4;
        MainRoleManager.Instance.RemoveBulletBuff(ID);
        
        bool IsPassed = false;
        int buffLevel = 0;
        List<BulletJson> CurBullets = MainRoleManager.Instance.CurBullets;
        for (int i = 0; i < CurBullets.Count; i++)
        {
            BulletJson curData = CurBullets[i];
            if (curData.ElementalType == (int)ElementalTypes.Non)
            {
                if (curData.Level == 2)
                {
                    IsPassed = true;
                    buffLevel = 2;
                }
                if (curData.Level == 3)
                {
                    IsPassed = true;
                    buffLevel = 3;
                }
            }
        }

        if (!IsPassed)
            return;

        BulletBuff curBuff = new BulletBuff();
        curBuff.ID = ID;
        HashSet<int> set = new HashSet<int>();
        for (int i = 0; i < CurBullets.Count; i++)
        {
            if (CurBullets[i].ElementalType == (int)ElementalTypes.Non)
                set.Add(CurBullets[i].ID);
        }
        if (buffLevel == 2)
        {
            foreach (var each in set)
                curBuff.bulletIdToDamage.Add(each,3);
        }
        if (buffLevel == 3)
        {
            foreach (var each in set)
                curBuff.bulletIdToDamage.Add(each,10);
        }
    }
}