using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public Image TutorialBG;
    public ParticleSystem FXArrow;
    public ParticleSystem FXHand;
    public ParticleSystem DoubleClick;
    [Header("AD&Space教学提示资产")]
    public GameObject KeyBoardGO;
    public GameObject btnSure;
    public Transform SureAPos;
    public GameObject KeySpaceGO;
    TutorialController tutorialController;

    void Start()
    {
        CloseFX();
        if (GM.Root.IsSkipStorylineMode)
            return;
        tutorialController = new TutorialController();
        
        //1）构造教学关卡1的引导序列
        TutorialCompletionStatus curStatus = PlayerManager.Instance._PlayerData._TutorialCompletionStatus;
        if (curStatus.L1 == false)
        {
            if (curStatus.L1Step1 == false)
                tutorialController.AddStep(new L1Step1PickBullet(tutorialController, TutorialBG, FXArrow));
            if (curStatus.L1Step2 == false)
                tutorialController.AddStep(new L1Step2EquipBullet(tutorialController, TutorialBG, FXArrow,FXHand));
            if (curStatus.L1Step3 == false)
                tutorialController.AddStep(new L1Step3Battle(tutorialController, TutorialBG, FXArrow,KeyBoardGO,btnSure,SureAPos,KeySpaceGO));
            if (curStatus.L1Step4 == false)
                tutorialController.AddStep(new L1Step4EquipGem(tutorialController, TutorialBG, FXArrow,FXHand));
            if (curStatus.L1Step5 == false)
                tutorialController.AddStep(new L1Step5DragBullet(tutorialController, TutorialBG, FXArrow,FXHand));
        }
        
        //2）构造教学关卡2的引导序列
        if (curStatus.L1 && curStatus.L2 == false)
        {
            if (curStatus.L2Step1 == false)
                tutorialController.AddStep(new L2Step1Tutorial(tutorialController, TutorialBG, FXArrow,DoubleClick));
        }
        
        //3）构造教学关卡3的引导序列
        if (curStatus.L2 && curStatus.L3 == false)
        {
            if (curStatus.L3Step1 == false)
                tutorialController.AddStep(new L3Step1Tutorial(tutorialController, TutorialBG, FXArrow));
        }
        
        tutorialController.StartTutorial();
    }

    void CloseFX()
    {
        TutorialBG.enabled = false;
        ParticleSystem[] allfx = GetComponentsInChildren<ParticleSystem>(true);
        allfx.ForEach(fx => { fx.Clear();fx.Stop();});
    }
}