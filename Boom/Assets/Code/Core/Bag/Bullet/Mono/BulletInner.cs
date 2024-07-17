using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInner : BulletBase
{
    public float maxDis = 105f;
    List<GameObject> FXs;

    void Start()
    {
        FXs = new List<GameObject>();
    }

    void Update()
    {
        // 让子弹沿着Z轴向前移动
        transform.Translate(forward * 10f * Time.deltaTime);
        if (transform.position.x>maxDis)
        {
            Destroy(gameObject);
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
}
