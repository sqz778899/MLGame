using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Header("基础资产")] 
    public bool IsJampTutorial = false;
    public Image bgImage;
    [Header("资源类资产")]
    public GameObject BulletFloor1;
    public GameObject TreasureBoxFloor2;
    [Header("箭头资产")]
    public GameObject ArrowFloor1;
    public List<GameObject> CrossRoomArrowsFloor2;
    public List<MapRoomNode> AwardRoomsFloor2;
    [Header("UI资产")]
    public GameObject UIBag;
    public GameObject UIBagBulletTab;
    public GameObject UIBulletSlot1;
    public GameObject UIStart;
    public GameObject UIKeyBoard;
    public GameObject UIPressEnter;
    //Step4
    public GameObject UIFightContinue;
    public GameObject UIBagGemTab;
    public GameObject UIGemSlot;
    
    [Header("特效资产")]
    public GameObject PickBullet01Arrow;
    public GameObject BagFXArrow;
    public GameObject BagBulletTabFXArrow;
    public GameObject SlotFXArrow;
    public GameObject StartFXArrow;
    public GameObject OBHPFXArrow;
    //镶嵌宝石
    public GameObject TreasureBoxArrow;
    public GameObject ArrowInlayGemToBag;
    public GameObject ArrowGemTab;
    public GameObject HandPointInlayGem;
    
    [Header("脚本类")]
    public Dialogue CurDialogue;
    public FightLogic FightLogic;
    RoleInMap curRole;
    RoleInMap _curRole
    {
        get
        {
            if (curRole == null)
                curRole = MainRoleManager.Instance.MainRoleIns.GetComponent<RoleInMap>();
            return curRole;
        }
    }
    
    //临时监听的变量
    bool _isSetBullet = false;
    bool _isPressEnter3_3 = false;
    bool _isOpenBox = false;
    bool _isAwardRoom5_1 = false;
    bool _isSetGem = false;
    
    void Start()
    {
        if (IsJampTutorial)
        {
            Destroy(this.gameObject);
            return;
        }
        Step1();
        LockedNeedNodes();
    }

    void Update()
    {
        #region 解锁宝箱
        if (!_isOpenBox)
        {
            if (TreasureBoxFloor2.GetComponent<TreasureNode>().isOpened)
            {
                Step4_1();//打开宝箱之后调用
                _isOpenBox = true;
            }
        }

        if (MainRoleManager.Instance.InLayGems.Count > 0 && !_isSetGem)
        {
            _isSetGem = true;
            Step4_5();
        }
        #endregion
        
        if (!_isAwardRoom5_1) //如果进入横向奖励房间，则终止
        {
            if (AwardRoomsFloor2.Any(each => each.RoomID == MainRoleManager.Instance.CurMapSate.CurRoomID))
            {
                Step5();
                _isAwardRoom5_1 = true;
            }
        }
        if (FightLogic._isAttacked && !_isPressEnter3_3)
        {
            _isPressEnter3_3 = true;
            DestroyImmediate(UIPressEnter);
        }
        
        if (MainRoleManager.Instance.CurBullets.Count > 0 && !_isSetBullet)
        {
            _isSetBullet = true;
            Step2_4();
        }
    }

    void LockedNeedNodes()
    {
        CrossRoomArrowsFloor2.ForEach(each => each.GetComponent<ArrowNode>().TutorialLocked = true);
    }

    #region Step1 引导，捡起黏土子弹
    void Step1() //新手教程对话
    {
        ResetBullet(BulletFloor1);
        CurDialogue.OnDialogueEnd += Step1_1;
        CurDialogue.LoadDialogue("Beginner01");
        _curRole.IsLocked = true;
    }
    void Step1_1() //引导，捡起黏土子弹
    {
        bgImage.enabled = true;
        BulletFloor1.AddComponent<ShaderHoleController>().radius = 0.06f;
        ArrowFloor1.GetComponent<ArrowNode>().IsLocked = true;
        PickBullet01Arrow.SetActive(true);
        PickBullet01Arrow.GetComponent<ParticleSystem>().Play();
        bgImage.enabled = true;
    }
    
    public void Step1_2() //点击黏土子弹触发对话
    {
        UIManager.Instance.IsLockedClick = true;
        _curRole.IsLocked = false;
        bgImage.enabled = false;
        ResetBullet(BulletFloor1);
        DestroyImmediate(PickBullet01Arrow);
        CurDialogue.LoadDialogue("Beginner02");
        CurDialogue.OnDialogueEnd += Step1_3;
    }
    
    void Step1_3() //子弹加入
    {
        UIManager.Instance.IsLockedClick = false;
        BulletFloor1.GetComponent<BulletMapNode>().OnDiaCallBack();
        StartCoroutine(WaitToStep2());
    }
    #endregion

    #region Step2 引导,装备黏土子弹
    IEnumerator WaitToStep2()
    {
        UIManager.Instance.IsLockedClick = true;
        yield return new WaitForSeconds(1f);
        Step2();
    }
    void Step2()  //自言自语，我要装备子弹
    {
        CurDialogue.OnDialogueEnd += Step2_1;
        CurDialogue.LoadDialogue("Beginner03");
    }

    void Step2_1()  //引导，点击背包按钮
    {
        UIManager.Instance.IsLockedClick = false;
        bgImage.enabled = true;
        ShaderHoleController bageSC = UIBag.AddComponent<ShaderHoleController>();
        bageSC.radius = 0.08f;
        BagFXArrow.SetActive(true);
        BagFXArrow.GetComponent<ParticleSystem>().Play();
        
        UIBag.GetComponent<Button>().onClick.AddListener(Step2_2);
    }
    
    void Step2_2() //引导，切换背包菜单
    {
        //清空之前绑定
        DestroyImmediate(BagFXArrow);
        UIBag.GetComponent<Button>().onClick.RemoveListener(Step2_2);
        ShaderHoleController bageSC = UIBag.GetComponent<ShaderHoleController>();
        if (bageSC) DestroyImmediate(bageSC);
        
        //现在Mark
        UIBagBulletTab.AddComponent<ShaderHoleController>().radius = 0.08f;
        BagBulletTabFXArrow.SetActive(true);
        BagBulletTabFXArrow.GetComponent<ParticleSystem>().Play();
        
        //现在绑定
        Button btnBagBulletTab = UIBagBulletTab.GetComponent<Button>(); //背包子弹Tab按钮
        btnBagBulletTab.onClick.AddListener(Step2_3);
    }

    void Step2_3() //小手漂移挪子弹
    {
        //清空之前绑定
        DestroyImmediate(BagBulletTabFXArrow);
        ShaderHoleController bageSC = UIBagBulletTab.GetComponent<ShaderHoleController>();
        if (bageSC) DestroyImmediate(bageSC);
        UIBagBulletTab.GetComponent<Button>().onClick.RemoveListener(Step2_3);
        
        //现在Mark
        UIBulletSlot1.AddComponent<ShaderHoleController>().radius = 0.08f;
        SlotFXArrow.SetActive(true);
        SlotFXArrow.GetComponent<ParticleSystem>().Play();
    }

    void Step2_4() //成功装备子弹
    {
        //_isStartStep2_4 = true;
        //清空之前绑定
        DestroyImmediate(SlotFXArrow);
        ShaderHoleController bageSC = UIBulletSlot1.GetComponent<ShaderHoleController>();
        if (bageSC) DestroyImmediate(bageSC);
        bgImage.enabled = false;
        
        //现在Mark
        StartFXArrow.SetActive(true);
        StartFXArrow.GetComponent<ParticleSystem>().Play();
        UIStart.GetComponent<Button>().onClick.AddListener(Step2_5);
    }

    void Step2_5() //回到房间
    {
        Debug.Log("进入场景！！！");
        //清空之前绑定
        DestroyImmediate(StartFXArrow);
        UIStart.GetComponent<Button>().onClick.RemoveListener(Step2_5);
        ArrowFloor1.GetComponent<ArrowNode>().IsLocked = false;
        
        //绑定新的流程
        ArrowFloor1.GetComponent<ArrowNode>().onClick.AddListener(Step3);
    }
    #endregion

    #region Step3 战斗
    void Step3()
    {
        UIManager.Instance.IsLockedClick = true;
        bgImage.enabled = true;
        //清空之前绑定
        ArrowFloor1.GetComponent<ArrowNode>().onClick.RemoveListener(Step3);
        
        //现在Mark
        GameObject DialogueRoot = GameObject.Find("DialogueRoot");
        DialogueFight SC = DialogueRoot.GetComponentInChildren<DialogueFight>();
        SC.isLocked = true;
        SC.HPGOList[0].AddComponent<ShaderHoleController>().radius = 0.05f;
        OBHPFXArrow.SetActive(true);
        OBHPFXArrow.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
    }

    public void Step3_1() //外部按钮调用，观察血量很重要
    {
        UIManager.Instance.IsLockedClick = false;
        bgImage.enabled = false;
        //清空之前绑定
        DestroyImmediate(OBHPFXArrow);
        GameObject DialogueRoot = GameObject.Find("DialogueRoot");
        DialogueFight SC = DialogueRoot.GetComponentInChildren<DialogueFight>();
        SC.isLocked = false;
        
        //绑定新的
        SC.BtnFight.onClick.AddListener(Step3_2);
    }

    void Step3_2() //进入战斗中
    {
        UIManager.Instance.IsLockedClick = true;
        bgImage.enabled = true;
        UIKeyBoard.SetActive(true);
    }

    public void Step3_3() //按键教程
    {
        UIManager.Instance.IsLockedClick = false;
        bgImage.enabled = false;
        //解绑之前
        DestroyImmediate(UIKeyBoard);
        //
        UIPressEnter.SetActive(true);
        UIFightContinue.GetComponent<Button>().onClick.AddListener(Step4);
    }
    #endregion

    #region Step4 镶嵌宝石
    void Step4() //引导开宝箱
    {
        //解绑之前
        UIFightContinue.GetComponent<Button>().onClick.RemoveListener(Step4);
        //延迟一下执行
        StartCoroutine(WaitToStep4());
        UIManager.Instance.IsLockedClick = true;
    }
    
    IEnumerator WaitToStep4()
    {
        yield return new WaitForSeconds(0.7f);
        UIManager.Instance.IsLockedClick = false;
        _curRole.IsLocked = true;
        bgImage.enabled = true;
        //Mark现在
        MarkGO(TreasureBoxFloor2,TreasureBoxArrow);
    }

    void Step4_1() //打开了宝箱之后，自言自语我可以装备宝石
    {
        bgImage.enabled = false;
        _curRole.IsLocked = false;
        //解绑之前
        DelMark(TreasureBoxFloor2, TreasureBoxArrow);
        CurDialogue.LoadDialogue("InlayGem");
        CurDialogue.OnDialogueEnd += Step4_2;
    }

    void Step4_2() //延迟执行
    {
        CurDialogue.OnDialogueEnd -= Step4_2;
        bgImage.enabled = true;
        MarkGO(UIBag, ArrowInlayGemToBag);//高亮显示背包
        UIBag.GetComponent<Button>().onClick.AddListener(Step4_3);
    }
    
    /*IEnumerator WaitToStep4_2()//引导打开背包
    {
        yield return new WaitForSeconds(1f);
        bgImage.enabled = true;
        MarkGO(UIBag, ArrowInlayGemToBag);//高亮显示背包
        UIBag.GetComponent<Button>().onClick.AddListener(Step4_3);
    }*/
    
    void Step4_3() //引导切换宝石Tab
    {
        //解绑之前
        UIBag.GetComponent<Button>().onClick.RemoveListener(Step4_3);
        DelMark(UIBag, ArrowInlayGemToBag);//取消高亮显示背包
        
        MarkGO(UIBagGemTab, ArrowGemTab);//高亮显示背包宝石Tab
        UIBagGemTab.GetComponent<Button>().onClick.AddListener(Step4_4);
    }

    void Step4_4() //引导镶嵌宝石
    {
        //解绑之前
        UIBagGemTab.GetComponent<Button>().onClick.RemoveListener(Step4_4);
        DelMark(UIBagGemTab, ArrowGemTab);//取消高亮背包宝石Tab

        MarkGO(UIGemSlot, HandPointInlayGem); //小手漂移，引导镶嵌宝石动画
    }

    void Step4_5()//镶嵌宝石完成时候调用
    {
        bgImage.enabled = false;
        DelMark(UIGemSlot, HandPointInlayGem);//取消小手漂移，引导镶嵌宝石动画
    }
    #endregion

    #region Step5 解锁横向房间
    public void Step5()//如果进入横向房间，则解锁Arraw
    {
        CrossRoomArrowsFloor2.ForEach(each => each.GetComponent<ArrowNode>().TutorialLocked = false);
    }
    #endregion
    
    #region 一些临时方法
    void ResetBullet(GameObject bullet)
    {
        BulletMapNode bm = bullet.GetComponent<BulletMapNode>();
        bm.SpineQuitHighLight();
    }

    void MarkGO(GameObject ins,GameObject fx = null)
    {
        ins.AddComponent<ShaderHoleController>().radius = 0.08f;
        fx?.SetActive(true);
        fx?.GetComponent<ParticleSystem>().Play();
    }

    void DelMark(GameObject ins,GameObject fx = null)
    {
        var sc = ins.GetComponent<ShaderHoleController>();
        if (sc) DestroyImmediate(sc);
        if (fx) DestroyImmediate(fx);
    }
    #endregion
}