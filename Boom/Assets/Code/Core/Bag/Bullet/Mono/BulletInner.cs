using System;
using System.Collections;
using Spine.Unity;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BulletInner:ItemBase
{
    public BulletData _data; //绝对核心数据
    public RoleInner CurRole; //当前持有的角色

    [Header("表现资产")] 
    public TextMeshPro txtDamage;
    public Renderer CurRenderer;
    public SkeletonAnimation Skeleton;
    public GameObject HitEffect; // 击中效果预制体
    public SkeletonDataAsset HitSpfxAsset; // 子弹击中效果子弹Spine资产资产
    
    internal Vector3 forward = new Vector3(1, 0, 0);
    List<Material> _materials;
    
    [Header("重要属性")]
    public BulletInnerState _state;
    public List<BattleOnceHit> BattleOnceHits = new List<BattleOnceHit>();
    public Rigidbody2D rb;
    public float RunSpeed = 10.0f;
    public float FollowDis = 3;
    public float AniScale = 1f;
    
    int _piercingCount; //穿透的敌人的数量
    int _resonance;
    public float CurSpeed;

    void Start()
    {
        _piercingCount = 0;
        _resonance = 0;
        _state = BulletInnerState.Common;
        _materials = new List<Material>();
        CurSpeed = 60f;
        foreach (var each in CurRenderer.materials)
        {
            Material newMat = new Material(each);
            _materials.Add(newMat);
        }
        CurRenderer.materials = _materials.ToArray();
        
        AniUtility.PlayIdle(Skeleton,AniScale);
        foreach (Material material in _materials)
            material.SetFloat("_Transparency", 1);

        StartCoroutine(InitSkeleton());
    }
    
    IEnumerator InitSkeleton()
    {
        // 等待 1 帧，确保 Spine 所有依赖生命周期跑完
        yield return null;
        Skeleton.Initialize(true);
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
            case BulletInnerState.Dead:
                Destroy(gameObject);
                break;
        }
    }
    
    void FixedUpdate()
    {
        if (_state == BulletInnerState.Attacking)// 让子弹沿着Z轴向前移动
        {
            Vector2 move = forward * CurSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + move);
        }
    }

    public void UpText()
    {
        Vector3 tmpPos = new Vector3(0,1.2f,0) + transform.position;
        tmpPos.y += 0.4f;
        txtDamage.transform.position = tmpPos;
    }
    
    public void ReturnText() => txtDamage.transform.position = transform.position + new Vector3(0,1.2f,0);

    #region 击中敌人相关
    void OnTriggerEnter2D(Collider2D other)
    {
        // 白名单标签
        string[] whiteListTags = { "Enemy" ,"Shield"};  // 你可以在这里定义多个标签
        
        // 检查触发的对象的标签是否在白名单中
        if (Array.Exists(whiteListTags, tag => other.CompareTag(tag)))
        {
            //穿透敌人的数量
            if (_piercingCount >= _data.FinalPiercing)
                _state = BulletInnerState.Dead;

            HandleEnemyHit(other.GetComponent<EnemyBase>());
            HandleHitEffect();
            if (_state == BulletInnerState.Dead || other.tag=="Enemy")
            {
                //如果是最后一个敌人，子弹消失
                HandleBulletDisappear();
                //战报收集！！传递给WarReport消息。
                WarReport warReport = BattleManager.Instance.battleData.CurWarReport;
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
                s.InfoDict[_data.CurSlotController.SlotID] = new KeyValuePair<BulletData, List<BattleOnceHit>>(_data, BattleOnceHits);
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
        Destroy(gameObject);
    }
    #endregion

    #region 攻击
    public IEnumerator ReadyToAttack(Vector3 targetPos)
    {
        CurRenderer.materials = _materials.ToArray();
        //...............填弹...........
        _state = BulletInnerState.AttackBegin;
        float aniTime = 0f;
        Vector3 curScale = transform.localScale;
        Skeleton.Initialize(true);//不初始化会有BUG，spine的莫名其妙
        AniUtility.PlayAttack(Skeleton,ref aniTime,AniScale);
        transform.DOMove(targetPos, aniTime);
        transform.DOScale(curScale * 0.5f , aniTime);
        StartCoroutine(FadeOut(aniTime));
        //......关闭伤害提示UI
        txtDamage.gameObject.SetActive(false);
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
        curFX.transform.SetParent(UIManager.Instance.Logic.MapManagerSC.MapBuleltRoot.transform,false);
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
    
    float CurDistance() =>transform.position.x - PlayerManager.Instance.RoleInFightGO.transform.position.x;
    #endregion

    #region 数据绑定相关
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
        //绑定战场数据
        CurRole = PlayerManager.Instance.RoleInFightSC;
        FollowDis = Mathf.Abs(data.CurSlotController.SlotID  * 1f);
        
        Vector3 startPos = new Vector3(CurRole.transform.position.x - 1, -0.64f, -0.15f);
        float offsetX = startPos.x - (data.CurSlotController.SlotID - 1) * 1f;
        transform.position = new Vector3(offsetX,startPos.y,startPos.z + (data.CurSlotController.SlotID - 1));
        transform.SetParent(UIManager.Instance.Logic.MapManagerSC.MapBuleltRoot.transform,false);
    }

    void OnDataChangedInner()
    {
        Skeleton.skeletonDataAsset = ResManager.instance.GetAssetCache<SkeletonDataAsset>
            (PathConfig.GetBulletImageOrSpinePath(_data.ID,BulletInsMode.Inner));
        HitEffect = ResManager.instance.GetAssetCache<GameObject>(
            PathConfig.BulletSpfxTemplate);
        HitSpfxAsset = ResManager.instance.GetAssetCache<SkeletonDataAsset>
            (PathConfig.GetBulletSpfxPath(_data.ID));
        txtDamage.text = _data.FinalDamage.ToString();
    }
    
    void OnDestroy()
    {
        _data.OnDataChanged -= OnDataChangedInner;
    }
    #endregion
}
