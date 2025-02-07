using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Spine.Unity;

public class Enemy : MonoBehaviour
{
    public int MaxHP = 3;
    public int CurHP = 3;
    //功能相关
    public EnemyState EState;
    public DamageState DState;
    bool isDying = false;
    //表现相关
    public GameObject txtHitNode;
    [Header("SpineAbout")]
    public SkeletonAnimation Ani;
    
    [Header("Award")]
    //Award...........
    public Award award;
    
    FightLogic _fightLogic;

    void Start()
    {
        EState = EnemyState.live;
        InitState(null);
        if (_fightLogic==null)
            _fightLogic = UIManager.Instance.FightLogicGO.GetComponent<FightLogic>();
    }

    void Update()
    {
        switch (EState)
        {
            case EnemyState.live:
                if (CurHP == MaxHP)
                    AniUtility.PlayIdle(Ani);
                else
                    AniUtility.PlayIdle01(Ani);
                break;
            case EnemyState.hit:
                break;
            case EnemyState.dead:
                AniUtility.PlayDead01(Ani);
                break;
        }
    }

    public void InitState(DamageState curState)
    {
        if (curState!=null)
            DState = curState;
        else
            DState = new DamageState();
    }

    public void TakeDamage(int damage)
    {
        if (EState == EnemyState.dead) return; // 防止重复触发死亡状态
        
        EState = EnemyState.hit;
        //伤害跳字
        HitText(damage);
        CurHP -= damage;
        float hitTime = 0f;
        AniUtility.PlayHit01(Ani,ref hitTime); //播放受击动画
        
        if (CurHP <= 0)// 如果血量为0或更少，则开始死亡结算
        {
            StopAllCoroutines();
            EState = EnemyState.dead;
            StartCoroutine(ChangeCalculation());
        }
        else
            StartCoroutine(ChangeHitState(hitTime)); //没死等一下播放完动画，切一下Idle状态
    }

    //爆装备了
    public void GetAward()
    {
        foreach (var each in award.Items)
        {
            //1）添加Item到数据&&GO层
            MainRoleManager.Instance.AddItem(each);
            //2) 展示Item
            //ItemManager.InstanceItemByID();
        }
    }
    
    //等待播放Hit动画，没死就切一下Idle状态
    public IEnumerator ChangeHitState(float hitTime)
    {
        yield return new WaitForSeconds(hitTime);
        EState = EnemyState.live;
    }
    
    //等待结算时间，时间到之后开启结算。。。。。
    public IEnumerator ChangeCalculation()
    {
        yield return new WaitForSeconds(_fightLogic.waitCalculateTime);
        MainRoleManager.Instance.Score += award.score;
        _fightLogic.isBeginCalculation = true; //通知进行结算
    }

    void HitText(int damage)
    {
        GameObject txtHitIns = Instantiate(ResManager.instance
            .GetAssetCache<GameObject>(PathConfig.TxtHitPB),txtHitNode.transform);
        txtHitIns.GetComponent<TextMeshPro>().text = "-" + damage;
        Animation curAni = txtHitIns.GetComponent<Animation>();
        curAni.Play();
    }
}