using UnityEngine;

public class Bullet : BulletBase
{
    void Update()
    {
        // 让子弹沿着Z轴向前移动
        transform.Translate(forward * _bulletData.speed * Time.deltaTime);
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
                Instantiate(_bulletData.hitEffect, transform.position, transform.rotation);
            }

            // 销毁子弹
            Destroy(gameObject);
        }
    }
}
