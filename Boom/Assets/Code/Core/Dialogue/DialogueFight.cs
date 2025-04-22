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
    public float observeHPYOffset = 145;
    public List<GameObject> HPGOList = new List<GameObject>();
    
    MapNodeData _sourceNode;
    EnemyConfigData _enemyConfig;
    
    public void InitData(MapNodeData nodeData)
    {
        _sourceNode = nodeData;

        if (nodeData.EventData is not RoomArrowRuntimeData arrowData || arrowData.BattleConfig == null)
        {
            Debug.LogError("传入的不是 RoomArrow 或者敌人配置为空！");
            return;
        }

        _enemyConfig = arrowData.BattleConfig;
        Portrait.sprite = EnemyFactory.GetEnemyPortrait(_enemyConfig.ID);

        // 初始化观察HP界面
        GameObject observeHPAsset = ResManager.instance.GetAssetCache<GameObject>(PathConfig.ObserveHPPB);
        List<int> shields = _enemyConfig.ShieldConfig?.ShieldsHPs ?? new();
        for (int i = 0; i < shields.Count + 1; i++)
        {
            GameObject observeHPIns = Instantiate(observeHPAsset, ObserveHPRoot.transform);
            HPGOList.Add(observeHPIns);
            RectTransform rectTrans = observeHPIns.GetComponent<RectTransform>();
            rectTrans.anchoredPosition = new Vector2(0, observeHPYOffset * i);
            
            int currentHP = (i == 0) ? _enemyConfig.HP : shields[i - 1];
            var hpText = observeHPIns.GetComponentInChildren<TextMeshProUGUI>(true);
            hpText.text = currentHP.ToString();
        }
    }
    
    public void EnterFight()
    {
        UIManager.Instance.IsLockedClick = false;
        BattleManager.Instance.EnterFight(_enemyConfig, LevelID);
        QuitSelf();
    }

    public void QuitSelf()
    {
        UIManager.Instance.IsLockedClick = false;
        // 解锁原节点（如果你还需要这么处理）
        if (_sourceNode != null)
            _sourceNode.IsLocked = false;
        gameObject.SetActive(false);
    }
}
