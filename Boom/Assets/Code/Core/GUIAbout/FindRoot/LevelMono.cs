using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelMono : MonoBehaviour
{
    List<Enemy> _curEnemy;

    public List<Enemy> CurEnemy
    {
        get
        {
            if (_curEnemy == null) 
                _curEnemy = G_Enemy.transform.GetComponentsInChildren<Enemy>().ToList();
            return _curEnemy;
        }   
    }
    public GameObject G_Enemy;
}