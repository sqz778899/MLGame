using UnityEngine;

public class SlotController
{
    public int SlotID;
    public SlotType SlotType;
    public ItemDataBase CurBaseData;

    SlotView _view;

    public void BindView(SlotView view) =>_view = view;
    
    public bool IsFilled => CurBaseData != null;
    public bool IsEmpty => CurBaseData == null;
    
    public Vector3 TooltipOffset
    {
        get
        {
            return SlotType switch
            {
                SlotType.GemBagSlot => new Vector3(1.01f, -0.5f, 0),
                SlotType.BagItemSlot => new Vector3(1.01f, -0.5f, 0),
                SlotType.GemInlaySlot => new Vector3(-0.92f, -0.52f, 0),
                SlotType.BagEquipSlot => new Vector3(-0.92f, -0.52f, 0),
                _ => Vector3.zero
            };
        }
    }

    public bool CanAccept(ItemDataBase data)
    {
        if (data == null) return false;

        switch (SlotType)
        {
            case SlotType.GemBagSlot:
            case SlotType.GemInlaySlot:
                return data is GemData;
            case SlotType.BagItemSlot:
            case SlotType.BagEquipSlot:
                return data is ItemData;
        }
        return false;
    }

    public void Assign(ItemDataBase data, GameObject itemGO)
    {
        Unassign();
        CurBaseData = data;
        CurBaseData.CurSlotController = this;
        if (itemGO != null)
            _view.Display(itemGO);
    }

    public void Unassign()
    {
        if (CurBaseData != null)
            CurBaseData.CurSlotController = null;

        CurBaseData = null;
        _view?.Clear();
    }
}

public enum SlotType
{
    SpawnnerSlot = 0,
    BagItemSlot = 1,
    BulletSlot = 2,
    BagEquipSlot = 3,
    GemBagSlot = 4,
    GemInlaySlot = 5,
    CurBulletSlot = 6,
    BulletInnerSlot = 7
}
