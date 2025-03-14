using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [Header("角色")]
    public GameObject Role;
    //public EnemySpawner enemySpawner; // 假设敌人生成器组件

    // 根据难度设置地图怪物、障碍物等
    public void SetupDifficulty(int level)
    {
        /*if (enemySpawner != null)
        {
            enemySpawner.SetDifficulty(level);
        }*/

        // 其他状态设置...
    }
}
