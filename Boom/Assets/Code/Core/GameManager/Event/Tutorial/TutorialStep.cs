using System.Collections.Generic;
using UnityEngine;


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

//具体引导步骤继承基类
public class StepPickBullet : TutorialStepBase
{
    GameObject bullet;
    GameObject arrow;

    public StepPickBullet(TutorialController controller, GameObject bullet, GameObject arrow)
        : base(controller)
    {
        this.bullet = bullet;
        this.arrow = arrow;
    }

    public override void Enter()
    {
        UIManager.Instance.IsLockedClick = true;
        bullet.AddComponent<ShaderHoleController>().radius = 0.06f;
        arrow.SetActive(true);

        EventManager.OnBulletPicked += OnBulletPicked;
    }

    void OnBulletPicked(int bulletID)
    {
        Exit();
        controller.NextStep();
    }

    public override void Exit()
    {
        UIManager.Instance.IsLockedClick = false;
        Object.Destroy(bullet.GetComponent<ShaderHoleController>());
        arrow.SetActive(false);

        EventManager.OnBulletPicked -= OnBulletPicked;
    }
}
