using System;
using System.Collections.Generic;
using UnityEngine;

public static class BulletFactory
{
    public static ItemBase CreateBullet(BulletData _bulletData,
        BulletInsMode bulletInsMode)
    {
        GameObject bulletIns = ResManager.instance.
            CreatInstance(PathConfig.GetBulletTemplate(bulletInsMode));
        
        ItemBase bulletSC = bulletIns.GetComponent<ItemBase>();
        if (bulletSC is Bullet bulletNew)
        {
            bulletNew.BindData(_bulletData);
            bulletNew.BulletInsMode = bulletInsMode;
        }
        if (bulletSC is BulletInner _bulletInner)
            _bulletInner.BindData(_bulletData);
        if (bulletSC is DraggableBulletSpawner bulletSpawner)
            bulletSpawner.BindData(_bulletData);
        
        return bulletSC;
    }   
} 