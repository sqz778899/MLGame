using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SingelReportRoot : MonoBehaviour
{
    [Header("战报数据资产")] 
    public Image Portrait;
    public TextMeshProUGUI txtDamage;
    public TextMeshProUGUI txtPiercing;
    public TextMeshProUGUI txtResonance;
    public TextMeshProUGUI txtEffectiveDamage;
    public TextMeshProUGUI txtOverflowDamage;

    public GameObject IconEnemy;
    public GameObject IconShield;

    public void SyncReport(Enemy CurEnemy)
    {
        
    }
}