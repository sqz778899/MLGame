using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public float Distance = 3;
    public float Speed = 10.0f;
    SkeletonAnimation _ain;
    Vector3 forward = new Vector3(1, 0, 0);
    
    bool IsMove = true;
    
    void Start()
    {
        _ain = transform.GetChild(0).GetComponent<SkeletonAnimation>();
        AniUtility.PlayIdle(_ain);
    }
    
    void Update()
    {
        Move();
    }
    
    float CurDistance()
    {
        return transform.position.x - UIManager.Instance.RoleIns.transform.position.x;
    }
    void Move()
    {
        float dis = CurDistance();
        if(Math.Abs(dis) > Distance)
        {
            AniUtility.PlayRun(_ain);
            if (dis < 0)
            {
                AniUtility.TrunAround(_ain,1);
                transform.Translate( forward * Speed * Time.deltaTime);
            }
            else
            {
                AniUtility.TrunAround(_ain,-1);
                transform.Translate( -forward * Speed * Time.deltaTime);
            }
        }
        else
        {
            AniUtility.PlayIdle(_ain);
        }
    }
    
}
