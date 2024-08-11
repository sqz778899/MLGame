using System;
using System.Collections;
using Spine.Unity;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity.Editor;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletInner : BulletBase
{
    public float FollowDis = 3;
    public float RunSpeed = 10.0f;
    
    public float DisabearDis = 105f;
    public BulletInnerState _state;
    
    SkeletonAnimation _ain;
    public float AniScale = 1f;
    List<GameObject> FXs;

    void Start()
    {
        _state = BulletInnerState.Common;
        _ain = transform.GetChild(0).GetComponent<SkeletonAnimation>();
        AniUtility.PlayIdle(_ain,AniScale);
        FXs = new List<GameObject>();
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
                transform.Translate(forward * 10f * Time.deltaTime);
                break;
            case BulletInnerState.Dead:
                Destroy(gameObject);
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 检测子弹是否触碰到敌人（敌人需要有 "Enemy" 标签）
        if (other.CompareTag("Enemy"))
        {
            // 如果有敌人，将其血量减少
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                CalculateDamageManager.Instance.CalDamage(_bulletData,enemy);
            }

            // 创建击中特效
            if (_bulletData.hitEffect != null)
            {
                StartCoroutine(PlayHitFX());
            }
            //延迟销毁子弹
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void DestroySelf()
    {
        for (int i = FXs.Count - 1; i >= 0; i--)
        {
            Destroy(FXs[i]);
        }
        // 销毁子弹
        Destroy(gameObject);
    }

    #region 攻击

    public IEnumerator Attack(Connon CurConnon)
    {
        //...............填弹...........
        _state = BulletInnerState.AttackBegin;
        float aniTime = 0f;
        Vector3 curScale = transform.localScale;
        AniUtility.PlayAttack(_ain,ref aniTime,AniScale);
        Transform fillBulletTarget = CurConnon.FillNode.transform;
        transform.DOMove(fillBulletTarget.position, aniTime);
        transform.DOScale(curScale * 0.5f , aniTime);
        //DOFade();

        float connonAnitime = 0f;
        CurConnon.Attack(ref connonAnitime);
        yield return new WaitForSeconds(aniTime);
        transform.DOScale(curScale, 0.1f);
        
        _state = BulletInnerState.Attacking;
        AniUtility.PlayAttacking(_ain,AniScale);
    }

    public IEnumerator PlayHitFX()
    {
        GameObject curFX = Instantiate(_bulletData.hitEffect, transform.position, transform.rotation);
        SkeletonAnimation curSpfxSC = curFX.GetComponentInChildren<SkeletonAnimation>();
        curSpfxSC.skeletonDataAsset = _bulletData.hitSpfxAsset;
        SpineEditorUtilities.ReloadSkeletonDataAssetAndComponent(curSpfxSC);
        float aniTime = 0f;
        AniUtility.PlayAttack(curSpfxSC,ref aniTime,AniScale);
        FXs.Add(curFX);
        yield return new WaitForSeconds(aniTime);
    }
    #endregion
    
    void Run()
    {
        float dis = CurDistance();
        //..............面向...................
        if (dis < 0)
            AniUtility.TrunAround(_ain, 1);
        else
            AniUtility.TrunAround(_ain, -1);
        
        //..............移动方向...................
        if(Math.Abs(dis) > FollowDis)
        {
            AniUtility.PlayRun(_ain,AniScale);
            if (dis < 0)
                transform.Translate( forward * RunSpeed * Time.deltaTime);
            else
                transform.Translate( -forward * RunSpeed * Time.deltaTime);
        }
        else
            AniUtility.PlayIdle(_ain,AniScale);
    }
    
    float CurDistance()
    {
        return transform.position.x - UIManager.Instance.RoleIns.transform.position.x;
    }
}
