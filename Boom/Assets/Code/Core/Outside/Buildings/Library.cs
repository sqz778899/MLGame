using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Library : BuildBase
{
    public override void OnOffBuild()
    {
        if (GM.Root.PlayerMgr._QuestData.MainStoryProgress >= 2)
        {
            // 1)如果主线剧情进度>=1，解锁图书馆
            IsStorylineLocked = false;
        }
        base.OnOffBuild();
    }
}
