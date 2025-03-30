using System;
using System.Collections.Generic;
using UnityEngine;

public class StorylineManager:MonoBehaviour
{
    List<StorylineController> activeQuests = new();

    void Start()
    {
        if (GM.Root.IsSkipStorylineMode)
            return;
        StorylineController mainQuest = new StorylineController();
        mainQuest.AddStep(new ChapterOneStep1(mainQuest));
        mainQuest.AddStep(new ChapterOneStep2(mainQuest));
        mainQuest.AddStep(new ChapterOneStep3(mainQuest));
        mainQuest.AddStep(new ChapterOneStep4(mainQuest));
        
        mainQuest.StartQuest();

        activeQuests.Add(mainQuest);
    }

    void OnDestroy() => activeQuests.ForEach(s => s.Dispose());
}