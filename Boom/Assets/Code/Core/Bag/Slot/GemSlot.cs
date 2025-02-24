using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSlot : SlotBase
{
    UILockedState _state;
    public UILockedState State
    {
        set
        {
            if (_state != value)
            {
                _state = value;
                ChangeState();
            }
        }
        get { return _state; }
    }
    [Header("锁定的美术资源")] 
    public GameObject Locked;
    [Header("功能参数")]
    public int BulletSlotIndex;
    public Vector3 ChildScale = Vector3.one;
    public override void SOnDrop(GameObject _childIns,SlotType _slotType)
    {
        base.SOnDrop(_childIns,_slotType);
        ChildIns.transform.localScale = ChildScale;
    }
    
    void Awake()
    {
        ChangeState();
    }
    
    void ChangeState()
    {
        switch (State)
        {
            case UILockedState.isNormal:
                if (Locked != null)
                    Locked.SetActive(false);
                break;
            case UILockedState.isLocked:
                if (Locked != null)
                    Locked.SetActive(true);
                break;
        }
    }
}
