using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class GUIFightRoot : MonoBehaviour
{
    public GameObject FightLogicGO;
    public GameObject G_BulletInScene; //场景内的子弹的父节点
    public GameObject CharILIns;

    public Vector3 OriginCameraPos;
    public float orthographicSize;
    
    public Vector3 TargetCameraOffset;
    public float TargetOrthographicSize;

    private void Start()
    {
        TargetCameraOffset = new Vector3(-2.5f,-0.65f,0);
        //TargetCameraPos = new Vector3(0,0.45f,-10);
        TargetOrthographicSize = 3.35f;
    }

    public void InitData()
    {
        OriginCameraPos = Camera.main.transform.position;
        orthographicSize = Camera.main.orthographicSize;
    }
    
    public void SetCameraEdit()
    {
        InitData();
        Sequence seq = DOTween.Sequence();
        seq.Append(Camera.main.transform.DOMove(OriginCameraPos + TargetCameraOffset, 0.8f).SetEase(Ease.InOutQuad));
        seq.Join(DOTween.To(() => Camera.main.orthographicSize, 
            x => Camera.main.orthographicSize = x, 
            TargetOrthographicSize, 0.8f).SetEase(Ease.InOutQuad));
    }
    
    public void SetCameraBattle()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(Camera.main.transform.DOMove(OriginCameraPos, 0.8f).SetEase(Ease.InOutQuad));
        seq.Join(DOTween.To(() => Camera.main.orthographicSize, 
            x => Camera.main.orthographicSize = x, 
            orthographicSize, 0.8f).SetEase(Ease.InOutQuad));
    }
}
