using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommonTooltip : MonoBehaviour
{
    public Image ImgThumbnail;
    public TextMeshProUGUI txtTitle;
    public TextMeshProUGUI txtDescription;

    public void SyncInfo(int ID,ItemTypes CurType)
    {
        switch (CurType)
        {
            case ItemTypes.Bullet:
                BulletTooltipInfo curInfo = BulletManager.Instance.GetBulletInfo(ID);
                ImgThumbnail.sprite = curInfo.bulletImage;
                txtTitle.text = curInfo.name;
                txtDescription.text = curInfo.description;
                break;
        }
    }
}
