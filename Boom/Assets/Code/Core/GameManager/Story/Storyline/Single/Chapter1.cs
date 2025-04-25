using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Chapter1Step1: IStorylineNodeBuilder
{
    Dialogue dia => EternalCavans.Instance.DialogueSC;
    public StorylineNode Build()
    {
        return new StorylineNode {
            ID = 1,
            Name = "开局邓肯对话",
            TriggerCondition = () => Condition(),
            OnStart = () => OnStart()
        };
    }

    public bool Condition() =>
        PlayerManager.Instance._QuestData.MainStoryProgress == 0 &&
        SceneManager.GetActiveScene().name == "1.MainEnv";
    
    public void OnStart()
    {
        dia.LoadDialogue("开局对话邓肯");
        dia.OnDialogueEnd += OnComplete;
    }
    
    public void OnComplete()
    {
        dia.OnDialogueEnd -= OnComplete;
        QuestManager.Instance.SelectQuest(1);
        PlayerManager.Instance._QuestData.MainStoryProgress += 1;
        StorylineSystem.Instance.MarkNodeComplete(1);
    }
}

public class Chapter1Step2: IStorylineNodeBuilder
{
    Dialogue dia => EternalCavans.Instance.DialogueSC;
    TutorialGUI _tutorialGUI;
    GameObject _portal;
    Portal _portalSC;
    QuestBar _curQuestBar;
    public StorylineNode Build()
    {
        return new StorylineNode {
            ID = 2,
            Name = "穿透对话邓肯",
            TriggerCondition = () => Condition(),
            OnStart = () => OnStart()
        };
    }

    public bool Condition() =>
        PlayerManager.Instance._QuestData.MainStoryProgress == 1 &&
        SceneManager.GetActiveScene().name == "1.MainEnv";
    
    public void OnStart()
    {
        dia.LoadDialogue("穿透对话邓肯");
        dia.OnDialogueEnd += LeadPortal;
        InitRes();
    }

    void InitRes()
    {
        if (_tutorialGUI == null)
            _tutorialGUI = GameObject.Find("TutorialGUI").GetComponent<TutorialGUI>();
        if (_portal == null)
        {
            _portal = GameObject.Find("Portal");
            _portalSC = _portal.GetComponent<Portal>();
        }
    }
    
    //引导去传送门
    void LeadPortal()
    {
        dia.OnDialogueEnd -= LeadPortal;
        //1)设置背景板状态
        _tutorialGUI.TutorialBG.enabled = true;
        //2)设置箭头状态
        TutoConfig.SetTutoHigh(_portal.gameObject,0.07f);
        RectTransform arrowRTrans = _tutorialGUI.FXArrow.GetComponent<RectTransform>();
        Vector3 newPos = _portal.transform.position + TutoConfig.arrowOffsetPortal;
        Vector2 UIPos = UTools.GetUISpacePos(newPos,arrowRTrans);
        arrowRTrans.anchoredPosition = UIPos;
        arrowRTrans.GetComponent<FloatingIcon>().ResetPos(arrowRTrans.transform.localPosition);
        _tutorialGUI.FXArrow.Play();
        //3)绑定事件
        _portalSC = _portal.GetComponent<Portal>();
        _portalSC.onClick.AddListener(LeadPortalMenu);
    }
    
    //引导菜单
    void LeadPortalMenu()
    {
        //1)解锁各种
        _portalSC.onClick.RemoveListener(LeadPortalMenu);
        TutoConfig.RemoveTutoHigh(_portal.gameObject);
        //2)找到穿透学实践的任务
        QuestBar[] questBars = _tutorialGUI.QuestRoot.GetComponentsInChildren<QuestBar>(true);
        _curQuestBar = questBars.FirstOrDefault(q => q.QuestID == 2);
        TutoConfig.SetTutoHigh(_curQuestBar.gameObject,0.15f);
        TutoConfig.SetArrow(_tutorialGUI.FXArrow,_curQuestBar.transform.position + TutoConfig.arrowOffset);
        //3)添加子菜单事件
        _curQuestBar.btnQuest.onClick.AddListener(LeadPortalMenu2);
    }
    
    //引导子菜单
    void LeadPortalMenu2()
    {
        //1)解锁各种
        _curQuestBar.btnQuest.onClick.RemoveListener(LeadPortalMenu2);
        TutoConfig.RemoveTutoHigh(_curQuestBar.gameObject);
        //2)找到挑战按钮
        TutoConfig.SetTutoHigh(_tutorialGUI.QuestGO.gameObject,0.07f);
        TutoConfig.SetArrow(_tutorialGUI.FXArrow,_tutorialGUI.QuestGO.gameObject.transform.position + TutoConfig.arrowOffset);
        //3)添加挑战按钮事件
        _tutorialGUI.QuestGO.onClick.AddListener(End);
    }
    
    void End()
    {
        _tutorialGUI.QuestGO.onClick.RemoveListener(End);
        _tutorialGUI.TutorialBG.enabled = false;
        _tutorialGUI.FXArrow.Clear();
        _tutorialGUI.FXArrow.Stop();
        OnComplete();
    }
    
    public void OnComplete()
    {
        PlayerManager.Instance._QuestData.MainStoryProgress += 1;
        StorylineSystem.Instance.MarkNodeComplete(2);
    }
}

public class Chapter1Step3: IStorylineNodeBuilder
{
    Dialogue dia => EternalCavans.Instance.DialogueSC;
    #region 资产与构建相关
    TutorialGUI _tutorialGUI;
    GameObject _library;
    Library _librarySC;
    public StorylineNode Build()
    {
        return new StorylineNode {
            ID = 3,
            Name = "共振对话邓肯",
            TriggerCondition = () => Condition(),
            OnStart = () => OnStart()
        };
    }
    
    void InitRes()
    {
        if (_tutorialGUI == null)
            _tutorialGUI = GameObject.Find("TutorialGUI").GetComponent<TutorialGUI>();
        if (_library == null)
        {
            _library = GameObject.Find("Library");
            _librarySC =_library.GetComponent<Library>();
        }
    }
    #endregion

    public bool Condition() =>
        PlayerManager.Instance._QuestData.MainStoryProgress == 2 &&
        SceneManager.GetActiveScene().name == "1.MainEnv";
    
    public void OnStart()
    {
        dia.LoadDialogue("共振对话邓肯");
        dia.OnDialogueEnd += LeadLibrary;
        InitRes();
    }
    
    void LeadLibrary()
    {
        dia.OnDialogueEnd -= LeadLibrary;
        //1)设置背景板状态
        _tutorialGUI.TutorialBG.enabled = true;
        //2)设置箭头状态
        TutoConfig.SetTutoHigh(_library.gameObject,0.25f);
        RectTransform arrowRTrans = _tutorialGUI.FXArrow.GetComponent<RectTransform>();
        Vector3 newPos = _library.transform.position + TutoConfig.arrowOffsetPortal;
        Vector2 UIPos = UTools.GetUISpacePos(newPos,arrowRTrans);
        arrowRTrans.anchoredPosition = UIPos;
        arrowRTrans.GetComponent<FloatingIcon>().ResetPos(arrowRTrans.transform.localPosition);
        _tutorialGUI.FXArrow.Play();
        //3)绑定事件
        _librarySC = _library.GetComponent<Library>();
        _librarySC.onClick.AddListener(LeadLibraryMenu);
    }

    void LeadLibraryMenu()
    {
        //1)解锁各种
        _librarySC.onClick.RemoveListener(LeadLibraryMenu);
        TutoConfig.RemoveTutoHigh(_library.gameObject);
        //2)找到图书馆天赋
        TutoConfig.SetTutoHigh(_tutorialGUI._TalentNode.gameObject,0.07f);
        TutoConfig.SetArrow(_tutorialGUI.FXArrow,_tutorialGUI._TalentNode.gameObject.transform.position + TutoConfig.arrowOffset);
        _tutorialGUI.FXArrow.Play();
        //3)绑定事件
        Button btnTalent = _tutorialGUI._TalentNode.GetComponentInChildren<Button>(true);
        btnTalent.onClick.AddListener(End);
    }
    
    void End()
    {
        Button btnTalent = _tutorialGUI._TalentNode.GetComponentInChildren<Button>(true);
        btnTalent.onClick.RemoveListener(End);
        _tutorialGUI.TutorialBG.enabled = false;
        _tutorialGUI.FXArrow.Clear();
        _tutorialGUI.FXArrow.Stop();
        OnComplete();
    }
    
    public void OnComplete()
    {
        PlayerManager.Instance._QuestData.MainStoryProgress += 1;
        StorylineSystem.Instance.MarkNodeComplete(3);
    }
}