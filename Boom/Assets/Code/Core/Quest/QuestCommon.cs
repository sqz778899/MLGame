using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestState
{
    NotStarted,
    InProgress,
    Completed
}

[System.Serializable]
public class Quest
{
    public string ID;               // 唯一ID
    public string Name;             // 任务名称
    [TextArea] 
    public string Description;      // 任务描述
    public string SceneName;        // 要加载的场景名称
    public QuestState State;        // 任务状态
    // public Reward reward;        // 后续扩展：奖励类（自定义）
}

