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
        if (bulletSC is Bullet bulletNew)
        {
            bulletNew.BindData(_bulletData);
            bulletIns.GetComponent<ItemInteractionHandler>().BindData(_bulletData);
        }
        
        if (bulletSC is BulletInner _bulletInner)
        {
            _bulletInner.BindData(_bulletData);
        }
        
        if (bulletSC is BulletSpawner bulletSpawnerNew)
            bulletSpawnerNew.BindData(_bulletData);
        
        if (bulletSC is BulletSpawnerInner bulletSpawnerInner)
            bulletSpawnerInner.BindData(_bulletData);
        
        return bulletSC;
    }
    
    
    public static BulletData CreateVirtualReactionBullet(ElementReactionType reaction, int slotID)
    {
        BulletData fake = new BulletData(-999, null); // id -999 表示虚拟
        fake.Name = $"<Reaction: {reaction}>";        // 自定义名称
        fake.ElementalType = ElementalTypes.Non;
        fake.FinalDamage = 0; // 会被后续 DamageResult 替换
        fake.FinalPiercing = 0;
        fake.FinalResonance = 0;
        fake.CurSlotController = new FakeSlotController(slotID); // 自定义 SlotID
        return fake;
    }

    class FakeSlotController : ISlotController
    {
        public SlotType SlotType { get; }
        public int SlotID { get; }
        public ItemDataBase CurData { get; }
        public void Unassign(){}
        public bool CanAccept(ItemDataBase data) => false;
        public void Assign(ItemDataBase data, GameObject itemGO) {}
        public void AssignDirectly(ItemDataBase data, GameObject itemGO, bool isRefreshData = true) {}
        public Vector3 TooltipOffset { get; }
        public GameObject GetGameObject() => null;
        public FakeSlotController(int slotID) => SlotID = slotID;
    }

} 