using System;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager2 : MonoBehaviour
{
    public Image TutorialBG;
    public ParticleSystem FXArrow;
    public ParticleSystem FXHand;
    [Header("AD&Space教学提示资产")]
    public GameObject KeyBoardGO;
    public GameObject btnSure;
    public Transform SureAPos;
    public GameObject KeySpaceGO;

    TutorialController tutorialController;

    void Start()
    {
        CloseFX();
        tutorialController = new TutorialController();
        
        //构造引导序列
        tutorialController.AddStep(new L1Step1PickBullet(tutorialController, TutorialBG, FXArrow));
        tutorialController.AddStep(new L1Step2EquipBullet(tutorialController, TutorialBG, FXArrow,FXHand));
        tutorialController.AddStep(new L1Step3Battle(tutorialController,
            TutorialBG, FXArrow,KeyBoardGO,btnSure,SureAPos,KeySpaceGO));
        tutorialController.AddStep(new L1Step4EquipGem(tutorialController, TutorialBG, FXArrow,FXHand));
        
        /*tutorialController.AddStep(new StepEquipBullet(tutorialController, ...));
        tutorialController.AddStep(new StepCombat(tutorialController, ...));
        tutorialController.AddStep(new StepInlayGem(tutorialController, ...));*/

        tutorialController.StartTutorial();
    }

    void CloseFX()
    {
        FXHand.Clear();
        FXHand.Stop();
    }
}