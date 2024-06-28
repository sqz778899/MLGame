using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EntryDes : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public TextMeshProUGUI GlobalDes;
    private BulletEntry _bulletEntry;
    
    public void InitData(int ID)
    {
        List<BulletEntry> curDesign = TrunkManager.Instance.BulletEntryDesignJsons;
        foreach (var each in curDesign)
        {
            if (each.ID == ID)
            {
                _bulletEntry = each;
                Title.text = each.Name;
            }
        }
    }
    
    public void OnClickEntry()
    {
        GlobalDes.text = _bulletEntry.Description;
    }
}
