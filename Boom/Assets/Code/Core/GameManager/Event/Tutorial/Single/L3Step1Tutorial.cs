using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class L3Step1Tutorial:TutorialStepBase
{
    Image tutorialBG;
    ParticleSystem fxArrow;
    Dialogue _dialogue;
    GameObject _btnBag;
    InventoryData _InventoryData;
    BulletInvData _BulletInvData;
    
    public L3Step1Tutorial(TutorialController controller, Image _tutorialBG, ParticleSystem _fxArrow)
        : base(controller)
    {
        tutorialBG = _tutorialBG;
        fxArrow = _fxArrow;
    }

    public override void Enter()
    {
        _InventoryData = InventoryManager.Instance._InventoryData;
        _BulletInvData = InventoryManager.Instance._BulletInvData;
        _dialogue = EternalCavans.Instance.DialogueSC;
        GlobalTicker.Instance.OnUpdate += Update;
    }

    void Update()
    {
        if ((_InventoryData.BagGems.Count + _InventoryData.EquipGems.Count) == 2 &&
            _BulletInvData.BagBulletSpawners[0].SpawnerCount + _BulletInvData.EquipBullets.Count == 2 &&
            QuestManager.Instance.currentQuest.ID == 3)
        {
            GlobalTicker.Instance.OnUpdate -= Update;
            BeginTutorial();
        }
    }
    
    void BeginTutorial()
    {
        _dialogue.LoadDialogue("邓肯教学共振1");
        _btnBag = EternalCavans.Instance.btnBag;
        _btnBag.GetComponent<Button>().onClick.AddListener(OpenBag);
    }

    //教授会帮助自动装备好共振
    void OpenBag()
    {
        //1)解绑
        _btnBag.GetComponent<Button>().onClick.RemoveListener(OpenBag);
        //2)自动装备好共振
        _BulletInvData.ClearData();
        ISlotController bslot01 = SlotManager.GetSlotController(1, SlotType.CurBulletSlot);
        ISlotController bslot02 = SlotManager.GetSlotController(2, SlotType.CurBulletSlot);
        _BulletInvData.EquipBullet(new BulletData(1, bslot01 as BulletSlotController));
        _BulletInvData.EquipBullet(new BulletData(1, bslot02 as BulletSlotController));

        _InventoryData.ClearData();
        ISlotController slot01 = SlotManager.GetSlotController(3, SlotType.GemInlaySlot);
        ISlotController slot02 = SlotManager.GetSlotController(6, SlotType.GemInlaySlot);
        _InventoryData.EquipGem(new GemData(20,slot01 as GemSlotController));
        _InventoryData.EquipGem(new GemData(20,slot02 as GemSlotController));

        InventoryManager.Instance.InitAllBagGO();
        //3)设置背景板状态
        tutorialBG.enabled = true;
        Transform[] trans = EternalCavans.Instance.DialogueRoot.GetComponentsInChildren<Transform>(true);
        Transform imgBG = trans.FirstOrDefault(t => t.name == "imgBG");
        TutoConfig.SetTutoHigh(imgBG.gameObject,0.4f);
        _dialogue.LoadDialogue("邓肯教学共振2",true);
        _dialogue.OnDialogueEnd += EndTutorial;
    }

    void EndTutorial()
    {
        _dialogue.OnDialogueEnd -= EndTutorial;
        tutorialBG.enabled = false;
        Exit();
        controller.NextStep();
        TutorialCompletionStatus curStatus = PlayerManager.Instance._PlayerData._TutorialCompletionStatus;
        curStatus.L3Step1 = true; //标记当前步骤完成
        curStatus.L3 = true;
    }

    public override void Exit() {}
}