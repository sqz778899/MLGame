using System;
using System.Collections;
using Spine.Unity;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BulletInner:ItemBase
{
    public BulletData _data; //绝对核心数据
    
    [Header("表现资产")]
    public Renderer CurRenderer;
    public SkeletonAnimation Skeleton;
    public GameObject HitEffect; // 击中效果预制体
    public SkeletonDataAsset HitSpfxAsset; // 子弹击中效果子弹Spine资产资产
    
    internal Vector3 forward = new Vector3(1, 0, 0);
    List<Material> _materials;
    
    [Header("重要属性")]
    public BulletInnerState _state;
    public List<BattleOnceHit> BattleOnceHits = new List<BattleOnceHit>();
    public float RunSpeed = 10.0f;
    public float FollowDis = 3;
    public float AniScale = 1f;
    
    int _piercingCount; //穿透的敌人的数量
    int _resonance;
    public float CurSpeed;
    
    
    public void BindData(BulletData data)
    {
        if (_data != null)
            _data.OnDataChanged -= OnDataChangedInner; // 先退订旧Data的事件
        
        _data = data;
        if (_data != null)
        {
            _data.OnDataChanged += OnDataChangedInner;
            OnDataChangedInner(); // 立即刷新一遍
        }
    }
    
    void OnDataChangedInner()
    {
        Skeleton.skeletonDataAsset = ResManager.instance.GetAssetCache<SkeletonDataAsset>
            (PathConfig.GetBulletImageOrSpinePath(_data.ID,BulletInsMode.Inner));
        Skeleton.Initialize(true);
        HitEffect = ResManager.instance.GetAssetCache<GameObject>(
            PathConfig.BulletSpfxTemplate);
        HitSpfxAsset = ResManager.instance.GetAssetCache<SkeletonDataAsset>
            (PathConfig.GetBulletSpfxPath(_data.ID));
    }
    

    void Start()
    {
        _piercingCount = 0;
        _resonance = 0;
        _state = BulletInnerState.Common;
        /*_materials = new List<Material>();
        foreach (var each in _ain.skeletonDataAsset.atlasAssets[0].Materials)
        {
            Material newMat = new Material(each);
            _materials.Add(newMat);
        }
        _ain.skeletonDataAsset.atlasAssets[0].Materials = _materials;*/
        _materials = new List<Material>(Skeleton.skeletonDataAsset.atlasAssets[0].Materials);
        
        AniUtility.PlayIdle(Skeleton,AniScale);
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
            case BulletInnerState.Edit:
                break;
            case BulletInnerState.AttackBegin:
                break;
            case BulletInnerState.Attacking:// 让子弹沿着Z轴向前移动
                CurSpeed = 60f;
                transform.Translate(forward * CurSpeed * Time.deltaTime);
                break;
            case BulletInnerState.Dead:
                //CurRoleInner.Bullets.Remove(this);
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
            if (_piercingCount >= _data.FinalPiercing)
                _state = BulletInnerState.Dead;

            HandleEnemyHit(other.GetComponent<EnemyBase>());
            HandleHitEffect();
            if (_state == BulletInnerState.Dead)
            {
                //如果是最后一个敌人，子弹消失
                HandleBulletDisappear();
                //传递给WarReport消息。
                WarReport warReport = MainRoleManager.Instance.CurWarReport;
                int curWarIndex = warReport.CurWarIndex;
                
                if (!warReport.WarIndexToBattleInfo.TryGetValue(curWarIndex, out SingelBattleInfo s))
                {
                    s = new SingelBattleInfo();
                    warReport.WarIndexToBattleInfo[curWarIndex] = s;
                }
                
                var dic = new Dictionary<BulletData, List<BattleOnceHit>>
                {
                    [_data] = BattleOnceHits
                };
                s.InfoDict[_data.CurSlot.SlotID] = new KeyValuePair<BulletData, List<BattleOnceHit>>(_data, BattleOnceHits);
            }
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
    public IEnumerator ReadyToAttack(Vector3 targetPos)
    {
        //...............填弹...........
        _state = BulletInnerState.AttackBegin;
        float aniTime = 0f;
        Vector3 curScale = transform.localScale;
        AniUtility.PlayAttack(Skeleton,ref aniTime,AniScale);
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
        //StopAllCoroutines();
        _state = BulletInnerState.Attacking;
        foreach (Material material in _materials)
            material.SetFloat("_Transparency", 1);
        AniUtility.PlayAttacking(Skeleton,AniScale);
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
            AniUtility.TrunAround(Skeleton, 1); // 面朝左
        else
            AniUtility.TrunAround(Skeleton, -1); // 面朝右
        
        //..............移动方向...................
        if (shouldMove)
        {
            AniUtility.PlayRun(Skeleton, AniScale);
            float moveDistance = RunSpeed * Time.deltaTime;
            transform.Translate(forward * (dis < 0 ? moveDistance : -moveDistance));
        }
        else
            AniUtility.PlayIdle(Skeleton, AniScale);
    }
    
    float CurDistance()
    {
        return transform.position.x - UIManager.Instance.RoleIns.transform.position.x;
    }
    #endregion
}
