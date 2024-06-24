using TMPro;
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
        foreach (RollPR each in MainRoleManager.Instance.CurRollPR)
        {
            if (each.ID == ID)
                CurRollPr = each;
        }

        Sprite curImage = ResManager.instance.GetAssetCache<Sprite>(
            PathConfig.GetBulletMatImagePath(ID));
        MatImage.sprite = curImage;
    }
   
    void Update()
    {
        NarmalBar.fillAmount = CurRollPr.Probability;
        CurText.text = $"{CurRollPr.Probability * 100}%";
    }
}
