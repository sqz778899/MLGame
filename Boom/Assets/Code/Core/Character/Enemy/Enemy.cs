using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using UnityEngine.Serialization;

public class Enemy : EnemyBase
{
    public HealthBar CurHealthBar;

    [Header("盾牌")]
    public List<int> ShieldsHPs = new List<int>();
    public List<ShieldMono> Shields;

    [Header("Node相关")]
    public GameObject ShieldsNode;
   
    [Header("SpineAbout")]
    public SkeletonAnimation Ani;
    public Sprite Portrait;
    
    [Header("Award")]
    //Award...........
    public Award CurAward;
    
    public Action OnLoadData;
    BattleLogic _battleLogic;

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
        if (_battleLogic==null)
            _battleLogic = BattleManager.Instance.battleLogic;
        CurHealthBar.InitHealthBar(this); //初始化血条
        SetShields();//初始化盾牌
        Portrait = ResManager.instance.GetAssetCache<Sprite>(PathConfig.GetEnemyPortrait(ID));
        //初始化Spine资产
        SkeletonDataAsset curSkeletonDataAsset = ResManager.instance.
            GetAssetCache<SkeletonDataAsset>(PathConfig.GetEnemySkelentonDataPath(ID));
        Ani.skeletonDataAsset = curSkeletonDataAsset;
        Ani.Initialize(true);
        OnLoadData?.Invoke();//绑定MiniMap中的UI显示敌人状态
    }

    //设置盾牌数量和每个盾牌的血量
    public void SetShields()
    {
        Shields = new List<ShieldMono>();
        for (int i = ShieldsNode.transform.childCount-1; i >=0; i--)
            Destroy(ShieldsNode.transform.GetChild(i).gameObject);
        
        for (int i = 0; i < ShieldsHPs.Count; i++)
        {
            GameObject ShieldIns = ResManager.instance.CreatInstance(PathConfig.ShieldPB);
            ShieldMono curMono = ShieldIns.GetComponent<ShieldMono>();
            curMono.ShieldIndex = i;
            curMono.InitShield(ShieldsHPs[i]);
            curMono.HitColor = HitColor;
            curMono.HitTextRoot = HitTextRoot;
            ShieldIns.transform.SetParent(ShieldsNode.transform,false);
            float curStep = i * curMono.InsStep;
            ShieldIns.transform.localPosition = new Vector3(curStep,0,0);
            Shields.Add(curMono);
        }
    }
    
    //伤害
    public override void TakeDamage(BulletInner CurBullet,int damage)
    {
        base.TakeDamage(CurBullet,damage);
        //战场信息收集
        int OverflowDamage = 0;
        if (damage - CurHP > 0)
            OverflowDamage = damage - CurHP;
        int EffectiveDamage = damage - OverflowDamage;
       
        CurHP -= damage;
        OnTakeDamage?.Invoke();//MiniMap中的UI显示敌人状态
        CurBullet.BattleOnceHits.Add(new BattleOnceHit(CurBullet._data.CurSlotController.SlotID,
            -1,1,EffectiveDamage,OverflowDamage,damage,CurHP<=0));
        
        float hitTime = 0f;
        AniUtility.PlayHit01(Ani, ref hitTime);
        
        if (CurHP <= 0)
        {
            CurHP = 0;
            StopAllCoroutines();
            EState = EnemyState.dead;
        }
        else
            StartCoroutine(ChangeHitState(hitTime)); // 伤害后切换回 idle 状态
    }

    //爆装备了
    public void GetAward()
    {
    }
    #endregion
    
    #region 中间数据相关
    public EnemyMiddleData ToMiddleData() => new (ID,MaxHP, ShieldsHPs,CurAward);

    public void LoadMiddleData(EnemyMiddleData _enemyMidData)
    {
        ID = _enemyMidData.ID;
        MaxHP = _enemyMidData.HP;
        ShieldsHPs.Clear();
        ShieldsHPs.AddRange(_enemyMidData.ShieldsHPs);
        CurAward = _enemyMidData.CurAward;
    }

    internal override void OnDestroy()
    {
        base.OnDestroy();
        OnLoadData = null;
    }
    #endregion
    
    #region 表现相关
    //等待播放Hit动画，没死就切一下Idle状态
    public IEnumerator ChangeHitState(float hitTime)
    {
        yield return new WaitForSeconds(hitTime);
        EState = EnemyState.live;
    }
    #endregion
}