using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelMono : MonoBehaviour
{
    public Enemy CurEnemy;
    public GameObject EnemyRoot;
    public Collider2D MapCollider;
    public void SetEnemy(EnemyConfigData config) => CurEnemy = EnemyFactory.CreateEnemy(config,EnemyRoot.transform);
}