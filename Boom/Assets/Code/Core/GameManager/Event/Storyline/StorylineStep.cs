using System.Collections.Generic;
using UnityEngine;

public class StorylineController
{
    Queue<StorylineStepBase> stepsQueue = new();
    StorylineStepBase currentStep;

    public StorylineController()
    {
        GlobalTicker.Instance.OnUpdate += Update;
    }

    public void Dispose() =>  GlobalTicker.Instance.OnUpdate -= Update;
   
    public void AddStep(StorylineStepBase step) => stepsQueue.Enqueue(step);
    public void StartQuest() => NextStep();
    
    
    public void Update()
    {
        if (currentStep != null && currentStep.CheckComplete())
        {
            StepCompleted();
        }
    }
    
    public void StepCompleted()
    {
        currentStep.Exit();
        NextStep();
    }

    void NextStep()
    {
        if (stepsQueue.Count > 0)
        {
            currentStep = stepsQueue.Dequeue();
            currentStep.Enter();
        }
        else
            QuestComplete();
    }

    void QuestComplete() =>  Debug.Log("主线剧情任务完成！");
}

public abstract class StorylineStepBase
{
    protected StorylineController controller;

    public abstract void Enter();     //进入步骤
    public abstract void Exit();      //退出步骤
    public abstract bool CheckComplete();  //检查完成条件

    public StorylineStepBase(StorylineController controller)
    {
        this.controller = controller;
    }
}

public class StepEnterRoom : StorylineStepBase
{
    int targetRoomID;
    public StepEnterRoom(StorylineController controller, int roomID) : base(controller)
    {
        targetRoomID = roomID;
    }

    public override void Enter() => EventManager.OnRoomEntered += OnRoomEntered;

    void OnRoomEntered(int roomID)
    {
        if (roomID == targetRoomID)
        {
            controller.StepCompleted();
        }
    }

    public override void Exit()
    {
        EventManager.OnRoomEntered -= OnRoomEntered;
    }

    public override bool CheckComplete() => false;
}