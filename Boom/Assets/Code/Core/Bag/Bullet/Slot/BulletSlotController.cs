using UnityEngine;
using System;

public class BulletSlotController: BaseSlotController<ItemDataBase>
{
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
        // step 1:卸载原槽位
        BulletSlotController from = data.CurSlotController as BulletSlotController;
        itemGO.transform.SetParent(DragManager.Instance.dragRoot.transform);
        from?.Unassign();
        AssignDirectly(data, itemGO);
    }

    public override void AssignDirectly(ItemDataBase data, GameObject itemGO)
    {
        // step 2: 赋值自己
        _curData = data;
        _curData.CurSlotController = this;
        CachedGO = itemGO;
        // step 3: UI更新
        _view?.Display(itemGO);
        // step 4: 刷新数据层
        GM.Root.InventoryMgr._BulletInvData.EquipBullet(_curData as BulletData);
        GM.Root.InventoryMgr._BulletInvData.RefreshModifiers();
    }

    public void Unassign()
    {
        if (_curData != null)
        {
            GM.Root.InventoryMgr._BulletInvData.UnEquipBullet(_curData as BulletData);
            CurData.CurSlotController = null;
        }
        _curData = null;
        _view?.Clear();
    }

    public bool CanAccept(BulletData data) => 
        data != null && _bulletView.State == UILockedState.isNormal;
}