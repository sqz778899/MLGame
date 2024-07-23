using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RollBulletMat : RollBase
{
    public Image CurImg;
    public int ID;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        CurImg.color = Color.yellow;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        CurImg.color = OrignalColor;
    }

    public void InitImg()
    {
        CurImg.sprite = ResManager.instance.GetAssetCache<Sprite>(
            PathConfig.GetBulletImageOrSpinePath(ID, BulletInsMode.Mat));
    }
}
    
