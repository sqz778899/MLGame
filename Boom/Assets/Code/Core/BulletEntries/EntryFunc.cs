using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine.UI;

public static class EntryFunc
{
    public static void InvokeEntry(int ID)
    {
        switch (ID)
        {
            case 1:
                ClayClay();
                break;
        }
    }
    
    static void ClayClay()
    {
        int ID = 1;
        bool IsPassed = true;
        foreach (var each in MainRoleManager.Instance.CurBullets)
        {
            if (each.GetType() != ElementalTypes.Non)
                IsPassed = false;
        }

        if (!IsPassed) //取消Buff
        {
            List<int> NeedDelete = new List<int>();
            //注意从大到小正确排序
            for (int i = MainRoleManager.Instance.CurBulletBuffs.Count - 1; i >= 0; i--)
            {
                if (MainRoleManager.Instance.CurBulletBuffs[i].ID == ID)
                    NeedDelete.Add(i);
            }

            foreach (int each in NeedDelete)
                MainRoleManager.Instance.CurBulletBuffs.RemoveAt(each);
        }
        else//添加Buff
        {
            BulletBuff curBuff = new BulletBuff();
            curBuff.ID = ID;
            Dictionary<int, int> bulletIdToDamage = new Dictionary<int, int>();
            foreach (var each in MainRoleManager.Instance.CurBullets)
                bulletIdToDamage.Add(each.bulletID,1);
            curBuff.bulletIdToDamage = bulletIdToDamage;
            MainRoleManager.Instance.CurBulletBuffs.Add(curBuff);
        }
    }

    static void IceFire()
    {
        int ID = 2;
        bool IsPassed = true;
        
        foreach (var each in MainRoleManager.Instance.CurBullets)
        {
            if (each.GetType() == ElementalTypes.Fire)
            {
                
            }
        }
    }
}