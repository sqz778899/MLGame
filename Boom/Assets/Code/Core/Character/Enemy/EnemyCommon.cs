using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageState
{
    public int Ice;
    public int Fire;
    public int Electric;

    public DamageState()
    {
        Ice = 0;
        Fire = 0;
        Electric = 0;
    }
}

public enum EnemyState
{
    live = 1,
    hit = 2,
    dead = 3
}

public struct EnemyMiddleData
{
    public int ID;
    public int HP;
    public List<int> ShieldsHPs;
    public Award CurAward;
    
    public EnemyMiddleData(int _ID = 1,int _hp = 1, List<int> _shields = null,Award _award = default)
    {
        ID = _ID;
        HP = _hp;
        if (_shields == null)
            _shields = new List<int>();
        ShieldsHPs = _shields;
        CurAward = _award;
    }
}

public enum HealthBarType
{
    Shield = 1,
    Enemy = 2,
}
public class EnemyBase : MonoBehaviour
{
    [Header("基础属性")] 
    public int ID;
    int curHP;
    public int CurHP
    {
        get => curHP;
        set
        {
            curHP = Mathf.Clamp(value, 0, MaxHP);  // 防止当前血量大于最大血量或者小于0
        }
    }

    public int MaxHP;
    
    [Header("功能相关")]
    public EnemyState EState;
    public DamageState DState = new DamageState();
    
    [Header("Node相关")]
    public GameObject txtHitNode;
    
    //伤害
    public virtual void TakeDamage(BulletInner curBullet,int damage)
    {
        if (EState == EnemyState.dead) return; // 防止重复触发死亡状态
        
        EState = EnemyState.hit;
        //伤害跳字
        HitText(damage);
    }
    
    
    //伤害跳字
    void HitText(int damage)
    {
        GameObject txtHitIns = Instantiate(ResManager.instance
            .GetAssetCache<GameObject>(PathConfig.TxtHitPB),txtHitNode.transform);
        txtHitIns.GetComponent<TextMeshPro>().text = "-" + damage;
        Animation curAni = txtHitIns.GetComponent<Animation>();
        curAni.Play();
    }
}