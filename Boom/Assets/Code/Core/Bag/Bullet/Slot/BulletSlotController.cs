using UnityEngine;

public class BulletSlotController
{
    public BulletData CurData;
    public GameObject CachedGO;
    BulletSlotView _view;

    public void BindView(BulletSlotView view) => _view = view;

    public void Assign(BulletData data, GameObject bulletGO)
    {
        Unassign();
        CurData = data;
        //CurData.CurSlotController = this;
        CachedGO = bulletGO;
        _view?.Display(bulletGO);

        // 装备逻辑
        GM.Root.InventoryMgr._BulletInvData.EquipBullet(data);
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

    public bool CanAccept(BulletData data)
    {
        return data != null && _view.IsUnlocked;
    }
}