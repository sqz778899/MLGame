using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterOne: StorylineStepBase
{
    public ChapterOne(StorylineController controller) : base(controller) {}

    public override void Enter()=> EventManager.OnChapterOne += ooo;

    void ooo()
    {
        EternalCavans.Instance.DialogueSC.LoadDialogue("开局对话邓肯");
    }
    public override void Exit() => EventManager.OnChapterOne -= ooo;

    public override bool CheckComplete() => false;
}