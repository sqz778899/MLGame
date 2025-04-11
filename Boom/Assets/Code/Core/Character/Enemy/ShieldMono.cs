using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldMono : EnemyBase
{
    public int ShieldIndex;
    public HealthBar CurHealthBar;
    public float InsStep; //创建实例的间隔
    
    public void InitShield(int maxHP)
    {
        CurHP = MaxHP = maxHP;
        EState = EnemyState.live;
        CurHealthBar.InitHealthBar(this); //初始化血条
    }
    //处理盾牌受到伤害
    public override void TakeDamage(BulletInner CurBullet,int damage)
    {
        base.TakeDamage(CurBullet,damage);
        //战场信息收集
        int OverflowDamage = 0;
        if (damage - CurHP > 0)
            OverflowDamage = damage - CurHP;
        int EffectiveDamage = damage - OverflowDamage;
       
        CurHP -= damage;
        OnTakeDamage?.Invoke();
        CurBullet.BattleOnceHits.Add(new BattleOnceHit(CurBullet._data.CurSlotController.SlotID,
            ShieldIndex,-1,EffectiveDamage,OverflowDamage,damage,CurHP<=0));
        
        if (CurHP <= 0)
            Destroy(gameObject);
    }
}