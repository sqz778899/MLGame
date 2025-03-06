using UnityEngine;

public class SlotBase : MonoBehaviour
{
    public int SlotID;
    public int MainID;
    public SlotType SlotType;
    public GameObject ChildIns;

    public virtual void SOnDrop(GameObject _childIns)
    {
        ChildIns = _childIns;
        ChildIns.transform.position = transform.position;
        ChildIns.transform.SetParent(transform,true);
        ChildIns.transform.localScale = Vector3.one;
        ItemBase curSC = _childIns.GetComponentInChildren<ItemBase>();
        if (curSC is Bullet _bulletNew)
        {
            _bulletNew._data.CurSlot = this;
            MainID = _bulletNew._data.ID;
        }
        if (curSC is Gem _gemNew)
        {
            _gemNew._data.CurSlot = this;
            MainID = _gemNew._data.ID;
        }
        
    }
}