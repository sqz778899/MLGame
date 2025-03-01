using System.Collections.Generic;
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
    Vector2 _shieldStartPos = new Vector2(560, 512); 
    Vector2 _shieldOffset = new Vector2(-100, 0); 
    
    public void SyncReport(KeyValuePair<BulletJson,List<BattleOnceHit>> curInfo)
    {
        BulletJson curBulletJson = curInfo.Key;
        List<BattleOnceHit> curBattleOnceHits = curInfo.Value;
        txtDamage.text = curBulletJson.FinalDamage.ToString();
        txtPiercing.text = curBulletJson.FinalPiercing.ToString();
        txtResonance.text = curBulletJson.FinalResonance.ToString();
        int effectiveDamage = 0;
        int overflowDamage = 0;

        if (curBattleOnceHits.Count == 0) return;
        
        //记录每一次攻击的状态
        BattleOnceHit firstHit = curBattleOnceHits[0];
        if (firstHit.ShieldIndex == -1)//敌人无盾
        {
            if(firstHit.IsDestroyed)
                IconEnemy.transform.GetChild(0).gameObject.SetActive(true);
            GameObject textGO = IconEnemy.transform.GetChild(2).gameObject;
            textGO.SetActive(true);
            textGO.GetComponent<TextMeshProUGUI>().text = $"-{firstHit.EffectiveDamage}";
            
            effectiveDamage += firstHit.EffectiveDamage;
            overflowDamage += firstHit.OverflowDamage;
        }
        else
        {
            Dictionary<int,GameObject> iconShieldDict = new Dictionary<int, GameObject>();
            int ShieldCount = firstHit.ShieldIndex + 1;
            for (int i = 0; i < ShieldCount; i++)
            {
                GameObject curIconShield = Instantiate(IconShield, IconShield.transform.parent);
                curIconShield.SetActive(true);
                curIconShield.GetComponent<RectTransform>().anchoredPosition = _shieldStartPos + _shieldOffset * i;
                iconShieldDict.Add(i,curIconShield);
            }
            //敌人有盾的情况下，显示Icon
            foreach (BattleOnceHit each in curBattleOnceHits)
            {
                if (each.ShieldIndex == -1)
                {
                    IconEnemy.GetComponentInChildren<TextMeshProUGUI>().text = $"-{each.EffectiveDamage}";
                    if (!each.IsDestroyed)
                        IconEnemy.transform.GetChild(0).gameObject.SetActive(false);
                    continue;
                }
                
                GameObject curIconShield = iconShieldDict[each.ShieldIndex];
                GameObject textGO = curIconShield.transform.GetChild(2).gameObject;
                textGO.SetActive(true);
                textGO.GetComponent<TextMeshProUGUI>().text = $"-{each.EffectiveDamage}";
                if (each.IsDestroyed)
                    curIconShield.transform.GetChild(0).gameObject.SetActive(true);
                
                effectiveDamage += each.EffectiveDamage;
                overflowDamage += each.OverflowDamage;
            }
            Debug.Log("敌人有盾");
        }
        
        txtEffectiveDamage.text = effectiveDamage.ToString();
        txtOverflowDamage.text = overflowDamage.ToString();
    }
}