using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CalculateDamageManager : ScriptableObject
{
    #region 单例
    static CalculateDamageManager s_instance;
    
    public static CalculateDamageManager Instance
    {
        get
        {
            if (s_instance == null)
                s_instance = ResManager.instance.GetAssetCache<CalculateDamageManager>(PathConfig.CalculateDamageManagerOBJ);
            return s_instance;
        }
    }
    #endregion
    
    public void CalDamage(BulletData bullet,EnemyBase curEnemy)
    {
        
    }

    public void CalDamage(BulletInner bullet,EnemyBase curEnemy)
    {
        int curDamage = bullet._data.FinalDamage;
        switch (bullet._data.ElementalType)
        {
            case ElementalTypes.Non:
                curEnemy.TakeDamage(bullet,curDamage);
                break;
            case ElementalTypes.Ice:
                CalIce(bullet, curEnemy);
                break;
            case ElementalTypes.Fire:
                CalFire(bullet, curEnemy);
                break;
            case ElementalTypes.Electric:
                CalElectric(bullet, curEnemy);
                break;
        }
    }

    void CalIce(BulletInner bullet,EnemyBase curEnemy)
    {
        int curDamage = bullet._data.Damage;
        DamageState curDamageState = curEnemy.DState;
        
        //冰遇见火，会极度爆炸，造成大量伤害并且属性积蓄归零
        if (curDamageState.Fire != 0)
        {
            int curFire = curDamageState.Fire;
            int resultDamage = curFire + curDamage * 2;
            curDamageState.Fire = 0;
            curEnemy.TakeDamage(bullet,resultDamage);
            return;
        }
        
        curDamageState.Ice += curDamage;
    }

    void CalFire(BulletInner bullet, EnemyBase curEnemy)
    {
        int curDamage = bullet._data.Damage;
        DamageState curDamageState = curEnemy.DState;
        
        //火遇见冰，会冷却 todo Debuff
        if (curDamageState.Ice != 0)
        {
            if (curDamageState.Ice >= curDamage)
                curDamageState.Ice -= curDamage;
            else
            {
                curDamageState.Fire += (curDamage - curDamageState.Ice);
                curDamageState.Ice = 0;
            }
            return;
        }
        //雷火无妄
        if (curDamageState.Electric != 0)
        {
            if (curDamageState.Electric >= 5 &&
                (curDamage + curDamageState.Fire) >= 5)
            {
                int resultDamage = (curDamage + curDamageState.Fire + curDamageState.Electric) * 2;
                curEnemy.TakeDamage(bullet,resultDamage);
                return;
            }
        }
      
        curDamageState.Fire += curDamage;
    }

    void CalElectric(BulletInner bullet, EnemyBase curEnemy)
    {
        int curDamage = bullet._data.Damage;
        DamageState curDamageState = curEnemy.DState;
        int curElectric = curDamageState.Electric;
        
        if (curDamageState.Ice != 0)
        {
            int resultDamage = curElectric + curDamage;
            curDamageState.Electric += curDamage;
            curEnemy.TakeDamage(bullet,resultDamage);
            return;
        }

        if (curDamageState.Fire != 0)
        {
            //雷火无妄
            if (curDamageState.Fire >= 5 &&
                (curDamage + curElectric) >= 5)
            {
                int resultDamage = (curDamage + curElectric + curDamageState.Fire) * 2;
                curEnemy.TakeDamage(bullet,resultDamage);
                return;
            }
        }
        
        curDamageState.Electric += curDamage;
    }
}
