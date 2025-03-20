using System;
using UnityEngine;

public class TutorialManager2 : MonoBehaviour
{
    public GameObject BulletFloor1;
    public GameObject PickBulletArrow;
    //...其他资源引用

    TutorialController tutorialController;

    void Start()
    {
        tutorialController = new TutorialController();

        //构造引导序列
        tutorialController.AddStep(new StepPickBullet(tutorialController, BulletFloor1, PickBulletArrow));
        /*tutorialController.AddStep(new StepEquipBullet(tutorialController, ...));
        tutorialController.AddStep(new StepCombat(tutorialController, ...));
        tutorialController.AddStep(new StepInlayGem(tutorialController, ...));*/

        tutorialController.StartTutorial();
    }
}