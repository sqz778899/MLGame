using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ChapterOneStep3:StorylineStepBase
{
    TutorialGUI _tutorialGUI;
    GameObject _library;
    Library _librarySC;
   
    public ChapterOneStep3(StorylineController controller) : base(controller) {}
    
    public override void Enter()
    {
        EventManager.OnChapterOne += OnChapterOneCompleted;
        GlobalTicker.Instance.OnUpdate += Update;
    }
    
    void Update()
    {
        if (PlayerManager.Instance._QuestData.MainStoryProgress == 2)
        {
            GlobalTicker.Instance.OnUpdate -= Update;
            ResonanceLearn();
        }
    }

    void ResonanceLearn()
    {
        if (_tutorialGUI == null)
            _tutorialGUI = GameObject.Find("TutorialGUI").GetComponent<TutorialGUI>();
        if (_library == null)
            _library = GameObject.Find("Library");
        Dialogue curDia = EternalCavans.Instance.DialogueSC;
        curDia.LoadDialogue("共振对话邓肯");
        curDia.OnDialogueEnd += LeadLibrary;
    }

    void LeadLibrary()
    {
        EternalCavans.Instance.DialogueSC.OnDialogueEnd -= LeadLibrary;
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