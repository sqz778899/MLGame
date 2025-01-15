using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DislogueManager
{
    static GameObject DialogueRoot;

    static void InitDialogueRoot()
    {
        if (DialogueRoot != null)
            return;
        DialogueRoot = GameObject.Find("DialogueRoot");
    }
    
    //对话框的类型
    public static void CreatDialogueFight(int MapNodeID)
    {
        InitDialogueRoot();
        GameObject DialogueFightIns = ResManager.instance.CreatInstance(PathConfig.DialogueFightPB);
        DialogueFight CurFight = DialogueFightIns.GetComponent<DialogueFight>();
        CurFight.MapNodeID = MapNodeID;
        DialogueFightIns.transform.SetParent(DialogueRoot.transform,false);
    }
}
