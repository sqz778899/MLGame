using System;
using System.Collections.Generic;
using UnityEngine;

public class QusetRoot: MonoBehaviour
{
    [Header("QuestBar")]
    public GameObject QuestBarPrefab;
    public GameObject QuestMenuGO;
    public float Yoffset = 10f;
    
    public void InitAllQuests()
    {
        List<Quest> quests = PlayerManager.Instance._QuestData.Quests;
        for (int i = 0; i < quests.Count; i++)
        {
            GameObject go = Instantiate(QuestBarPrefab, transform);
            go.GetComponent<QuestBar>().SetInfo(quests[i].ID, QuestMenuGO);
            Vector3 pos = go.transform.position;
            go.transform.position = new Vector3(pos.x, pos.y - i*Yoffset, pos.z);
        }
    }
}