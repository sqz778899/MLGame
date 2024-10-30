using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIWin : MonoBehaviour
{
    public GameObject AwardRoot;
    
    public void Win(Award CurAward)
    {
        for (int i = 0; i < CurAward.Items.Count; i++)
        {
            int curItemID = CurAward.Items[i];
            GameObject AwardTextIns = ResManager.instance.CreatInstance(PathConfig.AwardTextPB);
            AwardText CurText = AwardTextIns.GetComponent<AwardText>();
            CurText.SyncAwardText(curItemID);
        }   
    }
}
