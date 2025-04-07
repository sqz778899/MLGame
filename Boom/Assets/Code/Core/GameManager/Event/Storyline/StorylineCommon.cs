using System;

public class StorylineNode
{
    public int ID;
    public string Name;
    public Func<bool> TriggerCondition; // 满足条件就启动剧情
    public Action OnStart;              // 剧情内容
    public Action OnComplete;           // 剧情结束后的处理
    public StorylineState State;
    
    public bool CanTrigger() => State == StorylineState.Inactive && TriggerCondition();
}

public interface IStorylineNodeBuilder
{
    StorylineNode Build();
}

public enum StorylineState
{
    Inactive = 0,  // 等待触发
    Active = 1,    // 正在进行中
    Completed = 2  // 已完成
}