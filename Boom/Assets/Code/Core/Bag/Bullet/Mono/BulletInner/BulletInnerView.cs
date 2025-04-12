using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class BulletInnerView : MonoBehaviour
{
    BulletData _data; //绝对核心数据
    [Header("表现资产")] 
    public TextMeshPro txtDamage;
    public Renderer CurRenderer;
    public SkeletonAnimation Skeleton;
    public GameObject HitEffectPrefab;
    public SkeletonDataAsset HitSpfxAsset; // 子弹击中效果子弹Spine资产资产
    [Header("表现参数")] 
    public float AniScale = 1f;
    
    #region 数据和初始化等等
    void Start()
    {
        //初始化Spine资产
        StartCoroutine(InitSkeleton());
        SetInitialPosition();
        transform.SetParent(UIManager.Instance.Logic.
            MapManagerSC.MapBuleltRoot.transform,false);
    }
    
    void SetInitialPosition()
    {
        if (_data == null) return; // 不做 null check 的话可以加个断言
        RoleInner curRole = PlayerManager.Instance.RoleInFightSC;
        Vector3 startPos = new Vector3(curRole.transform.position.x - 1, -0.64f, -0.15f);
        float offsetX = startPos.x - (_data.CurSlotController.SlotID - 1) * 1f;
        transform.position = new Vector3(offsetX, startPos.y, startPos.z + (_data.CurSlotController.SlotID - 1));
    }
    
    IEnumerator InitSkeleton()
    {
        // 等待 1 帧，确保 Spine 所有依赖生命周期跑完
        yield return null;
        Skeleton.Initialize(true);
    }

    public void BindDataView(BulletData data)
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

    //实时变化View资产
    void OnDataChangedInner()
    {
        Skeleton.skeletonDataAsset = ResManager.instance.GetAssetCache<SkeletonDataAsset>(
            PathConfig.GetBulletImageOrSpinePath(_data.ID, BulletInsMode.Inner));
        HitEffectPrefab = ResManager.instance.GetAssetCache<GameObject>(PathConfig.BulletSpfxTemplate);
        HitSpfxAsset = ResManager.instance.GetAssetCache<SkeletonDataAsset>(PathConfig.GetBulletSpfxPath(_data.ID));
        txtDamage.text = _data.FinalDamage.ToString();//实时变化
    }
    
    public void HandleDisappear() => Destroy(gameObject);
    
    void OnDestroy() => _data.OnDataChanged -= OnDataChangedInner;
    #endregion

    #region 日常状态
    public float GetHorizontalDistance(Vector3 playerPos) => transform.position.x - playerPos.x;
    //朝向
    public void FaceDirection(Vector3 dir) => Skeleton.transform.localScale = new Vector3(dir.x, 1, 1);
    //跑步动画+移动
    public void PlayRunAnimation() => AniUtility.PlayRun(Skeleton);
    public void TranslateHorizontally(float deltaX) => transform.Translate(new Vector3(deltaX, 0, 0));
    //Idle
    public void PlayIdleAnimation() => AniUtility.PlayIdle(Skeleton);
    
    //战场中操作子弹时候的TxtUI抬升下落相关
    public void UpText()
    {
        Vector3 tmpPos = new Vector3(0,1.05f,0) + transform.position;
        tmpPos.y += 0.4f;
        txtDamage.transform.position = tmpPos;
    }
    public void ReturnText() => txtDamage.transform.position = transform.position + new Vector3(0,1.05f,0);
    #endregion

    #region 攻击相关
    public void PlayAttackChargeAnimation(Vector3 targetPos)
    {
        float aniTime = 0f;
        Vector3 curScale = transform.localScale;
        Skeleton.Initialize(true);//不初始化会有BUG，spine的莫名其妙
        AniUtility.PlayAttack(Skeleton, ref aniTime);
        transform.DOMove(targetPos, aniTime);
        transform.DOScale(curScale * 0.5f , aniTime);
        StartCoroutine(FadeOut(aniTime));
        //......关闭伤害提示UI
        txtDamage.gameObject.SetActive(false);
    }

    IEnumerator FadeOut(float duration)
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetFloat("_Transparency", 1);
        CurRenderer.SetPropertyBlock(block);
        
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float alpha = 1 - t / duration;
            block.SetFloat("_Transparency", alpha);
            CurRenderer.SetPropertyBlock(block);
            yield return null;
        }
        block.SetFloat("_Transparency",0);
        CurRenderer.SetPropertyBlock(block);
    }
    
    public void PlayAttackingLoopAnimation()
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetFloat("_Transparency", 1);
        CurRenderer.SetPropertyBlock(block);
        AniUtility.PlayAttacking(Skeleton,AniScale);
    }

    public void AttackingFly(float speed, float deltaTime) =>
        transform.Translate(Vector3.right * speed * deltaTime);

    public void PlayHitEffect()
    {
        GameObject fx = Instantiate(HitEffectPrefab, transform.position, Quaternion.identity);
        SkeletonAnimation curSpfxSC = fx.GetComponentInChildren<SkeletonAnimation>();
        curSpfxSC.skeletonDataAsset = HitSpfxAsset;
        curSpfxSC.Initialize(true);
        float aniTime = 0f;
        AniUtility.PlayAttack(curSpfxSC,ref aniTime,AniScale);
        Destroy(fx, 1.0f); // 简化的自动销毁
    }
    #endregion
}
