using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DialogueFight : MonoBehaviour
{
    [Header("重要信息")] 
    public Image Portrait;
    public int LevelID = 1;
    public Transform ObserveHPRoot;
    public ArrowNode CurArrow;
    public float observeHPYOffset = 145;
    public TextMeshProUGUI CotentText;

    [Header("按钮")]
    public Button BtnFight;
    public List<GameObject> HPGOList = new List<GameObject>();
    
    public void InitData(ArrowNode _arraow)
    {
        CurArrow = _arraow;
        //头像的初始化
        Enemy curEnemy = CurArrow.CurEnemy;
        curEnemy.InitData();
        Portrait.sprite = curEnemy.Portrait;
        //战斗对话框的界面初始化
        GameObject observeHPAsset = ResManager.instance.GetAssetCache<GameObject>(PathConfig.ObserveHPPB);
        int HPCounts = curEnemy.ShieldsHPs.Count + 1;
        for (int i = 0; i < HPCounts; i++)
        {
            GameObject observeHPIns = Instantiate(observeHPAsset, ObserveHPRoot.transform);
            HPGOList.Add(observeHPIns);
            RectTransform observeHPRectTrans = observeHPIns.GetComponent<RectTransform>();
            // 设置位置
            observeHPRectTrans.anchoredPosition = new Vector2(0,0 + observeHPYOffset*i);
            // 获取并设置文本
            TextMeshProUGUI curObserveHPText = observeHPIns.GetComponentInChildren<TextMeshProUGUI>(true);
            int currentHP = (i == 0) ? curEnemy.MaxHP : curEnemy.ShieldsHPs[i - 1]; // 根据i来选择显示的血量
            curObserveHPText.text = currentHP.ToString();
        }
    }
    
    public void EnterFight()
    {
        if (EternalCavans.Instance.TutorialFightLock) return;
        
        UIManager.Instance.IsLockedClick = false;
        BattleManager.Instance.EnterFight(CurArrow.CurEnemy.ToMiddleData(),LevelID);
        QuitSelf();
    }

    public void QuitSelf()
    {
        UIManager.Instance.IsLockedClick = false;
        CurArrow.IsLocked = false;
        DestroyImmediate(gameObject);
    }
}
