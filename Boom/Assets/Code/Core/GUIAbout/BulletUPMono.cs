using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletUPMono :GUIBase
{
    public int ID;

    public Image ImgBefore;
    public Image ImgAfter;

    public void InitData(int _id)
    {
        ID = _id;
        ImgBefore.sprite = ResManager.instance.GetAssetCache<Sprite>(
            PathConfig.GetBulletImagePath(ID, BulletInsMode.Spawner));
        
        ImgAfter.sprite = ResManager.instance.GetAssetCache<Sprite>(
            PathConfig.GetBulletImagePath(ID + 100, BulletInsMode.Spawner));
    }

}
