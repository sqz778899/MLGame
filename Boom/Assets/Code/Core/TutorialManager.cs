using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Header("基础资产")]
    public Image bgImage;
    [Header("子弹资产")]
    public GameObject BulletFloor1;
    [Header("箭头资产")]
    public GameObject ArrowFloor1;
    
    [Header("UI资产")]
    public GameObject UIBag;
    public GameObject UIBagBulletTab;
    public GameObject UIBulletSlot1;
    public GameObject UIStart;
    public GameObject UIKeyBoard;
    
    [Header("特效资产")]
    public GameObject PickBullet01Arrow;
    public GameObject BagFXArrow;
    public GameObject BagBulletTabFXArrow;
    public GameObject SlotFXArrow;
    public GameObject StartFXArrow;
    public GameObject OBHPFXArrow;
    
    [Header("对话系统脚本")]
    public Dialogue CurDialogue;
    
    //临时监听的变量
    bool _isSetBullet = false;
    bool _isStartStep2_4 = false;
    
    void Start()
    {
        Step1();
    }

    void Update()
    {
        if(_isStartStep2_4) return;
        if (MainRoleManager.Instance.CurBullets.Count > 0)
        {
            _isSetBullet = true;
            Step2_4();
        }
    }

    #region Step1 引导，捡起黏土子弹
    void Step1()
    {
        bgImage.enabled = false;
        ResetBullet(BulletFloor1);
        CurDialogue.OnDialogueEnd += Step1_1;
        CurDialogue.LoadDialogue("Beginner01");//新手教程对话
    }
    void Step1_1() //引导，捡起黏土子弹
    {
        ArrowFloor1.GetComponent<ArrowNode>().IsLocked = true;
        PickBullet01Arrow.SetActive(true);
        PickBullet01Arrow.GetComponent<ParticleSystem>().Play();
        bgImage.enabled = true;
    }
    
    //点击黏土子弹触发
    public void Step1_2()
    {
        UIManager.Instance.IsLockedClick = true;
        bgImage.enabled = false;
        ResetBullet(BulletFloor1);
        DestroyImmediate(PickBullet01Arrow);
        CurDialogue.LoadDialogue("Beginner02");
        CurDialogue.OnDialogueEnd += Step1_3;
    }
    
    void Step1_3()
    {
        UIManager.Instance.IsLockedClick = false;
        MainRoleManager.Instance.AddSpawner(1);
        BulletJson bulletDesignJson = TrunkManager.Instance.BulletDesignJsons
            .FirstOrDefault(b => b.ID == 1) ?? new BulletJson();
        FloatingGetItemText(bulletDesignJson.Name);
        Destroy(BulletFloor1);
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
    void Step2()
    {
        CurDialogue.OnDialogueEnd += Step2_1;
        CurDialogue.LoadDialogue("Beginner03");
    }

    void Step2_1()
    {
        UIManager.Instance.IsLockedClick = false;
        bgImage.enabled = true;
        ShaderHoleController bageSC = UIBag.AddComponent<ShaderHoleController>();
        bageSC.radius = 0.08f;
        BagFXArrow.SetActive(true);
        BagFXArrow.GetComponent<ParticleSystem>().Play();
        
        Button btnBag = UIBag.GetComponent<Button>();
        btnBag.onClick.AddListener(Step2_2);
    }
    
    void Step2_2()
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
        //UIBulletSlotRole.AddComponent<ShaderHoleController>().radius = 0.08f;
        SlotFXArrow.SetActive(true);
        SlotFXArrow.GetComponent<ParticleSystem>().Play();
    }

    void Step2_4()
    {
        _isStartStep2_4 = true;
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

    void Step2_5()
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
        SC.BtnFight.onClick.AddListener(Step3);
    }

    void Step3_2() //进入战斗中
    {
        UIManager.Instance.IsLockedClick = true;
        bgImage.enabled = true;
        UIKeyBoard.SetActive(true);
    }

    public void Step3_3()
    {
        UIManager.Instance.IsLockedClick = false;
        bgImage.enabled = false;
        DestroyImmediate(UIKeyBoard);
    }
    #endregion
    
    #region 一些临时方法
    void ResetBullet(GameObject bullet)
    {
        BulletMapNode bm = bullet.GetComponent<BulletMapNode>();
        bm.SpineQuitHighLight();
    }
    
    void FloatingGetItemText(string Content)
    {
        Transform textNode = MainRoleManager.Instance.MainRoleIns.GetComponent<RoleInMap>().TextNode;
        GameObject textIns = ResManager.instance.CreatInstance(PathConfig.TxtGetItemPB);
        FloatingDamageText textSc = textIns.GetComponent<FloatingDamageText>();
        textIns.transform.SetParent(textNode.transform,false);
        textSc.AnimateText($"{Content}",new Color(218f/255f,218f/255f,218f/255f,1f));
    }
    #endregion
}