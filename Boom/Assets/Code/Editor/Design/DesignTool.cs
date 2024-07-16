using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class DesignTool
{
    public int EntryID;
    [Button(ButtonSizes.Large)]
    [ButtonGroup("Entry")]
    void AddEntry()
    {
        List<BulletEntry> DesignEntries = TrunkManager.Instance.BulletEntryDesignJsons;
        foreach (var each in DesignEntries)
        {
            if (each.ID == EntryID && !MainRoleManager.Instance.CurBulletEntries.Contains(each))
                MainRoleManager.Instance.CurBulletEntries.Add(each);
        } 
        
        UIManager.Instance.G_Help.GetComponent<HelpMono>().InitBulletEntryDes();
    }
    
    [Button(ButtonSizes.Large)]
    [ButtonGroup("Entry")]
    void ClearEntry()
    {
        MainRoleManager.Instance.CurBulletEntries.Clear();
    }
    
    [Button(ButtonSizes.Large)]
    //[ButtonGroup("Entry")]
    void ClearStandby()
    {
        foreach (var each in MainRoleManager.Instance.CurStandbyBulletMats)
        {
            each.ID = 0;
            each.InstanceID = 0;
        }

        MainRoleManager.Instance.InitStandbyBulletMats();
    }
    
    [Button(ButtonSizes.Large)]
    //[ButtonGroup("Entry")]
    void ResetAll()
    {
        TrunkManager.Instance.SetSaveFileTemplate();
    }
    
    [Button(ButtonSizes.Large)]
    [ButtonGroup("Switch")]
    void SwichEdit()
    {
        MainSceneMono curSC = GameObject.Find("MainScene").GetComponent<MainSceneMono>();
        curSC.SwitchEditScene();
    }
    
    [Button(ButtonSizes.Large)]
    [ButtonGroup("Switch")]
    void SwichMap()
    {
        MainSceneMono curSC = GameObject.Find("MainScene").GetComponent<MainSceneMono>();
        curSC.SwitchMapScene();
    }
    
    [Button(ButtonSizes.Large)]
    [ButtonGroup("Switch")]
    void SwichFight()
    {
        MainSceneMono curSC = GameObject.Find("MainScene").GetComponent<MainSceneMono>();
        curSC.SwitchFightScene();
    }
}