using System;
using UnityEngine;
using UnityEngine.Serialization;

public class GemSlotView:SlotView
{
    [Header("锁定UI的资产")]
    public GameObject LockedGO;
    public GemSlotInnerView InnerSlot;
    [SerializeField] Vector3 customScale = Vector3.one;

    public override void InitStep2()
    {
        Controller.LinkedInnerSlotController = InnerSlot.InnerController;
        InnerSlot.Controller = Controller;
        Controller.IsLocked = _state == UILockedState.isLocked; // ✅ 同步锁状态
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
            Controller.IsLocked = false; //实时同步逻辑状态
        }
        else if (State == UILockedState.isLocked)
        {
            LockedGO.SetActive(true);
            Controller.IsLocked = true; //实时同步逻辑状态
        }
    }
    #endregion
}