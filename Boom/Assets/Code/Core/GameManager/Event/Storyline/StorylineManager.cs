using System;
using System.Collections.Generic;
using UnityEngine;

public class StorylineManager:MonoBehaviour
{
    List<StorylineController> activeQuests = new();

    void Start()
    {
        StorylineController mainQuest = new StorylineController();
        //mainQuest.AddStep(new ChapterOne(mainQuest));
        /*mainQuest.AddStep(new StepDialogue(mainQuest, "MainQuest01"));
        mainQuest.AddStep(new StepCombat(mainQuest, enemyID: 201));
        mainQuest.AddStep(new StepCollectItem(mainQuest, itemID: 301));*/

        mainQuest.StartQuest();

        activeQuests.Add(mainQuest);
    }

    void OnDestroy() => activeQuests.ForEach(s => s.Dispose());
}