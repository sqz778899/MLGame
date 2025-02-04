using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSlot : SlotBase
{
    public int BulletSlotIndex;
    public Gem CurGem;
    public Vector3 ChildScale = Vector3.one;
    public override void SOnDrop()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform childTrans = transform.GetChild(i);
            childTrans.localScale = ChildScale;
        }
    }
}
