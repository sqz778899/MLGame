using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommonTooltip : MonoBehaviour
{
    public Image ImgThumbnail;
    public TextMeshProUGUI txtTitle;
    public TextMeshProUGUI txtDescription;

    public int ID;
    public ItemTypes CurType;

    void ppp()
    {
        switch (CurType)
        {
            case ItemTypes.Bullet:
                BulletTooltipInfo curInfo = BulletManager.Instance.GetBulletInfo(ID);
                ImgThumbnail = curInfo.bulletImage;
                txtTitle.text = curInfo.name;
                txtDescription.text = curInfo.description;
                break;
        }
    }
}
