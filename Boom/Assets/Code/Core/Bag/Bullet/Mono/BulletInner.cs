using System;
using System.Collections;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

public class BulletInner : BulletBase
{
    public float FollowDis = 3;
    public float RunSpeed = 10.0f;
    
    public float DisabearDis = 105f;
    
    public bool IsMove = true;
    
    SkeletonAnimation _ain;
    List<GameObject> FXs;

    void Start()
    {
        _ain = transform.GetChild(0).GetComponent<SkeletonAnimation>();
        AniUtility.PlayIdle(_ain);
        FXs = new List<GameObject>();
    }

    void Update()
    {
        Run(); //在界面跟着主角跑
        /*// 让子弹沿着Z轴向前移动
        if (!IsMove)
        {
            return;
        }
        transform.Translate(forward * 10f * Time.deltaTime);
        if (transform.position.x > DisabearDis)
        {
            Destroy(gameObject);
        }*/
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
                IsMove = false;
                GameObject curFX = Instantiate(_bulletData.hitEffect, transform.position, transform.rotation);
                FXs.Add(curFX);
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
    
    void Run()
    {
        float dis = CurDistance();
        if(Math.Abs(dis) > FollowDis)
        {
            AniUtility.PlayRun(_ain);
            if (dis < 0)
            {
                AniUtility.TrunAround(_ain,1);
                transform.Translate( forward * RunSpeed * Time.deltaTime);
            }
            else
            {
                AniUtility.TrunAround(_ain,-1);
                transform.Translate( -forward * RunSpeed * Time.deltaTime);
            }
        }
        else
            AniUtility.PlayIdle(_ain);
    }
    
    float CurDistance()
    {
        return transform.position.x - UIManager.Instance.RoleIns.transform.position.x;
    }
}
