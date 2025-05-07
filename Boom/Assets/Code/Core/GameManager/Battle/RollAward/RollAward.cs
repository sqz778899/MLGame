using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RollAward : MonoBehaviour
{
    [Header("资产")]
    [SerializeField] TextMeshProUGUI texJumpButton;
    [SerializeField] List<RollSingle> rollSingles;
    Action _onFinish;
    BattleData _battleData => GM.Root.BattleMgr.battleData;
    
    public void ShowReward(Action onFinish)
    {
        _onFinish = onFinish;
        gameObject.SetActive(true);
        Award award = _battleData.CurEnemy.Controller.GetAward();
        List<DropedObjEntry> selectedRewards = new List<DropedObjEntry>();
        HashSet<string> drawnKeys = new HashSet<string>();

        for (int i = 0; i < 3; i++)
        {
            DropedObjEntry reward = DropTableService.Draw(award.RollPoolName,
                GM.Root.BattleMgr._MapManager.CurMapSate.MapRandomSeed, null);
            if (reward != null && drawnKeys.Add($"{reward.ID}_{reward.DropedCategory}"))
                selectedRewards.Add(reward);
        }
        for (int i = 0; i < rollSingles.Count; i++)
        {
            DropedObjEntry entry = selectedRewards[i];
            rollSingles[i].InitData(entry, OnItemSelected);
        }
    }
    
    void OnItemSelected(DropedObjEntry dropedObjEntry)
    {
        DropRewardService.Drop(dropedObjEntry,Vector3.zero,MapEventType.BasicGambling,1);
        Debug.Log("获得奖励: " + dropedObjEntry.Name);
        Hide();
    }
    
    public void Hide() 
    {
        _onFinish?.Invoke();
        gameObject.SetActive(false);
    }
}
