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
                if (!quest.IsCompleted)
                    MainStoryProgress++;
                quest.IsCompleted = true;
            }
        }
    }

    public Quest GetQuestByID(int questID) => Quests.FirstOrDefault(q => q.ID == questID);
}