using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

//管理步骤状态机
public class TutorialController
{
    Queue<TutorialStepBase> stepsQueue;
    TutorialStepBase currentStep;

    public TutorialController()
    {
        stepsQueue = new Queue<TutorialStepBase>();
    }

    public void AddStep(TutorialStepBase step) => stepsQueue.Enqueue(step);
    public void StartTutorial() => NextStep();
    public void NextStep()
    {
        currentStep?.Exit();

        if (stepsQueue.Count > 0)
        {
            currentStep = stepsQueue.Dequeue();
            currentStep.Enter();
        }
        else
            EndTutorial();
    }

    //新手引导全部完成逻辑
    void EndTutorial() {}
}

//Step基类
public abstract class TutorialStepBase
{
    protected TutorialController controller;
    public abstract void Enter();
    public abstract void Exit();

    public TutorialStepBase(TutorialController controller)
    {
        this.controller = controller;
    }
}
