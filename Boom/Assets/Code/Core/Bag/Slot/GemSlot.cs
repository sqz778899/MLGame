using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSlot : SlotBase
{
    [SerializeField]
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
    public override void SOnDrop(GameObject _childIns)
    {
        base.SOnDrop(_childIns);
        ChildIns.transform.localScale = ChildScale;
        ChildIns.GetComponent<Gem>().BulletSlotIndex = BulletSlotIndex;
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
