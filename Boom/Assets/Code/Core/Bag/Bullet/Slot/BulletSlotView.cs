using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletSlotView: SlotView
{
    [Header("锁定状态")]
    public GameObject LockedGO;
    public BulletInnerSlotView InnerSlot;
    [SerializeField] List<GemSlotView> gemSlotViews;
    BulletSlotController _controller;//把这个类的Controller也改成BulletSlotController，避免多次转换

    public override void Init()
    {
        var controller = new BulletSlotController();
        controller.Init(ViewSlotID, ViewSlotType); // 用公开方法初始化
        controller.BindView(this);
        Controller = controller;
    }
    
    public override void InitStep2()
    {
        _controller = Controller as BulletSlotController;
        _controller.LinkedInnerSlotController = InnerSlot.BulletInnerController;
        InnerSlot.Controller = Controller;
        _controller.IsLocked = _state == UILockedState.isLocked; //同步锁状态
    }
    
    public override void Display(GameObject itemGO)
    {
        base.Display(itemGO);
        // 切换成 EditB 模式
        if (itemGO.TryGetComponent<BulletNew>(out var bullet))
            bullet.SwitchMode(BulletInsMode.EditB);
        // 落下即刻显示Tooltips
        TooltipsManager.Instance.Enable();
        if (itemGO.TryGetComponent<ItemInteractionHandler>(out var bulletInteractionHandler))
            bulletInteractionHandler.ShowTooltips();
    }


    #region 管理宝石槽
    [SerializeField] UILockedState _state;
    public UILockedState State
    {
        get => _state;
        set
        {
            if (_state != value)
            {
                _state = value;
                ChangeState();
            }
        }
    }
    void ChangeState()
    {
        bool locked = State == UILockedState.isLocked;
        LockedGO?.SetActive(locked);
        gemSlotViews.ForEach(g => g.State = State);
    }
    #endregion
}