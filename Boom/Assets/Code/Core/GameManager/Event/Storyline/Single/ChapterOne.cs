using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterOne: StorylineStepBase
{
    public ChapterOne(StorylineController controller) : base(controller) {}

    public override void Enter()=> EventManager.OnChapterOne += HelloWorld;

    void HelloWorld()
    {
        Dialogue curDia = EternalCavans.Instance.DialogueSC;
        curDia.LoadDialogue("开局对话邓肯");
        curDia.OnDialogueEnd += BeginQuest;
    }

    void BeginQuest()
    {
        QuestManager.Instance.SelectQuest(1);
    }
    
    public override void Exit() => EventManager.OnChapterOne -= HelloWorld;

    public override bool CheckComplete() => false;
}