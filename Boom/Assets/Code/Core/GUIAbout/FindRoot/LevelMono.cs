using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMono : MonoBehaviour
{
    public GameObject G_Enemy;

    public Enemy GetCurEnemy()
    {
        return G_Enemy.transform.GetChild(0).GetComponent<Enemy>();
    }
}
