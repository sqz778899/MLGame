using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using TMPro;
using UnityEngine;

public class UpgradeUIDisplay : MonoBehaviour
{
    [Header("依赖资产")]
    [SerializeField] SkeletonGraphic PreSkeleton;
    [SerializeField] SkeletonGraphic AfterSkeleton;
    [SerializeField] SkeletonGraphic MaxSkeleton;
    [SerializeField] TextMeshProUGUI txtPreName;
    [SerializeField] TextMeshProUGUI txtAfterName;
    [SerializeField] TextMeshProUGUI txtMaxName;
    [SerializeField] GameObject normalGroup;
    [SerializeField] GameObject maxGroup;
    [SerializeField] GameObject AttriComparisonGO;
    
    List<GameObject> attriComparisonList = new List<GameObject>();
    public void InitData(UpgradeBulletInfo curInfo)
    {
        //
        BulletJson afterBulletJson = null;
        if (curInfo.IsCanUpgrade) //还有升级空间
        {
            normalGroup.SetActive(true);
            maxGroup.SetActive(false);
            BulletJson preBulletJson = TrunkManager.Instance.GetBulletJson(curInfo.ID);
            afterBulletJson = TrunkManager.Instance.GetBulletJson(curInfo.ID + 100);
            BulletFactory.SetBulletInUI(PreSkeleton,curInfo.ID);
            BulletFactory.SetBulletInUI(AfterSkeleton,curInfo.ID + 100);
            txtPreName.text = curInfo.Name;
            txtAfterName.text = afterBulletJson.NameKey;
            //提升属性差异显示
            int difDamage = afterBulletJson.Damage - preBulletJson.Damage;
            int difCritical = afterBulletJson.Critical - preBulletJson.Critical;
            int difElementalInfusionValue = afterBulletJson.ElementalInfusionValue - preBulletJson.ElementalInfusionValue;
            Dictionary<string,int> difDic = new Dictionary<string, int>();
            difDic.Add("伤害",difDamage);
            difDic.Add("暴击",difCritical);
            difDic.Add("元素",difElementalInfusionValue);

            int count = 0;
            foreach (var each in difDic)
            {
                if (each.Value == 0) continue;
                GameObject go = Instantiate(AttriComparisonGO);
                attriComparisonList.Add(go);
                TextMeshProUGUI txt = go.GetComponentInChildren<TextMeshProUGUI>();
                go.SetActive(true);
                go.transform.SetParent(normalGroup.transform,false);
                go.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, -80 * count);
                txt.text = $"{each.Key}+{each.Value}";
                if (each.Key == "暴击")
                    txt.text = $"{each.Key}+{each.Value}%";
                count++;
            }
        }
        else
        {
            normalGroup.SetActive(false);
            maxGroup.SetActive(true);
            BulletFactory.SetBulletInUI(MaxSkeleton,curInfo.ID);
            txtMaxName.text = curInfo.Name;
        }
    }

    public void ClearData()
    {
        for (int i = attriComparisonList.Count - 1; i>=0; i--)
            Destroy(attriComparisonList[i]);
        attriComparisonList.Clear();
    }
}