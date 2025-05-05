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
    public GameObject QuestMenuGO;
    QuestMenu QuestMenuSC;
    public bool IsLocked = false;

    public void SetInfo(int questID,GameObject questMenuGO,bool isLocked = false)
    {
        IsLocked = isLocked;
        Quest curQuest = GM.Root.PlayerMgr._QuestData.GetQuestByID(questID);
        txtQuestName.text = curQuest.Name;
        QuestID = questID;
        QuestMenuGO = questMenuGO;
        QuestMenuSC = questMenuGO.GetComponent<QuestMenu>();
        //1)同步勋章状态
        if (curQuest.IsCompleted)
            Medal.SetActive(true);
        else
            Medal.SetActive(false);
        
        //2)同步星级状态
        foreach (Transform child in RankStarRoot.transform)
            child.gameObject.SetActive(false);
        for (int i = 0; i < curQuest.Level && i < RankStarRoot.transform.childCount; i++)
            RankStarRoot.transform.GetChild(i).gameObject.SetActive(true);
        
        //3)添加按钮事件 
        btnQuest.onClick.AddListener(OpenMenu);
    }

    void OpenMenu()
    {
        if (IsLocked)
        {
            FloatingTextFactory.CreateUIText("任务暂时锁定", transform.localPosition, Color.white, 50f);
            return;
        }
        QuestMenuSC.SetInfo(QuestID);
        QuestMenuGO.SetActive(true);
    }
}
