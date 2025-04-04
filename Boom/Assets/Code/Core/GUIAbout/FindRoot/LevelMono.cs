using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelMono : MonoBehaviour
{
    public Enemy CurEnemy;
    public GameObject EnemyRoot;
    public Transform HitTextRoot; //伤害跳字的GO节点
    public Collider2D MapCollider;
    
    public void SetEnemy(EnemyMiddleData _midData)
    {
        GameObject EnmeyIns = ResManager.instance.CreatInstance(PathConfig.EnemyPB);
        Enemy curEnemySC = EnmeyIns.GetComponent<Enemy>();
        CurEnemy = curEnemySC;
        curEnemySC.LoadMiddleData(_midData);
        curEnemySC.HitTextRoot = HitTextRoot;
        EnmeyIns.transform.SetParent(EnemyRoot.transform,false);
        //EnmeyIns.transform.position = new Vector3(14,-0.45f,0);
    }
}