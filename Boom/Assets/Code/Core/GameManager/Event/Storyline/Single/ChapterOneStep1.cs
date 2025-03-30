using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterOneStep1: StorylineStepBase
{
    public ChapterOneStep1(StorylineController controller) : base(controller) {}

    public override void Enter()
    {
        EventManager.OnChapterOne += OnChapterOneCompleted;
        GlobalTicker.Instance.OnUpdate += Update;
    }
    
    void Update()
    {
        if (PlayerManager.Instance._QuestData.MainStoryProgress == 0)
        {
            GlobalTicker.Instance.OnUpdate -= Update;
            HelloWorld();
        }
    }

    void HelloWorld()
    {
        Dialogue curDia = EternalCavans.Instance.DialogueSC;
        curDia.LoadDialogue("开局对话邓肯");
        curDia.OnDialogueEnd += BeginQuest;
    }

    void BeginQuest()
    {
        EternalCavans.Instance.DialogueSC.OnDialogueEnd -= BeginQuest;
        QuestManager.Instance.SelectQuest(1);
    }
     
    void OnChapterOneCompleted()
    {
        Exit();
        controller.NextStep();
    }
    
    public override void Exit() => EventManager.OnChapterOne -= OnChapterOneCompleted;

    public override bool CheckComplete() => false;
}