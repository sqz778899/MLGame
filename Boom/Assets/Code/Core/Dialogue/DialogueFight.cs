using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueFight : MonoBehaviour
{
    public int LevelID = 1;
    
    public Transform ObserveHPRoot;
    public ArrowNode CurArrow;
    public float observeHPYOffset = 145;

    [Header("按钮")]
    public Button BtnFight;
    public Button BtnBack;
    public List<GameObject> HPGOList = new List<GameObject>();
    public bool isLocked = false;
    
    public void InitData(ArrowNode _arraow)
    {
        CurArrow = _arraow;
        GameObject observeHPAsset = ResManager.instance.GetAssetCache<GameObject>(PathConfig.ObserveHPPB);
        int HPCounts = _arraow.CurEnemy.ShieldsHPs.Count + 1;
        for (int i = 0; i < HPCounts; i++)
        {
            GameObject observeHPIns = Instantiate(observeHPAsset, ObserveHPRoot.transform);
            HPGOList.Add(observeHPIns);
            RectTransform observeHPRectTrans = observeHPIns.GetComponent<RectTransform>();
            // 设置位置
            observeHPRectTrans.anchoredPosition = new Vector2(0,0 + observeHPYOffset*i);
            // 获取并设置文本
            TextMeshProUGUI curObserveHPText = observeHPIns.GetComponentInChildren<TextMeshProUGUI>(true);
            int currentHP = (i == 0) ? _arraow.CurEnemy.MaxHP : _arraow.CurEnemy.ShieldsHPs[i - 1]; // 根据i来选择显示的血量
            curObserveHPText.text = currentHP.ToString();
        }
    }
    
    public void EnterFight()
    {
        if (isLocked ) return;
        UIManager.Instance.IsLockedClick = false;
        MainSceneMono _mainSceneMono = UIManager.Instance.MainSceneGO.GetComponent<MainSceneMono>();
        MainRoleManager.Instance.InitFightData(CurArrow.CurEnemy.ToMiddleData(),LevelID);
        _mainSceneMono.SwitchFightScene();
        QuitSelf();
    }
    
    public void QuitSelf()
    {
        if (isLocked ) return;
        UIManager.Instance.IsLockedClick = false;
        CurArrow.IsLocked = false;
        DestroyImmediate(gameObject);
    }
}
