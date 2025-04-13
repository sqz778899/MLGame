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
  
        //List<BattleOnceHit> 战场表现收集
        int totalDamage = 0;
        SingleBattleReport curInfo = BattleManager.Instance.battleData.CurWarReport.GetCurBattleInfo();

        for (int i = 0; i < curInfo.BulletAttackRecords.Count; i++)
        {
            BulletAttackRecord record = curInfo.BulletAttackRecords[i];
            GameObject curSingelReport = Instantiate(SingelReportTemplate, ReportRoot.transform);
            curSingelReport.SetActive(true);
            curSingelReport.GetComponent<RectTransform>().anchoredPosition = SingelYOffset * i;
            curSingelReport.GetComponent<SingelReportRoot>().SyncReport(record,out int totalTemp);
            totalDamage += totalTemp;
        }
        txtTotalDamage.text = totalDamage.ToString();
    }
}