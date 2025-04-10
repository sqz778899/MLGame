using System;
using UnityEngine;
using UnityEngine.Serialization;

public class GemSlotView:SlotView
{
    [Header("锁定UI的资产")]
    public GameObject LockedGO;
    public GemSlotInnerView InnerSlot;
    [SerializeField] Vector3 customScale = Vector3.one;
    SlotController _controller;//把这个类的Controller也改成SlotController，避免多次转换

    public override void InitStep2()
    {
        _controller = Controller as SlotController;
        _controller.LinkedInnerSlotController = InnerSlot.InnerController;
        InnerSlot.Controller = Controller;
        _controller.IsLocked = _state == UILockedState.isLocked; //同步锁状态
    }

    public override void Display(GameObject itemGO)
    {
        base.Display(itemGO);
        itemGO.transform.localScale = customScale;
    }

    #region 锁定状态相关
    [SerializeField]
    UILockedState _state;
    public UILockedState State
    {
        set
        {
            if (_state != value)
                _state = value;
            ChangeState();
        }
        get { return _state; }
    }
    
    void ChangeState()
    {
        if (State == UILockedState.isNormal)
        {
            LockedGO.SetActive(false);
            _controller.IsLocked = false; //实时同步逻辑状态
        }
        else if (State == UILockedState.isLocked)
        {
            LockedGO.SetActive(true);
            _controller.IsLocked = true; //实时同步逻辑状态
        }
    }
    #endregion
}