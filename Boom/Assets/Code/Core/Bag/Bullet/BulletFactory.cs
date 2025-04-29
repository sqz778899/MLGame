using System;
using System.Collections.Generic;
using UnityEngine;

public static class BulletFactory
{
    public static ItemBase CreateBullet(BulletData _bulletData,BulletInsMode bulletInsMode)
    {
        GameObject bulletIns = ResManager.instance.
            CreatInstance(PathConfig.GetBulletTemplate(bulletInsMode));
        
        ItemBase bulletSC = bulletIns.GetComponent<ItemBase>();
        if (bulletSC is BulletNew bulletNew)
        {
            bulletNew.BindData(_bulletData);
            bulletIns.GetComponent<ItemInteractionHandler>().BindData(_bulletData);
        }

        /*if (bulletSC is BulletInner _bulletInner)
        {
            _bulletInner.BindData(_bulletData);
        }*/
        
        if (bulletSC is BulletInner _bulletInner)
        {
            _bulletInner.BindData(_bulletData);
        }
        
        if (bulletSC is BulletSpawnerNew bulletSpawnerNew)
            bulletSpawnerNew.BindData(_bulletData);
        
        if (bulletSC is BulletSpawnerInner bulletSpawnerInner)
            bulletSpawnerInner.BindData(_bulletData);
        
        return bulletSC;
    }   
} 