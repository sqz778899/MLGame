using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLogic
{
    public float GetDistance()
    {
        Vector3 enemyPos = UIManager.Instance.EnemyILIns.transform.position;
        Vector3 charPos = UIManager.Instance.CharILIns.transform.position;
        return Vector3.Distance(enemyPos,charPos);
    }
}
