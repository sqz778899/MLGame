using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DialogueManager
{
    static GameObject DialogueRoot;

    static void InitDialogueRoot()
    {
        if (DialogueRoot != null)
            return;
        DialogueRoot = GameObject.Find("DialogueRoot");
    }
    
    //对话框的类型
    public static GameObject CreatDialogueFight()
    {
        InitDialogueRoot();
        GameObject DialogueFightIns = ResManager.instance.CreatInstance(PathConfig.DialogueFightPB);
        DialogueFightIns.transform.SetParent(DialogueRoot.transform,false);
        return DialogueFightIns;
    }
}