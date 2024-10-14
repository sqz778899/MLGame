using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommonTooltip : MonoBehaviour
{
    public Transform ThumbnailNode;
    public Image ImgThumbnail;
    public TextMeshProUGUI txtTitle;
    public TextMeshProUGUI txtDescription;

    public void SyncInfo(int ID,TipTypes CurType)
    {
        switch (CurType)
        {
            case TipTypes.Bullet:
                BulletTooltipInfo curInfo = BulletManager.Instance.GetBulletInfo(ID);
                ImgThumbnail.sprite = curInfo.bulletImage;
                txtTitle.text = curInfo.name;
                txtDescription.text = curInfo.description;
                break;
            case TipTypes.Item:
                break;
        }
    }
}
    