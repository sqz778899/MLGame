using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PRDisplayBar : MonoBehaviour
{
    public Image MatImage;
    public Image NarmalBar;
    public TextMeshProUGUI CurText;
    public int ID;
    RollPR CurRollPr;
    //-1118 -200 //-362 -200
    //-1118 -308
    void Start()
    {
        InitDataByID();
    }

    public void InitDataByID()
    {
        /*foreach (RollPR each in MainRoleManager.Instance.CurRollPR)
        {
            if (each.ID == ID)
                CurRollPr = each;
        }*/
        
        string curImagePath = "";
        if (ID != 0)
            curImagePath = PathConfig.GetBulletImageOrSpinePath(ID, BulletInsMode.Mat);
        else
            curImagePath = PathConfig.ScoreMatImage;
        
        Sprite curImage = ResManager.instance.GetAssetCache<Sprite>(curImagePath);
        MatImage.sprite = curImage;
    }
   
    void Update()
    {
        NarmalBar.fillAmount = CurRollPr.Probability;
        CurText.text = $"{CurRollPr.Probability * 100}%";
    }
}
