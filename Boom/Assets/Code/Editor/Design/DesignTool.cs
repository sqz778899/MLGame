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
}