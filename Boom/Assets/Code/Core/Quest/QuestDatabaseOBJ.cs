using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDatabaseOBJ : ScriptableObject
{
    public List<Quest> quests = new List<Quest>();

    public Quest GetQuestByID(int id)
    {
        return quests.Find(q => q.ID == id);
    }
}
