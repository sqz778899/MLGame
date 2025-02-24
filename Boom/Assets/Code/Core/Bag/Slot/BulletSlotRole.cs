using System.Collections.Generic;
using UnityEngine;

public class BulletSlotRole: BulletSlot
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
    [Header("Gems")]
    public List<GemSlot> GemSlots;
    
    void Awake()
    {
        ChangeState();
    }
    
    void ChangeState()
    {
        switch (State)
        {
            case UILockedState.isNormal:
                Locked.SetActive(false);
                GemSlots.ForEach(gemslot => gemslot.State = UILockedState.isNormal);
                break;
            case UILockedState.isLocked:
                Locked.SetActive(true);
                GemSlots.ForEach(gemslot => gemslot.State = UILockedState.isLocked);
                break;
        }
    }
}