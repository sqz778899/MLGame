using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;

    public void TakeDamage(int amount)
    {
        health -= amount;

        // 如果血量为0或更少，销毁这个敌人
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
