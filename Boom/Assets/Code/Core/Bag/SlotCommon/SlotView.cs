using System;
using UnityEngine;


public class SlotView:MonoBehaviour
{
    public int ViewSlotID;
    public SlotType ViewSlotType;
    public int InstanceID;
    public bool IsLocked;
    public ISlotController Controller;

    public virtual void Init() {}

    public virtual void InitStep2() {}

    public virtual void Display(GameObject itemGO)
    {
        itemGO.transform.SetParent(transform);
        itemGO.transform.localPosition = Vector3.zero;
        itemGO.transform.localScale = Vector3.one;
    }

    public virtual void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
            Destroy(transform.GetChild(i).gameObject);
    }
}

public enum SlotType
{
    SpawnnerSlot = 0,
    ItemBagSlot = 1,
    BulletSlot = 2,
    ItemEquipSlot = 3,
    GemBagSlot = 4,
    GemInlaySlot = 5,
    CurBulletSlot = 6,
    BulletInnerSlot = 7,
    SpawnnerSlotInner = 8,
    GemBagSlotInner = 9,
    ShopSlot = 10,
}
