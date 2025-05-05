using UnityEngine;
using UnityEngine.UI;

public class L2Step1Tutorial:TutorialStepBase
{
    Image tutorialBG;
    ParticleSystem fxArrow;
    ParticleSystem fxDoubleClick;
    Dialogue _dialogue;
    GameObject _btnBag;
    InventoryData _InventoryData;
    
    public L2Step1Tutorial(TutorialController controller, Image _tutorialBG, ParticleSystem _fxArrow,ParticleSystem _fxDoubleClick)
        : base(controller)
    {
        tutorialBG = _tutorialBG;
        fxArrow = _fxArrow;
        fxDoubleClick = _fxDoubleClick;
    }

    public override void Enter()
    {
        _btnBag = EternalCavans.Instance.BagButtonGO;
        _dialogue = EternalCavans.Instance.DialogueSC;
        _InventoryData = InventoryManager.Instance._InventoryData;
        GM.Root.GlobalTickerMgr.OnUpdate += Update;
    }

    void Update()
    {
        if (_InventoryData.BagGems.Count > 0 && GM.Root.QuestMgr.currentQuest.ID == 2)
        {
            GM.Root.GlobalTickerMgr.OnUpdate -= Update;
            BeginTutorial();
        }
    }

    void BeginTutorial()
    {
        _dialogue.LoadDialogue("邓肯教学双击宝石1");
        _dialogue.OnDialogueEnd += OpenBag;
    }

    //双击快捷装备宝石
    void OpenBag()
    {
        _dialogue.OnDialogueEnd -= OpenBag;
        //1）锁定
        EternalCavans.Instance.TutorialFightLock = true;
        //2）设置引导状态
        tutorialBG.enabled = true;
        TutoConfig.SetArrow(fxArrow,EternalCavans.Instance.btnBag_Apos.position);
        TutoConfig.SetTutoHigh(_btnBag,0.07f);
        //3)注册事件
        _btnBag.GetComponent<Button>().onClick.AddListener(Dia2);
    }

    void Dia2()
    {
        //1)移除监听
        _btnBag.GetComponent<Button>().onClick.RemoveListener(Dia2);
        fxArrow.Clear();
        fxArrow.Stop();
        tutorialBG.enabled = false;
        //2)锁定
        EternalCavans.Instance.TutorialDragGemLock = true;
        EternalCavans.Instance.TutoriaSwichBulletLock = true;
        EternalCavans.Instance.TutorialCloseBagLock = true;
        //2)
        _dialogue.LoadDialogue("邓肯教学双击宝石2");
        _dialogue.OnDialogueEnd += DoubleClick;
    }

    void DoubleClick()
    {
        //1）取消
        _dialogue.OnDialogueEnd -= DoubleClick;
        tutorialBG.enabled = true;
        EternalCavans.Instance.TutorialDragGemLock = false;
        EternalCavans.Instance.TutoriaSwichBulletLock = false;
        EternalCavans.Instance.TutorialCloseBagLock = false;
        //3)设置引导状态
        GameObject slotGO = SlotManager.GetSlotGO(1, SlotType.GemBagSlot);
        TutoConfig.SetTutoHigh(slotGO,0.06f);
        TutoConfig.SetArrow(fxArrow,slotGO.transform.position+ TutoConfig.arrowOffset);
        Vector3 doubleOffet = new (1.6f,0.2f,0);
        TutoConfig.SetArrow(fxDoubleClick,slotGO.transform.position + doubleOffet);
        //4)注册下一步
        GM.Root.GlobalTickerMgr.OnUpdate += Update2;
    }
    
    void Update2()
    {
        if (_InventoryData.EquipGems.Count > 0)
        {
            GM.Root.GlobalTickerMgr.OnUpdate -= Update2;
            EndTutorial();
        }
    }

    void EndTutorial()
    {
        tutorialBG.enabled = false;
        EternalCavans.Instance.TutorialDragGemLock = false;
        EternalCavans.Instance.TutoriaSwichBulletLock = false;
        EternalCavans.Instance.TutorialCloseBagLock = false;
        EternalCavans.Instance.TutorialFightLock = false;
        fxArrow.Clear();
        fxArrow.Stop();
        fxDoubleClick.Clear();
        fxDoubleClick.Stop();
        Exit();
        controller.NextStep();
        TutorialCompletionStatus curStatus = GM.Root.PlayerMgr._PlayerData._TutorialCompletionStatus;
        curStatus.L2Step1 = true; //标记当前步骤完成
        curStatus.L2 = true;
    }

    public override void Exit() {}      
}