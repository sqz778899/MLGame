using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestState
{
    Locked = 0,
    NotStarted = 1,
    InProgress = 2,
    Completed = 3,
    Failed = 4
}


[Serializable]
public class Quest
{
    public int ID;                // 唯一ID
    public string Name;              // 任务名称
    public int Level;               // 任务等级
    [TextArea] 
    public string Description;       // 任务描述
    public QuestState State;         // 任务状态
    public bool IsCompleted;         // 是否完成过
    public int DifficultyLevel;         // 难度等级（用于调整怪物或地图状态）
    public int TotalScore;             //历史最高总分
    public int TotalLoopCount;         //历史最高循环次数
    public int ExplorationPercent;   //探索进度

    //初始化策划配置，不变的状态
    public void InitQuest(int _id = -1)
    {
        if (_id == -1)
            _id = ID;
        else
            ID = _id;
        QuestJson questDesign = TrunkManager.Instance.GetQuestJson(_id);
        Name = questDesign.Name;
        Level = questDesign.Level;
        Description = questDesign.Description;
    }
    
    //读取存档，变化的状态
    public void LoadQuest(QuestSaveData data)
    {
        if (ID == data.ID)
        {
            InitQuest();
            State = data.State;
            DifficultyLevel = data.DifficultyLevel;
            IsCompleted = data.IsCompleted;
            TotalScore = data.TotalScore;
            TotalLoopCount = data.TotalLoopCount;
            ExplorationPercent = data.ExplorationPercent;
        }
    }
    
    public Quest(int _id = -1)
    {
        InitQuest(_id);
        State = QuestState.NotStarted;
        DifficultyLevel = 0;
        IsCompleted = false;
        TotalScore = 0;
        TotalLoopCount = 0;
        ExplorationPercent = 0;
    }
}

