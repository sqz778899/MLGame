using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIWarReport : MonoBehaviour
{
    public Enemy CurEnemy;
    public GameObject SingelReportRoot;
    
    //B
    public void SyncReport()
    {
        //Bullet 这个类
        //Enemy  这个类
        //List<BattleOnceHit> 战场表现收集
        SingelReportRoot.GetComponent<SingelReportRoot>().SyncReport(CurEnemy);
    }
}