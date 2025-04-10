using UnityEngine;
using System;

public class BulletSlotController: BaseSlotController<ItemDataBase>
{
    public BulletData CurData;
    public event Action OnBulletChanged;
    public BulletInnerSlotController LinkedInnerSlotController;
    public bool IsLocked; //槽位是不是锁定的
    BulletSlotView _bulletView;
    public override void BindView(SlotView view)
    {
        base.BindView(view);
        _bulletView = _view as BulletSlotView;
    }

    public override void Assign(ItemDataBase data, GameObject itemGO)
    {
        Unassign();
        CurData = data as BulletData;
        CurData.CurSlotController = this;
        CachedGO = itemGO;
        _view?.Display(itemGO);

        GM.Root.InventoryMgr._BulletInvData.EquipBullet(CurData);
        GM.Root.InventoryMgr._BulletInvData.RefreshModifiers();
    }

    public void Unassign()
    {
        if (CurData != null)
        {
            GM.Root.InventoryMgr._BulletInvData.UnEquipBullet(CurData);
            CurData.CurSlotController = null;
        }

        CurData = null;
        _view?.Clear();
    }

    public bool CanAccept(BulletData data) => 
        data != null && _bulletView.State == UILockedState.isNormal;
}