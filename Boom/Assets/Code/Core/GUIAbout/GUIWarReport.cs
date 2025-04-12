using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GUIWarReport : MonoBehaviour
{
    public GameObject ReportRoot;
    public GameObject SingelReportTemplate;
    public TextMeshProUGUI txtTotalDamage;
    Vector2 SingelYOffset = new Vector2(0, -302);
    
    //B
    public void SyncReport()
    {
        for (int i = ReportRoot.transform.childCount -1; i >= 0; i--)
            Destroy(ReportRoot.transform.GetChild(i).gameObject);
        //Bullet 这个类
        //Enemy  这个类
        //List<BattleOnceHit> 战场表现收集
        int totalDamage = 0;
        SingelBattleInfoOld curInfoOld = BattleManager.Instance.battleData.CurWarReport.GetCurBattleInfoOld();
        int count = 0;
        foreach (var eachInfo in curInfoOld.InfoDict)
        {
            GameObject curSingelReport = Instantiate(SingelReportTemplate, ReportRoot.transform);
            curSingelReport.SetActive(true);
            curSingelReport.GetComponent<RectTransform>().anchoredPosition = SingelYOffset * count;
            curSingelReport.GetComponent<SingelReportRoot>().SyncReport(eachInfo.Value,out int totalTemp);
            totalDamage += totalTemp;
            count++;
        }
        txtTotalDamage.text = totalDamage.ToString();
    }
}