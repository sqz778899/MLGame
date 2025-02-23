using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Spine.Unity;

public class Enemy : EnemyBase
{
    public HealthBar CurHealthBar;

    [Header("盾牌")]
    public List<int> ShieldsHPs = new List<int>();

    [Header("Node相关")]
    public GameObject ShieldsNode;
   
    [Header("SpineAbout")]
    public SkeletonAnimation Ani;
    
    [Header("Award")]
    //Award...........
    public Award award;
    
    FightLogic _fightLogic;

    void Start()
    {
        InitData();
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

    #region 主要逻辑
    public void InitData()
    {
        CurHP = MaxHP;
        EState = EnemyState.live;
        if (_fightLogic==null)
            _fightLogic = UIManager.Instance.FightLogicGO.GetComponent<FightLogic>();
        CurHealthBar.InitHealthBar(this); //初始化血条
        SetShields();//初始化盾牌

        SkeletonDataAsset curSkeletonDataAsset = ResManager.instance.
            GetAssetCache<SkeletonDataAsset>(PathConfig.GetEnemySkelentonDataPath(ID));
        Ani.skeletonDataAsset = curSkeletonDataAsset;
        Ani.Initialize(true);
    }

    //设置盾牌数量和每个盾牌的血量
    public void SetShields()
    {
        for (int i = ShieldsNode.transform.childCount-1; i >=0; i--)
            DestroyImmediate(ShieldsNode.transform.GetChild(i).gameObject);
        
        for (int i = 0; i < ShieldsHPs.Count; i++)
        {
            GameObject ShieldIns = ResManager.instance.CreatInstance(PathConfig.ShieldPB);
            ShieldMono curMono = ShieldIns.GetComponent<ShieldMono>();
            curMono.InitShield(ShieldsHPs[i]);
            ShieldIns.transform.SetParent(ShieldsNode.transform,false);
            float curStep = i * curMono.InsStep;
            ShieldIns.transform.localPosition = new Vector3(curStep,0,0);
        }
    }
    
    //伤害
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        float hitTime = 0f;
        AniUtility.PlayHit01(Ani, ref hitTime);

        if (CurHP <= 0)
        {
            StopAllCoroutines();
            EState = EnemyState.dead;
            StartCoroutine(ChangeCalculation());
        }
        else
            StartCoroutine(ChangeHitState(hitTime)); // 伤害后切换回 idle 状态
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
    #endregion
    
    #region 中间数据相关
    public EnemyMiddleData ToMiddleData() => new (ID,MaxHP, ShieldsHPs);

    public void LoadMiddleData(EnemyMiddleData _enemyMidData)
    {
        ID = _enemyMidData.ID;
        MaxHP = _enemyMidData.HP;
        ShieldsHPs.Clear();
        ShieldsHPs.AddRange(_enemyMidData.ShieldsHPs);
    }
    #endregion
    
    #region 表现相关
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

    //伤害跳字
    void HitText(int damage)
    {
        GameObject txtHitIns = Instantiate(ResManager.instance
            .GetAssetCache<GameObject>(PathConfig.TxtHitPB),txtHitNode.transform);
        txtHitIns.GetComponent<TextMeshPro>().text = "-" + damage;
        Animation curAni = txtHitIns.GetComponent<Animation>();
        curAni.Play();
    }
    #endregion
}