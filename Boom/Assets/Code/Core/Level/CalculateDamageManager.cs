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

    public void CalDamage(BulletData curBulletData,Enemy curEnemy)
    {
        int curDamage = curBulletData.damage;
        int curHealth = curEnemy.health;
        switch (curBulletData.elementalType)
        {
            case ElementalTypes.NonElemental:
                curEnemy.health = curHealth - curDamage;
                curEnemy.TakeDamage(curDamage);
                break;
            case ElementalTypes.Ice:
                CalIce(curBulletData, curEnemy);
                break;
            case ElementalTypes.Fire:
                CalFire(curBulletData, curEnemy);
                break;
            case ElementalTypes.Electric:
                CalElectric(curBulletData, curEnemy);
                break;
        }
    }

    void CalIce(BulletData curBulletData,Enemy curEnemy)
    {
        int curDamage = curBulletData.damage;
        EnemyState curEnemyState = curEnemy.state;
        
        //冰遇见火，会极度爆炸，造成大量伤害并且属性积蓄归零
        if (curEnemyState.Fire != 0)
        {
            int curFire = curEnemyState.Fire;
            int resultDamage = curFire + curDamage * 2;
            curEnemyState.Fire = 0;
            curEnemy.health -= resultDamage;
            curEnemy.TakeDamage(resultDamage);
            return;
        }
        
        curEnemyState.Ice += curDamage;
    }

    void CalFire(BulletData curBulletData, Enemy curEnemy)
    {
        int curDamage = curBulletData.damage;
        EnemyState curEnemyState = curEnemy.state;
        
        //火遇见冰，会冷却 todo Debuff
        if (curEnemyState.Ice != 0)
        {
            if (curEnemyState.Ice >= curDamage)
                curEnemyState.Ice -= curDamage;
            else
            {
                curEnemyState.Fire += (curDamage - curEnemyState.Ice);
                curEnemyState.Ice = 0;
            }
            return;
        }
        //雷火无妄
        if (curEnemyState.Electric != 0)
        {
            if (curEnemyState.Electric >= 5 &&
                (curDamage + curEnemyState.Fire) >= 5)
            {
                int resultDamage = (curDamage + curEnemyState.Fire + curEnemyState.Electric) * 2;
                curEnemy.health -= resultDamage;
                curEnemy.TakeDamage(resultDamage);
                return;
            }
        }
      
        curEnemyState.Fire += curDamage;
    }

    void CalElectric(BulletData curBulletData, Enemy curEnemy)
    {
        int curDamage = curBulletData.damage;
        EnemyState curEnemyState = curEnemy.state;
        int curElectric = curEnemyState.Electric;
        
        if (curEnemyState.Ice != 0)
        {
            int resultDamage = curElectric + curDamage;
            curEnemyState.Electric += curDamage;
            curEnemy.health -= resultDamage;
            curEnemy.TakeDamage(resultDamage);
            return;
        }

        if (curEnemyState.Fire != 0)
        {
            //雷火无妄
            if (curEnemyState.Fire >= 5 &&
                (curDamage + curElectric) >= 5)
            {
                int resultDamage = (curDamage + curElectric + curEnemyState.Fire) * 2;
                curEnemy.health -= resultDamage;
                curEnemy.TakeDamage(resultDamage);
                return;
            }
        }
        
        curEnemyState.Electric += curDamage;
    }
}
