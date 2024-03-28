using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public int score = 100;

    public void TakeDamage(int amount)
    {
        health -= amount;

        // 如果血量为0或更少，销毁这个敌人
        if (health <= 0)
        {
            DestroySelf();
        }
    }

    void DestroySelf()
    {
        CharacterManager.Instance.Score += score;
        CharacterManager.Instance.WinOrFailState = WinOrFail.Win;
        Destroy(gameObject);
    }
}
