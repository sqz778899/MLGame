using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUIWin : MonoBehaviour
{
    [Header("重要资产")]
    public TextMeshProUGUI textScore;
    public GameObject AwardRoot;
    
    public void Win(Award CurAward)
    {
        MainRoleManager.Instance.Score += CurAward.Score;
        textScore.text = $"+ {CurAward.Score}";
        for (int i = 0; i < CurAward.Items.Count; i++)
        {
            int curItemID = CurAward.Items[i];
            GameObject AwardTextIns = ResManager.instance.CreatInstance(PathConfig.AwardTextPB);
            AwardText CurText = AwardTextIns.GetComponent<AwardText>();
            CurText.SyncAwardText(curItemID);
        }   
    }
}
