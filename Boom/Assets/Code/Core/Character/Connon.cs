﻿using System;
using Spine.Unity;
using DG.Tweening;
using UnityEngine;

public class Connon : MonoBehaviour
{
    [Header("SpineAbout")]
    public SkeletonAnimation Ani;
    public AnimationCurve AniCurve;
    [Header("子弹相关")] 
    public Transform FillNode; //装填自己的子弹的位置
    
    public void Appear(Vector3 targetPos,ref float aniTime)
    {
        AniUtility.PlayAppear(Ani, ref aniTime);
        transform.DOMove(targetPos, aniTime).SetEase(AniCurve);
    }
    
    public void Attack(ref float aniTime)
    {
        AniUtility.PlayAttack(Ani, ref aniTime);
    }
}