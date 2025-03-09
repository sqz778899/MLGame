using UnityEngine;

public class GemSlotInner: SlotBase
{
    public GemSlot CurGemSlot;
    [Header("功能参数")]
    public Vector3 ChildScale = Vector3.one;
    
    public override void SOnDrop(GameObject _childIns)
    {
        base.SOnDrop(_childIns);
        ChildIns.transform.localScale = ChildScale;
    }
}