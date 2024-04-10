using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletBase : MonoBehaviour
{
    public Vector3 forward = new Vector3(1, 0, 0);
    public BulletData _bulletData;
    public BulletInsMode bulletInsMode;
    public GameObject GroupStar; 
    
    public virtual void Update()
    {
        if (GroupStar != null)
        {
            SetStart(_bulletData.Level);
        }
    }

    void SetStart(int Level)
    {
        switch (Level)
        {
            case 1:
                GroupStar.transform.GetChild(0).gameObject.SetActive(true);
                GroupStar.transform.GetChild(1).gameObject.SetActive(false);
                GroupStar.transform.GetChild(2).gameObject.SetActive(false);
                break;
            case 2:
                GroupStar.transform.GetChild(0).gameObject.SetActive(true);
                GroupStar.transform.GetChild(1).gameObject.SetActive(true);
                GroupStar.transform.GetChild(2).gameObject.SetActive(false);
                break;
            case 3:
                GroupStar.transform.GetChild(0).gameObject.SetActive(true);
                GroupStar.transform.GetChild(1).gameObject.SetActive(true);
                GroupStar.transform.GetChild(2).gameObject.SetActive(true);
                break;
        }
    }

    public void InitBulletData()
    {
        if (_bulletData == null)
            _bulletData = new BulletData(1);
        
        _bulletData.SetDataByID(bulletInsMode);
        
        //找到目标挂载子弹贴图的地方。
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