using UnityEngine;

public class BulletSlotSpawnerController: BaseSlotController<ItemDataBase>
{
    BulletSpawnerSlotView _bulletView;
    public override void BindView(SlotView view)
    {
        base.BindView(view);
        _bulletView = _view as BulletSpawnerSlotView;
    }
    
    public override void Assign(ItemDataBase data, GameObject itemGO)
    {
        Unassign();
        _curData = data;
        _curData.CurSlotController = this;
        CachedGO = itemGO;
        _bulletView?.Display(itemGO);
    }
    
    public void Unassign()
    {
        if (_curData != null)
            _curData.CurSlotController = null;
        _curData = null;
        _bulletView?.Clear();
    }
}