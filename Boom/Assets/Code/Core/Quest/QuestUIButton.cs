using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIButton : MonoBehaviour
{
    public string questID; // 在Inspector设置任务ID
    public Button button;

    void Start()
    {
        button.onClick.AddListener(OnQuestButtonClicked);
    }

    void OnQuestButtonClicked()
    {
        QuestManager.Instance.SelectQuest(questID);
    }
}
