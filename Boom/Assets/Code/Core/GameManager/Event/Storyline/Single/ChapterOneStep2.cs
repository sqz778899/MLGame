using System.Linq;
using UnityEngine;

public class ChapterOneStep2:StorylineStepBase
{
    TutorialGUI _tutorialGUI;
    GameObject _portal;
    Portal _portalSC;
    QuestBar _curQuestBar;
    public ChapterOneStep2(StorylineController controller) : base(controller) {}
    
    public override void Enter()
    {
        EventManager.OnChapterOne += OnChapterOneCompleted;
        GlobalTicker.Instance.OnUpdate += Update;
    }
    
    void Update()
    {
        if (PlayerManager.Instance._QuestData.MainStoryProgress == 1)
        {
            GlobalTicker.Instance.OnUpdate -= Update;
            PiercingLearn();
        }
    }

    void PiercingLearn()
    {
        if (_tutorialGUI == null)
            _tutorialGUI = GameObject.Find("TutorialGUI").GetComponent<TutorialGUI>();
        if (_portal == null)
            _portal = GameObject.Find("Portal");
        Dialogue curDia = EternalCavans.Instance.DialogueSC;
        curDia.LoadDialogue("穿透对话邓肯");
        curDia.OnDialogueEnd += LeadPortal;
    }

    void LeadPortal()
    {
        EternalCavans.Instance.DialogueSC.OnDialogueEnd -= LeadPortal;
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
        OnChapterOneCompleted();
    }
     
    void OnChapterOneCompleted()
    {
        Exit();
        controller.NextStep();
    }
    
    public override void Exit() => EventManager.OnChapterOne -= OnChapterOneCompleted;

    public override bool CheckComplete() => false;
}