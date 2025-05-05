using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestData : ScriptableObject
{
    public List<Quest> Quests = new();

    [Header("主线剧情推进标记")]
    public int MainStoryProgress;
    
    public void UpdateQuestState(int questID, QuestState newState)
    {
        var quest = Quests.Find(q => q.ID == questID);
        if (quest != null && quest.State != newState)
        {
            quest.State = newState;
            if (newState == QuestState.Completed)
            {
                // 1)如果任务完成，主线剧情进度+1
                if (!quest.IsCompleted)
                    MainStoryProgress++;
                // 2)任务完成后，标记为已完成
                quest.IsCompleted = true;
                // 3)任务完成后，记录历史最高分数
                int curScore = GM.Root.PlayerMgr._PlayerData.Score;
                quest.TotalScore = Mathf.Max(curScore, quest.TotalScore);
                // 4)任务完成后，记录完成次数
                quest.TotalLoopCount += 1;
                // 5)任务完成后，记录房间探索进度
                int explorePercent = GM.Root.BattleMgr._MapManager.CurMapSate.ExplorePercent;
                quest.ExplorationPercent = Mathf.Max(explorePercent, quest.ExplorationPercent);
                // 6)任务完成后，结算魔尘奖励
                GM.Root.PlayerMgr._PlayerData.LevelRewards();
            }
        }
    }

    public Quest GetQuestByID(int questID) => Quests.FirstOrDefault(q => q.ID == questID);
}