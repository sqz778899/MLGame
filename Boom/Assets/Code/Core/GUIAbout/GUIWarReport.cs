using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIWarReport : MonoBehaviour
{
    public Enemy CurEnemy;
    public GameObject SingelReportRoot;
    Vector2 SingelYOffset = new Vector2(0, -302);
    
    //B
    public void SyncReport()
    {
        //Bullet 这个类
        //Enemy  这个类
        //List<BattleOnceHit> 战场表现收集
        SingelBattleInfo curInfo = MainRoleManager.Instance.CurWarReport.GetCurBattleInfo();
        int count = 0;
        foreach (var eachInfo in curInfo.InfoDict)
        {
            GameObject curSingelReport = Instantiate(SingelReportRoot, SingelReportRoot.transform.parent);
            curSingelReport.SetActive(true);
            curSingelReport.GetComponent<RectTransform>().anchoredPosition = SingelYOffset * count;
            curSingelReport.GetComponent<SingelReportRoot>().SyncReport(eachInfo.Value);
            count++;
        }
    }
}