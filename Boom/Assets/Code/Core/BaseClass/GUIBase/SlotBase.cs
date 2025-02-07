using UnityEngine;

public class SlotBase : MonoBehaviour
{
    public int SlotID;
    public int MainID;
    public SlotType SlotType;
    public GameObject ChildIns;

    public virtual void SOnDrop(GameObject _childIns,SlotType _slotType)
    {
        ChildIns = _childIns;
        ChildIns.transform.position = transform.position;
        ChildIns.transform.SetParent(transform,true);
        ChildIns.transform.localScale = Vector3.one;
        SlotType = _slotType;
        DragBase dragBaseSC = _childIns.GetComponentInChildren<DragBase>();
        //同步新的Slot信息
        MainID = dragBaseSC.ID;
        dragBaseSC.CurSlot = this;
        dragBaseSC.SlotID = SlotID;
        dragBaseSC.SlotType = SlotType;
    }
}