using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestBar : MonoBehaviour
{
    public int QuestID;
    [Header("资产")]
    public Button btnQuest;
    public TextMeshProUGUI txtQuestName;
    public GameObject RankStarRoot;
    public GameObject Medal;

    public void SetInfo(int questID)
    {
        Quest curQuest = PlayerManager.Instance._QuestData.GetQuestByID(questID);
        txtQuestName.text = curQuest.Name;
        QuestID = questID;
        if (curQuest.IsCompleted)
            Medal.SetActive(true);
        else
            Medal.SetActive(false);
        
        // 清空RankStarRoot下的所有子对象
        foreach (Transform child in RankStarRoot.transform)
            child.gameObject.SetActive(false);
        
        // 根据Quest的Level打开相应数量的子对象
        for (int i = 0; i < curQuest.Level && i < RankStarRoot.transform.childCount; i++)
            RankStarRoot.transform.GetChild(i).gameObject.SetActive(true);
        
        btnQuest.onClick.AddListener(() => QuestManager.Instance.SelectQuest(questID));
    }
}
