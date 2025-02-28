using System;
using System.Collections;
using Spine.Unity;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletInner : Bullet
{
    public int BattleOrder = -1;
    public float FollowDis = 3;
    public float RunSpeed = 10.0f;
    
    public BulletInnerState _state;
    
    SkeletonAnimation _ain;
    List<Material> _materials;
    public float AniScale = 1f;

    [Header("重要属性")]
    public RoleInner CurRoleInner;
    int _piercingCount; //穿透的敌人的数量
    int _resonance;
    float _curSpeed;

    internal override void Start()
    {
        base.Start();
        _piercingCount = 0;
        _resonance = 0;
        _state = BulletInnerState.Common;
        _ain = transform.GetChild(0).GetComponent<SkeletonAnimation>();
        _materials = new List<Material>(_ain.skeletonDataAsset.atlasAssets[0].Materials);
        AniUtility.PlayIdle(_ain,AniScale);
        foreach (Material material in _materials)
            material.SetFloat("_Transparency", 1);
    }

    void Update()
    {
        switch (_state)
        {
            case BulletInnerState.Common:
                Run();//在界面跟着主角跑
                break;
            case BulletInnerState.AttackBegin:
                break;
            case BulletInnerState.Attacking:// 让子弹沿着Z轴向前移动
                AccelerateMove();
                break;
            case BulletInnerState.Dead:
                CurRoleInner.Bullets.Remove(this);
                Destroy(gameObject);
                break;
        }
    }

    #region 击中敌人相关
    void OnTriggerEnter2D(Collider2D other)
    {
        // 白名单标签
        string[] whiteListTags = { "Enemy" };  // 你可以在这里定义多个标签
        
        // 检查触发的对象的标签是否在白名单中
        if (Array.Exists(whiteListTags, tag => other.CompareTag(tag)))
        {
            //穿透敌人的数量
            if (_piercingCount >= FinalPiercing)
                _state = BulletInnerState.Dead;

            HandleEnemyHit(other.GetComponent<EnemyBase>());
            HandleHitEffect();
            HandleBulletDisappear();
            _piercingCount++;
        }
    }
    
    //击中敌人
    void HandleEnemyHit(EnemyBase enemy)
    {
        if (enemy != null)
            CalculateDamageManager.Instance.CalDamage(this, enemy);
    }
    
    void HandleHitEffect()
    {
        if (HitEffect != null)
            StartCoroutine(PlayHitFX());
    }
    
    void HandleBulletDisappear()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
    #endregion

    #region 攻击
    void AccelerateMove()
    {
        _curSpeed = Mathf.Lerp(_curSpeed, 100f, Time.deltaTime * 0.5f);
        transform.Translate(forward * _curSpeed * Time.deltaTime);
    }
    public IEnumerator ReadyToAttack(Vector3 targetPos)
    {
        //...............填弹...........
        _state = BulletInnerState.AttackBegin;
        float aniTime = 0f;
        Vector3 curScale = transform.localScale;
        AniUtility.PlayAttack(_ain,ref aniTime,AniScale);
        transform.DOMove(targetPos, aniTime);
        transform.DOScale(curScale * 0.5f , aniTime);
        StartCoroutine(FadeOut(aniTime));
        
        float elapsed = 0f;
        while (elapsed < 10f && _state == BulletInnerState.AttackBegin)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
    
    // 渐隐效果
    IEnumerator FadeOut(float duration)
    {
        // 开始渐隐效果
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float alpha = 1 - t / duration;

            // 修改每个材质的透明度
            foreach (Material material in _materials)
            {
                if (material == null)
                    continue;
                material.SetFloat("_Transparency", alpha);
            }

            yield return null;
        }
        gameObject.SetActive(false);
        // 确保透明度为0
        foreach (Material material in _materials)
            material.SetFloat("_Transparency", 0);
    }

    public void Attack()
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        _state = BulletInnerState.Attacking;
        foreach (Material material in _materials)
            material.SetFloat("_Transparency", 1);
        AniUtility.PlayAttacking(_ain,AniScale);
    }

    public IEnumerator PlayHitFX()
    {
        GameObject curFX = Instantiate(HitEffect, transform.position, transform.rotation);
        curFX.transform.SetParent(UIManager.Instance.G_BulletInScene.transform,false);
        SkeletonAnimation curSpfxSC = curFX.GetComponentInChildren<SkeletonAnimation>();
        curSpfxSC.skeletonDataAsset = HitSpfxAsset;
        curSpfxSC.Initialize(true);
        float aniTime = 0f;
        AniUtility.PlayAttack(curSpfxSC,ref aniTime,AniScale);
        yield return new WaitForSeconds(aniTime);
    }
    #endregion

    #region 跑步相关
    void Run()
    {
        float dis = CurDistance();
        bool shouldMove = Mathf.Abs(dis) > FollowDis;
        
        //..............面向...................
        if (dis < 0)
            AniUtility.TrunAround(_ain, 1); // 面朝左
        else
            AniUtility.TrunAround(_ain, -1); // 面朝右
        
        //..............移动方向...................
        if (shouldMove)
        {
            AniUtility.PlayRun(_ain, AniScale);
            float moveDistance = RunSpeed * Time.deltaTime;
            transform.Translate(forward * (dis < 0 ? moveDistance : -moveDistance));
        }
        else
            AniUtility.PlayIdle(_ain, AniScale);
    }
    
    float CurDistance()
    {
        return transform.position.x - UIManager.Instance.RoleIns.transform.position.x;
    }
    #endregion
}
