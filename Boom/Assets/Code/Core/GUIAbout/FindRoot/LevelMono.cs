using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMono : MonoBehaviour
{
    Enemy _curEnemy;

    public Enemy CurEnemy
    {
        get
        {
            if (_curEnemy == null) 
                _curEnemy = G_Enemy.transform.GetChild(0).GetComponent<Enemy>();
            return _curEnemy;
        }   
    }
    public GameObject G_Enemy;
}