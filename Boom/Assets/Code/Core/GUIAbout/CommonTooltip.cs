using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommonTooltip : MonoBehaviour
{
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
                Item curItem = new Item(ID);
                Sprite curItemSprite = ResManager.instance.GetAssetCache<Sprite>(curItem.resAllPath);
                ImgThumbnail.sprite = curItemSprite;
                txtTitle.text = curItem.name;
                txtDescription.text = "w: " + curItem.waterElement + "\n" +
                                     "f: " + curItem.fireElement + "\n" +
                                     "t: " + curItem.thunderElement + "\n" +
                                     "l: " + curItem.lightElement + "\n" +
                                     "d: " + curItem.darkElement + "\n" +
                                     "max: " + curItem.maxDamage;
                break;
        }
    }
}
