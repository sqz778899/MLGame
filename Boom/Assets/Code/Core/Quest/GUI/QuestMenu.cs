using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestMenu : MonoBehaviour
{
    public int QuestID;
    [Header("资产")]
    public TextMeshProUGUI txtQuestName;//任务名称
    public TextMeshProUGUI txtQuesDescription;//任务描述
    public GameObject RankStarRoot;//任务等级
    public TextMeshProUGUI txtTotalScore;//总分
    public TextMeshProUGUI txtTotalLoopCount;//总循环次数
    public Image progressBarImage; //进度条
    public TextMeshProUGUI txtExplorationPercent; //探索进度百分比
    public Button btnQuest;
    [Header("进度条颜色设置")]
    public Color colorLow = Color.white;                 // ≤70% 时纯白色
    public Color colorHigh = new Color(1f, 0.85f, 0f);   // 接近100%时金色
    
    public void SetInfo(int questID)
    {
        QuestID = questID;
        Quest curQuest = PlayerManager.Instance._QuestData.GetQuestByID(questID);
        //1)同步任务信息
        txtQuestName.text = curQuest.Name;
        txtQuesDescription.text = curQuest.Description;
        
        //2)同步星级状态
        foreach (Transform child in RankStarRoot.transform)
            child.gameObject.SetActive(false);
        for (int i = 0; i < curQuest.Level && i < RankStarRoot.transform.childCount; i++)
            RankStarRoot.transform.GetChild(i).gameObject.SetActive(true);
        
        //3)同步历史记录
        txtTotalScore.text = curQuest.TotalScore.ToString();
        txtTotalLoopCount.text = curQuest.TotalLoopCount.ToString();
        txtExplorationPercent.text = $"{curQuest.ExplorationPercent}%";
        
        UpdateProgressBar(curQuest.ExplorationPercent/100f);
        
        //4)添加按钮事件
        btnQuest.onClick.AddListener(() => QuestManager.Instance.SelectQuest(questID));
    }
    
    public void UpdateProgressBar(float percent)
    {
        // 限制进度值范围
        percent = Mathf.Clamp01(percent);

        // 设置进度条填充量
        progressBarImage.fillAmount = percent;

        // 根据进度调整颜色
        if (percent <= 0.7f)
        {
            progressBarImage.color = colorLow; // 纯白色
        }
        else
        {
            // 计算从70%到100%的渐变比例（0~1）
            float t = (percent - 0.7f) / 0.3f;
            progressBarImage.color = Color.Lerp(colorLow, colorHigh, t);
        }
    }
}
