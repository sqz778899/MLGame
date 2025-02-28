using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldMono : EnemyBase
{
    public HealthBar CurHealthBar;
    public float InsStep; //创建实例的间隔
    
    public void InitShield(int maxHP)
    {
        CurHP = MaxHP = maxHP;
        EState = EnemyState.live;
        CurHealthBar.InitHealthBar(this); //初始化血条
    }
    //处理盾牌受到伤害
    public override void TakeDamage(BulletInner bullet,int damage)
    {
        base.TakeDamage(bullet,damage);
        if (CurHP <= 0)
            Destroy(gameObject);
    }
}