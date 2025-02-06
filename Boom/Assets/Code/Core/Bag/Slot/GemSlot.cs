using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSlot : SlotBase
{
    public int BulletSlotIndex;
    public Gem CurGem;
    public Vector3 ChildScale = Vector3.one;
    public override void SOnDrop(GameObject _childIns,SlotType _slotType)
    {
        base.SOnDrop(_childIns,_slotType);
        ChildIns.transform.localScale = ChildScale;
    }
}
