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
    
    public void SyncReport(BulletAttackRecord curInfo, out int totalDamage)
    {
        BulletData curBulletData = curInfo.BulletData;
        List<BattleOnceHit> curBattleOnceHits = curInfo.Hits;
        //战报标题部分
        txtDamage.text = curBattleOnceHits[0].FinalDamage.ToString();//伤害
        if (curBattleOnceHits[0].FinalPiercing == 0)
            txtPiercing.gameObject.SetActive(false);
        txtPiercing.text = curBattleOnceHits[0].FinalPiercing.ToString();//穿透
        if (curBattleOnceHits[0].FinalResonance == 0)
            txtResonance.gameObject.SetActive(false);
        txtResonance.text = curBattleOnceHits[0].FinalResonance.ToString();
        //细节部分参数
        int effectiveDamage = 0;
        int overflowDamage = 0;
        totalDamage = 0;
        //细节部分
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
            totalDamage = effectiveDamage + overflowDamage;
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
                    TextMeshProUGUI enemyHPText = IconEnemy.GetComponentInChildren<TextMeshProUGUI>(true);
                    enemyHPText.gameObject.SetActive(true);
                    enemyHPText.text = $"-{each.EffectiveDamage}";
                    if (!each.IsDestroyed)
                        IconEnemy.transform.GetChild(0).gameObject.SetActive(false);
                    effectiveDamage += each.EffectiveDamage;
                    overflowDamage += each.OverflowDamage;
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
        }
        //有效伤害
        if (effectiveDamage == 0)
            txtEffectiveDamage.gameObject.SetActive(false);
        txtEffectiveDamage.text = effectiveDamage.ToString();
        //溢出伤害
        if (overflowDamage == 0)
            txtOverflowDamage.gameObject.SetActive(false);
        txtOverflowDamage.text = overflowDamage.ToString();
        
        totalDamage = effectiveDamage + overflowDamage;
        BattleManager.Instance.battleData.CurWarReport.TotalDamage = totalDamage;
        BattleManager.Instance.battleData.CurWarReport.EffectiveDamage = effectiveDamage;
        BattleManager.Instance.battleData.CurWarReport.OverFlowDamage = overflowDamage;
    }
}