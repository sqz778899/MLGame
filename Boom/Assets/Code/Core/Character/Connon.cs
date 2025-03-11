using System;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using DG.Tweening;
using UnityEngine;
using Event = Spine.Event;

public class Connon : MonoBehaviour
{
    [Header("SpineAbout")]
    public SkeletonAnimation Ani01;
    public SkeletonAnimation Ani02;
    public AnimationCurve AniCurve;
    [Header("子弹相关")] 
    public List<BulletInner> AllBullets;
    public Transform FillNode; //装填自己的子弹的位置
    public Transform FireNode; //发射子弹的位置
    
    // 添加一个标志和子弹索引变量
    bool isFiring = false;
    int curBulletIndex;
    
    void Start()
    {
        // 监听 Spine 动画中的事件
        Ani01.AnimationState.Event += SpEventFire;
        AllBullets = new List<BulletInner>();
    }
    
    void SpEventFire(TrackEntry trackEntry, Event e)
    {
        // 处理事件，根据事件名称执行不同的逻辑
        switch (e.Data.Name)
        {
            case "fire":
                if (isFiring)
                {
                    //发射子弹
                    GameObject curBulletIns = AllBullets[curBulletIndex].gameObject;
                    curBulletIns.transform.position = FireNode.transform.position;
                    AllBullets[curBulletIndex].Attack();
                    isFiring = false; // 重置标志
                }
                break;

            default:
                Debug.Log($"Unhandled event: {e.Data.Name}");
                break;
        }
    }
    
    
    public void Appear(Vector3 targetPos,ref float aniTime)
    {
        AniUtility.PlayAppear(Ani01, ref aniTime);
        AniUtility.PlayAppear(Ani02, ref aniTime);
        transform.DOMove(targetPos, aniTime).SetEase(AniCurve);
    }

    public void Reload(ref float aniTime)
    {
        AniUtility.PlayReload(Ani01, ref aniTime);
        AniUtility.PlayReload(Ani02, ref aniTime);
    }
    
    public void Attack(int bulletIndex,ref float aniTime)
    {
        // 设置标志，表示即将进行开火攻击
        isFiring = true;
        curBulletIndex = bulletIndex;
        //bulletInner.Attack();
        AniUtility.PlayAttack(Ani01, ref aniTime,isReset:true);
        AniUtility.PlayAttack(Ani02, ref aniTime,isReset:true);
    }
    
    void OnDestroy()
    {
        // 移除事件监听器，防止内存泄漏
        Ani01.AnimationState.Event -= SpEventFire;
    }
}