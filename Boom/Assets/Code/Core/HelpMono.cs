using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HelpMono : GUIBase
{
    public GameObject BulletEntryRoot;
    public TextMeshProUGUI CurEntryDes;

    Vector2 _startPos = new Vector2(0,475);
    float offsetY = 180f;
    
    void Start()
    {
        InitBulletEntryDes();
    }
    
    public void InitBulletEntryDes()
    {
        //....................Clear BulletEntry.....................
        for (int i = BulletEntryRoot.transform.childCount - 1; i >= 0; i--)
            DestroyImmediate(BulletEntryRoot.transform.GetChild(i).gameObject);
        
        //.................Init BulletEntry Des.....................
        List<BulletEntry> curBullets = MainRoleManager.Instance.CurBulletEntries;
        for (int i = 0; i < curBullets.Count; i++)
        {
            GameObject curPB = ResManager.instance.CreatInstance(PathConfig.BulletEntryPB);
            EntryDes curScript = curPB.GetComponent<EntryDes>();
            curScript.GlobalDes = CurEntryDes;
            curScript.InitData(curBullets[i].ID);
            curPB.transform.SetParent(BulletEntryRoot.transform,false);
            curPB.GetComponent<RectTransform>().anchoredPosition = _startPos - new Vector2(0,offsetY * i);
        }
    }
}
