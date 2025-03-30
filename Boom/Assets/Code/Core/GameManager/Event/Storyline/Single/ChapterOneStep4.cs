using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ChapterOneStep4:StorylineStepBase
{
    public ChapterOneStep4(StorylineController controller) : base(controller) {}
    
    public override void Enter()
    {
        EventManager.OnChapterOne += OnChapterOneCompleted;
        GlobalTicker.Instance.OnUpdate += Update;
    }
    
    void Update()
    {
        if (PlayerManager.Instance._QuestData.MainStoryProgress == 3)
        {
            GlobalTicker.Instance.OnUpdate -= Update;
            Complete();
        }
    }

    void Complete()
    {
        Dialogue curDia = EternalCavans.Instance.DialogueSC;
        curDia.LoadDialogue("教学全部完成邓肯");
        curDia.OnDialogueEnd += End;
    }
    
    void End()
    {
        EternalCavans.Instance.DialogueSC.OnDialogueEnd -= End;
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