using UnityEngine;

public class BulletInnerSlotController: BaseSlotController<ItemDataBase>
{
    BulletInnerSlotView _view;

    public void BindView(BulletInnerSlotView view) => _view = view;

    public override void Assign(ItemDataBase data, GameObject itemGO)
    {
        //如果子弹槽是锁定状态，则无法拖拽
        if (_view.IsLocked) return;
        
        BulletInnerSlotController from = data.CurSlotController as BulletInnerSlotController;
        from?.Unassign();
        AssignDirectly(data, itemGO);
    }

    public override void AssignDirectly(ItemDataBase data, GameObject itemGO)
    {
        //2)添加新子弹，到数据层
        _curData = data;
        _curData.CurSlotController = this;
       
        GM.Root.InventoryMgr._BulletInvData.EquipBullet(data as BulletData);//添加新子弹
        GM.Root.InventoryMgr.AddBulletToFight(data as BulletData);//战场创建傻逼兮兮的小子弹
        
        // step 3: UI更新
        _view?.Display(null);
        
        GameObject.Destroy(itemGO);
    }
    
    public override void Unassign()
    {
        if (_curData != null)
        {
            GM.Root.InventoryMgr._BulletInvData.UnEquipBullet(_curData as BulletData);
            CurData.CurSlotController = null;
        }
        _curData = null;
        _view?.Clear();
    }
    
    public override bool CanAccept(ItemDataBase data)
    {
        if (data == null) return false;
        if (!(data is BulletData)) return false;
        return !_view.IsLocked;
    }
}