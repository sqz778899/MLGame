using UnityEngine;
using UnityEngine.UI;
using System.Linq;

//Step1 最开始的新手引导，捡起黏土子弹
public class L1Step1PickBullet : TutorialStepBase
{
    Image tutorialBG;
    ParticleSystem fxArrow;
    MapNodeController _curBullet;

    public L1Step1PickBullet(TutorialController controller, Image _tutorialBG, ParticleSystem _fxArrow)
        : base(controller)
    {
        tutorialBG = _tutorialBG;
        fxArrow = _fxArrow;
    }

    public override void Enter()
    {
        //1)注册事件，结束流程后调用
        EventManager.OnBulletPicked += OnBulletPicked;
        BeginTutorial();
    }
    void BeginTutorial()
    {
        //1)设置引导板状态
        tutorialBG.enabled = false;
        fxArrow.Clear();
        fxArrow.Stop();
        EternalCavans.Instance.TutorialFightLock = true;
        //2)开启对话
        Dialogue curDia = EternalCavans.Instance.DialogueSC;
        curDia.LoadDialogue("魔法基础大聪明");
        curDia.OnDialogueEnd += PickBullet;
    }

    void PickBullet()
    {
        EternalCavans.Instance.DialogueSC.OnDialogueEnd -= PickBullet;
        UIManager.Instance.IsLockedClick = true;
        //1）开启背景板
        tutorialBG.enabled = true;
        //2）找到需要引导的黏土子弹
        MapRoomNode curRoom = BattleManager.Instance._MapManager.GetMapRoomNode(1);
        _curBullet = curRoom.BulletRes[0];
        _curBullet._view.gameObject.AddComponent<ShaderHoleController>().radius = 0.06f;
        //3）计算一下引导箭头特效的坐标，赋予并显示
        RectTransform arrowRTrans = fxArrow.GetComponent<RectTransform>();
        Vector3 newPos = _curBullet._view.transform.position + TutoConfig.arrowOffset;
        Vector2 UIPos = UTools.GetUISpacePos(newPos,arrowRTrans);
        
        arrowRTrans.anchoredPosition = UIPos;
        arrowRTrans.GetComponent<FloatingIcon>().ResetPos(arrowRTrans.transform.localPosition);
        fxArrow.Play();
        //3)锁定角色，锁定资产
        _curBullet.Locked();
        //4)添加临时点击事件，切换引导状态。
        _curBullet._view.OnClick += JoinYou;
        //5)注册回调事件，进入下一步
        EternalCavans.Instance.DialogueSC.OnDialogueEnd += diaCallback;
    }
    void JoinYou()
    {
        _curBullet._view.OnClick -= JoinYou;
        //1）关闭各种宏
        _curBullet.UnLocked();
        //2)关闭背景板和引导特效
        tutorialBG.enabled = false;
        fxArrow.Clear();
        fxArrow.Stop();
        //3)销毁ShaderHoleController脚本
        Object.Destroy(_curBullet._view.GetComponent<ShaderHoleController>());
    }
    
    void diaCallback()
    {
        //_curBullet._dialogue.OnDialogueEnd -= diaCallback;
        EventManager.OnBulletPicked?.Invoke();
    }

    void OnBulletPicked()
    {
        Exit();
        controller.NextStep();
        PlayerManager.Instance._PlayerData._TutorialCompletionStatus.L1Step1 = true;
    }

    public override void Exit() => EventManager.OnBulletPicked -= OnBulletPicked;
}

//Step2 装备黏土子弹
public class L1Step2EquipBullet : TutorialStepBase
{
    Image tutorialBG;
    ParticleSystem fxArrow;
    ParticleSystem fXHand;
    
    GameObject _btnBag; //背包按钮
    GameObject _btnSWBullet;//子弹页签切换的按钮
    GameObject _btnStart;//开始按钮
    
    public L1Step2EquipBullet(TutorialController controller, Image _tutorialBG, ParticleSystem _fxArrow,ParticleSystem _fxHand)
        : base(controller)
    {
        tutorialBG = _tutorialBG;
        fxArrow = _fxArrow;
        fXHand = _fxHand;
    }

    public override void Enter()
    {
        //1)设置引导板状态
        tutorialBG.enabled = true;
        //2)设置按钮高亮&&引导箭头
        _btnBag = EternalCavans.Instance.BagButtonGO;
        _btnBag.AddComponent<ShaderHoleController>().radius = 0.08f;
        TutoConfig.SetArrow(fxArrow,EternalCavans.Instance.btnBag_Apos.position);
        //3)给按钮注册引导事件
        _btnBag.GetComponent<Button>().onClick.AddListener(OpenBag);
        //4)注册结束事件
        EventManager.OnBulletEquipped += OnBulletEquipped;
        GM.Root.GlobalTickerMgr.OnUpdate += Update;
    }

    void OpenBag()
    {
        _btnBag.GetComponent<Button>().onClick.RemoveListener(OpenBag);
        //1)设置按钮高亮&&引导箭头
        _btnSWBullet = EternalCavans.Instance.btnSWBullet;
        _btnSWBullet.AddComponent<ShaderHoleController>().radius = 0.08f;
        TutoConfig.SetArrow(fxArrow,EternalCavans.Instance.btnSWBullet_Apos.position);
        //2)给按钮注册引导事件
        _btnSWBullet.GetComponent<Button>().onClick.AddListener(EquipBullet);
        //3)销毁ShaderHoleController脚本
        Object.Destroy(_btnBag.GetComponent<ShaderHoleController>());
    }

    void EquipBullet()
    {
        _btnSWBullet.GetComponent<Button>().onClick.RemoveListener(EquipBullet);
        fxArrow.Clear();
        fxArrow.Stop();
        EternalCavans.Instance.TutorialSwichGemLock = true; 
        EternalCavans.Instance.TutorialCloseBagLock = true; 
        //1)设置槽位高亮
        HandPointMove tsc = fXHand.gameObject.GetComponent<HandPointMove>();
        //2)小手平移特效
        GameObject bulletSlot01 = SlotManager.GetSlotGO(1, SlotType.BulletSlot);
        GameObject EquipSlot01 = SlotManager.GetSlotGO(1, SlotType.CurBulletSlot);
        bulletSlot01.AddComponent<ShaderHoleController>().radius = 0.08f;
        tsc.startTrans = bulletSlot01.transform;
        tsc.endTrans = EquipSlot01.transform;
        tsc.ResetPos();
        fXHand.Play();
    }
    
    public void Update()
    {
        if (InventoryManager.Instance._BulletInvData.EquipBullets.Count > 0) { ReturnToRoom(); }
    }

    void ReturnToRoom()
    {
        GM.Root.GlobalTickerMgr.OnUpdate -= Update;
        EternalCavans.Instance.TutorialSwichGemLock = false; 
        EternalCavans.Instance.TutorialCloseBagLock = false; 
        //1）清空之前的状态
        fXHand.Clear();
        fXHand.Stop();
        GameObject bulletSlot01 = SlotManager.GetSlotGO(1, SlotType.BulletSlot);
        Object.Destroy(bulletSlot01.GetComponent<ShaderHoleController>());
        //2）设置按钮高亮&&引导箭头
        _btnStart = EternalCavans.Instance.btnStart;
        _btnStart.AddComponent<ShaderHoleController>().radius = 0.2f;
        TutoConfig.SetArrow(fxArrow,EternalCavans.Instance.btnStart_Apos.position);
        //2)给按钮注册引导事件
        _btnStart.GetComponent<Button>().onClick.AddListener(TheEnd);
    }

    void TheEnd()
    {
        _btnStart.GetComponent<Button>().onClick.RemoveListener(TheEnd);
        tutorialBG.enabled = false;
        fxArrow.Clear();
        fxArrow.Stop();
        EternalCavans.Instance.TutorialFightLock = false;//这时候才允许进入战斗
        EventManager.OnBulletEquipped?.Invoke();
    }

    void OnBulletEquipped()
    {
        Exit();
        controller.NextStep();
        PlayerManager.Instance._PlayerData._TutorialCompletionStatus.L1Step2 = true;
    }

    public override void Exit() => EventManager.OnBulletEquipped -= OnBulletEquipped;
}

//Step3 战斗
public class L1Step3Battle : TutorialStepBase
{
    Image tutorialBG;
    ParticleSystem fxArrow;
    GameObject keyBoardGO;
    GameObject btnSure;
    Transform sureAPos;
    GameObject keySpaceGO;
    
    public L1Step3Battle(TutorialController controller, Image _tutorialBG, 
        ParticleSystem _fxArrow,GameObject _keyBoardGO,
        GameObject _btnSure,Transform _sureAPos,GameObject _keySpaceGO)
        : base(controller)
    {
        tutorialBG = _tutorialBG;
        fxArrow = _fxArrow;
        keyBoardGO = _keyBoardGO;
        btnSure = _btnSure;
        sureAPos = _sureAPos;
        keySpaceGO = _keySpaceGO;
    }

    public override void Enter()
    {
        EventManager.OnFirstBattleEnd += OnFirstBattleEnd;
        GM.Root.GlobalTickerMgr.OnUpdate += Update;
    }
    
    public void Update()
    {
        if (BattleManager.Instance.IsInBattle) BeginBattle();
    }

    void BeginBattle()
    {
        GM.Root.GlobalTickerMgr.OnUpdate -= Update;
        //1)设置各种高亮状态
        tutorialBG.enabled = true;
        keyBoardGO.SetActive(true);
        keyBoardGO.AddComponent<ShaderHoleController>().radius = 0.12f;
        TutoConfig.SetArrow(fxArrow,sureAPos.position);
        //2)注册事件
        btnSure.GetComponent<Button>().onClick.AddListener(PressSpace);
    }

    //按下空格键
    void PressSpace()
    {
        btnSure.GetComponent<Button>().onClick.RemoveListener(PressSpace);
        GameObject.Destroy(keyBoardGO);//后面没用了
        fxArrow.Clear();
        fxArrow.Stop();
        //1)设置各种高亮状态
        keySpaceGO.SetActive(true);
        keySpaceGO.transform.GetChild(0).gameObject.AddComponent<ShaderHoleController>().radius = 0.08f;
        //2)注册监听
        GM.Root.GlobalTickerMgr.OnUpdate += Update2;
    }

    void Update2()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GM.Root.GlobalTickerMgr.OnUpdate -= Update2;
            Pressed();
        }
    }

    void Pressed()
    {
        tutorialBG.enabled = false;
        GameObject.Destroy(keySpaceGO);//后面没用了
        EventManager.OnFirstBattleEnd.Invoke();
    }

    void OnFirstBattleEnd()
    {
        Exit();
        controller.NextStep();
        PlayerManager.Instance._PlayerData._TutorialCompletionStatus.L1Step3 = true;
    }

    public override void Exit() => EventManager.OnFirstBattleEnd -= OnFirstBattleEnd;
}

//Step4 镶嵌宝石
public class L1Step4EquipGem : TutorialStepBase
{
    Image tutorialBG;
    ParticleSystem fxArrow;
    ParticleSystem fXHand;
    
    MapNodeController _curBox; //宝箱
    GameObject _btnBag; //背包按钮
    GameObject _btnSWGem;//宝石页签切换的按钮
    GameObject _btnStart;//开始按钮
    
    public L1Step4EquipGem(TutorialController controller, Image _tutorialBG, 
        ParticleSystem _fxArrow,ParticleSystem _fxHand)
        : base(controller)
    {
        tutorialBG = _tutorialBG;
        fxArrow = _fxArrow;
        fXHand = _fxHand;
    }

    public override void Enter()
    {
        EventManager.OnGemEquipped += OnGemEquipped;
        GM.Root.GlobalTickerMgr.OnUpdate += Update;
    }
    
    public void Update()
    {
        if (BattleManager.Instance._MapManager == null) return;
        MapRoomNode CurRoom = BattleManager.Instance._MapManager.GetMapRoomNode(2);
        if (CurRoom.IsFogUnLocked)
        {
            GM.Root.GlobalTickerMgr.OnUpdate -= Update;
            OpenBox();
        }
    }

    public void OpenBox()
    {
        //1）开启背景板
        tutorialBG.enabled = true;
        UIManager.Instance.IsLockedClick = true;
        //2）找到需要开启的宝箱
        _curBox = PlayerManager.Instance.RoleInMapSC.CurRoom.Treasures[0];
        _curBox._view.gameObject.AddComponent<ShaderHoleController>().radius = 0.06f;
        //3）计算一下引导箭头特效的坐标，赋予并显示
        RectTransform arrowRTrans = fxArrow.GetComponent<RectTransform>();
        Vector3 newPos = _curBox._view.transform.position + TutoConfig.arrowOffset;
        Vector2 UIPos = UTools.GetUISpacePos(newPos,arrowRTrans);
        arrowRTrans.anchoredPosition = UIPos;
        arrowRTrans.GetComponent<FloatingIcon>().ResetPos(arrowRTrans.transform.localPosition);
        fxArrow.Play();
        //3)锁定角色，锁定资产
        _curBox.Locked();
        //4)添加临时点击事件
        //_curBox.onClick.AddListener(OpenBag);
    }

    public void OpenBag()
    {
        //_curBox.onClick.RemoveListener(OpenBag);
        //1）关闭各种宏
        _curBox.UnLocked();
        UIManager.Instance.IsLockedClick = false;
        //2)背包按钮
        _btnBag = EternalCavans.Instance.BagButtonGO;
        _btnBag.AddComponent<ShaderHoleController>().radius = 0.08f;
        TutoConfig.SetArrow(fxArrow,EternalCavans.Instance.btnBag_Apos.position);
        //3)给按钮注册引导事件
        _btnBag.GetComponent<Button>().onClick.AddListener(SwitchGem);
    }

    public void SwitchGem()
    {
        _btnBag.GetComponent<Button>().onClick.RemoveListener(SwitchGem);
        Object.Destroy(_btnBag.GetComponent<ShaderHoleController>());
        //1)设置按钮高亮&&引导箭头
        _btnSWGem = EternalCavans.Instance.btnSWGem;
        _btnSWGem.AddComponent<ShaderHoleController>().radius = 0.08f;
        TutoConfig.SetArrow(fxArrow,EternalCavans.Instance.btnSWGem_Apos.position);
        //3)锁定其他操作
        EternalCavans.Instance.TutoriaSwichBulletLock = true;
        EternalCavans.Instance.TutorialCloseBagLock = true;
        //2)给按钮注册引导事件
        _btnSWGem.GetComponent<Button>().onClick.AddListener(EquipGem);
    }

    public void EquipGem()
    {
        fxArrow.Clear();
        fxArrow.Stop();
        _btnSWGem.GetComponent<Button>().onClick.RemoveListener(EquipGem);
        EternalCavans.Instance.TutoriaSwichBulletLock = false;
        EternalCavans.Instance.TutorialCloseBagLock = false;
        //1)设置槽位高亮
        HandPointMove tsc = fXHand.gameObject.GetComponent<HandPointMove>();
        //2)小手平移特效
        GameObject gemSlot01 = SlotManager.GetSlotGO(1, SlotType.GemBagSlot).gameObject;
        GameObject equipGemSlot01 = SlotManager.GetSlotGO(3, SlotType.GemInlaySlot).gameObject;
        equipGemSlot01.AddComponent<ShaderHoleController>().radius = 0.05f;
        fXHand.transform.position = gemSlot01.transform.position;
        tsc.startTrans = gemSlot01.transform;
        tsc.endTrans = equipGemSlot01.transform;
        tsc.ResetPos();
        fXHand.Play();
        GM.Root.GlobalTickerMgr.OnUpdate += Update2;
    }
    
    public void Update2()
    {
        if (InventoryManager.Instance._InventoryData.EquipGems.Count > 0)
        {
            ReturnToRoom();
        }
    }
    
    void ReturnToRoom()
    {
        GM.Root.GlobalTickerMgr.OnUpdate -= Update2;
        //1）清空之前的状态
        fXHand.Clear();
        fXHand.Stop();
        GameObject bulletSlot01 = SlotManager.GetSlotGO(1, SlotType.GemBagSlot).gameObject;
        Object.Destroy(bulletSlot01.GetComponent<ShaderHoleController>());
        //2）设置按钮高亮&&引导箭头
        _btnStart = EternalCavans.Instance.btnStart;
        _btnStart.AddComponent<ShaderHoleController>().radius = 0.2f;
        TutoConfig.SetArrow(fxArrow,EternalCavans.Instance.btnStart_Apos.position);
        //2)给按钮注册引导事件
        _btnStart.GetComponent<Button>().onClick.AddListener(TheEnd);
    }
    
    void TheEnd()
    {
        _btnStart.GetComponent<Button>().onClick.RemoveListener(TheEnd);
        tutorialBG.enabled = false;
        fxArrow.Clear();
        fxArrow.Stop();
        EventManager.OnGemEquipped?.Invoke();
    }

    void OnGemEquipped()
    {
        Exit();
        controller.NextStep();
        PlayerManager.Instance._PlayerData._TutorialCompletionStatus.L1Step4 = true;
    }

    public override void Exit() => EventManager.OnGemEquipped -= OnGemEquipped;
}

//Step5 战斗内拖拽子弹
public class L1Step5DragBullet : TutorialStepBase
{
    Image tutorialBG;
    ParticleSystem fxArrow;
    ParticleSystem fXHand;

    public L1Step5DragBullet(TutorialController controller, Image _tutorialBG,
        ParticleSystem _fxArrow,ParticleSystem _fxHand)
        : base(controller)
    {
        tutorialBG = _tutorialBG;
        fxArrow = _fxArrow;
        fXHand = _fxHand;
        bagRootMini = EternalCavans.Instance.BagRootMini.GetComponent<BagRootMini>();
    }

    public override void Enter()
    {
        GM.Root.GlobalTickerMgr.OnUpdate += Update;
    }
    
    void Update()
    {
        if (BattleManager.Instance._MapManager == null) return;
        if (BattleManager.Instance._MapManager.CurMapSate.CurRoomID == 5)
        {
            if (BattleManager.Instance.IsInBattle)
            {
                GM.Root.GlobalTickerMgr.OnUpdate -= Update;
                BeginDrag();
            }
        }
    }

    GameObject tmpStart;
    GameObject tmpEnd;
    GameObject slotGO;
    BagRootMini bagRootMini;
    void BeginDrag()
    {
        //1)设置各种高亮状态
        tutorialBG.enabled = true;
        fxArrow.Clear();
        fxArrow.Stop();
        //2)找到Spawner
        ISlotController[] spawners = SlotManager.GetAllSlotController(SlotType.SpawnnerSlotInner);
        BulletSlotSpawnerController c = spawners.FirstOrDefault(s => s.SlotID == 1) as BulletSlotSpawnerController;
        slotGO = c._view.gameObject;
        TutoConfig.SetTutoHigh(slotGO,0.07f);
        //2)小手平移特效
        HandPointMove tsc = fXHand.gameObject.GetComponent<HandPointMove>();
        fXHand.transform.position = slotGO.transform.position;
        tsc.startTrans = slotGO.transform;
        Vector3 slotPos = new Vector3(-4.13f, -1.69f, 90f); 
        tmpStart = new GameObject("tmpStart");
        tmpStart.transform.position = slotPos;
        tsc.startTrans = tmpStart.transform;
        Vector3 offset = new Vector3(4f,2f,0);
        tmpEnd = new GameObject("tmp");
        tmpEnd.transform.position = slotPos + offset;
        tsc.endTrans = tmpEnd.transform;
        tsc.ResetPos();
        fXHand.Play();
        
        GM.Root.GlobalTickerMgr.OnUpdate += Update2;
    }
    
    void Update2()
    {
        if (bagRootMini.IsCameraNear)
        {
            GM.Root.GlobalTickerMgr.OnUpdate -= Update2;
            fXHand.Clear();
            fXHand.Stop();
            tutorialBG.enabled = false;
            OnDragged();
        }
    }
    
    void OnDragged()
    {
        TutorialCompletionStatus curStatus = PlayerManager.Instance._PlayerData._TutorialCompletionStatus;
        curStatus.L1 = true;
        curStatus.L1Step5 = true;
        Exit();
        controller.NextStep();
    }

    public override void Exit() {}
}