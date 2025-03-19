using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using TMPro;
using UnityEngine;

public class EnemyMiniMap : MonoBehaviour
{
    public Enemy CurEnemy;
    Dictionary<TextMeshProUGUI,ShieldMono> ShieldsBind;
    Dictionary<TextMeshProUGUI,GameObject> ShieldsTextGOBind;
    
    [Header("资产")]
    public SkeletonGraphic SkeletonGraphic; //敌人的UI层Spine资产
    public TextMeshProUGUI EnemyHPText; //敌人的血量文本
    public GameObject ShieldsNode; //敌人的护盾节点
    public GameObject ShieldGO; //护盾的预制体

    public void InitData(Enemy _curEnemy)
    {
        CurEnemy = _curEnemy;//绑定敌人
        ShieldsBind = new Dictionary<TextMeshProUGUI, ShieldMono>();
        ShieldsTextGOBind = new Dictionary<TextMeshProUGUI, GameObject>();
        CurEnemy.OnLoadData += BindData;
        CurEnemy.OnTakeDamage += ChangeEnemyUI;
        
        //EnemyMiddleData midData = BattleManager.Instance.EnemyMidData;
        SkeletonGraphic.skeletonDataAsset = ResManager.instance.
            GetAssetCache<SkeletonDataAsset>(PathConfig.GetEnemySkelentonDataPath(_curEnemy.ID));
        SkeletonGraphic.Initialize(true);
        AniUtility.PlayIdle(SkeletonGraphic);//最开始播放待机动画
    }

    public void BindData()
    {
        CurEnemy.OnLoadData -= BindData;//事件只需要注册响应一次，用过即丢
        EnemyHPText.text = CurEnemy.CurHP.ToString();
        //绑定盾牌
        for (int i = ShieldsNode.transform.childCount-1; i >= 0; i--)
            Destroy(ShieldsNode.transform.GetChild(i).gameObject);
        
        for (int i = 0; i < CurEnemy.ShieldsHPs.Count; i++)
        {
            GameObject shieldIns = Instantiate(ShieldGO);
            ShieldMono shieldSC = CurEnemy.Shields[i];
            shieldIns.SetActive(true);
            shieldIns.transform.SetParent(ShieldsNode.transform,false);
            float curStep = i * -220f;
            shieldIns.transform.localPosition = new Vector3(curStep,0,0);
            
            TextMeshProUGUI shieldText = shieldIns.GetComponentInChildren<TextMeshProUGUI>();
            shieldText.text = CurEnemy.ShieldsHPs[i].ToString();
            ShieldsBind.Add(shieldText,shieldSC);//绑定盾牌
            ShieldsTextGOBind.Add(shieldText,shieldIns);//绑定盾牌的UI
            shieldSC.OnTakeDamage += ChangeEnemyUI;//绑定盾牌的UI更新
        }
    }
    
    public void ChangeEnemyUI()
    {
        int mainHP = CurEnemy.CurHP;
        EnemyHPText.text = mainHP.ToString();
        if (mainHP <= 0)
            AniUtility.PlayDead01(SkeletonGraphic);//敌人死亡

        foreach (var each in ShieldsBind)
        {
            if(each.Value!=null)
                each.Key.text = each.Value.CurHP.ToString();
            else
                ShieldsTextGOBind[each.Key].SetActive(false);
        }
    }
}
