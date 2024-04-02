using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletBase : MonoBehaviour
{
    public Vector3 forward = new Vector3(1, 0, 0);
    public BulletData _bulletData;
    public BulletInsMode bulletInsMode;

    public void InitBulletData()
    {
        if (_bulletData == null)
        {
            _bulletData = new BulletData();
            _bulletData.ID = 1;
        }
        _bulletData.SetDataByID(bulletInsMode);
        
        //找到目标挂载子弹贴图的地方。
        Debug.Log(gameObject.name);
        if (bulletInsMode == BulletInsMode.Inner)
        {
            SpriteRenderer targetSprite = null;
            SpriteRenderer[] allRenderers = GetComponentsInChildren<SpriteRenderer>();
            foreach (var each in allRenderers)
            {
                if (each.gameObject.name == "imgBullet")
                {
                    targetSprite = each;
                    break;
                }
            }

            if (targetSprite == null)
                return;
            
            targetSprite.sprite = _bulletData.imgBullet;
        }
        else
        {
            Image[] allImage = GetComponentsInChildren<Image>();
            Image target = null;
            foreach (var each in allImage)
            {
                if (each.gameObject.name == "imgBullet")
                {
                    target = each;
                    break;
                }
            }
            if (target == null)
                return;
            //
            target.sprite = _bulletData.imgBullet;
        }
    }
}