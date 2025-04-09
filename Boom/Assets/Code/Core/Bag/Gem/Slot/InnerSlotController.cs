using UnityEngine;

public class InnerSlotController
{
    SlotView _view;
    public void BindView(SlotView view) => _view = view;
    

    /// <summary>
    /// 仅用于 UI 展示的简化 Assign
    /// </summary>
    public void Assign(ItemDataBase data, GameObject itemGO)
    {
        Unassign();
        if (itemGO != null)
            _view.Display(itemGO);
    }

    public void Unassign()
    {
        _view?.Clear();
    }
}