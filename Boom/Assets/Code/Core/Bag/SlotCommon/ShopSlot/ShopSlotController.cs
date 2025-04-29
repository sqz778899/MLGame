using UnityEngine;

public class ShopSlotController : BaseSlotController<ItemDataBase>
{
    public override bool CanAccept(ItemDataBase data) => false; //禁止从外部拖入

    public override void Assign(ItemDataBase data, GameObject itemGO)
    {
        AssignDirectly(data, itemGO);
    }

    public override void AssignDirectly(ItemDataBase data, GameObject itemGO,bool isRefreshData =true)
    {
        _curData = data;
        _curData.CurSlotController = this;
        CachedGO = itemGO;

        _view?.Display(itemGO);
    }

    public override void Unassign()
    {
        _curData.CurSlotController = null;
        _curData = null;
        _view?.Clear();
    }
}