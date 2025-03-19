using System;
using UnityEngine;

public class GemSlot : SlotBase
{
    public Action OnGemDataChange;
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

    [Header("持有宝石&&InnerSlot的重要数据")]
    GemData _curGemData;
    public GemData CurGemData
    {
        get => _curGemData;
        set
        {
            if (_curGemData != value)
            {
                _curGemData = value;
                OnGemDataChange?.Invoke();
            }
        }
    }
    public GemSlotInner CurGemSlotInner;
    
    [Header("锁定的美术资源")] 
    public GameObject Locked;
    [Header("功能参数")]
    public Vector3 ChildScale = Vector3.one;

  
    public override void SOnDrop(GameObject _childIns)
    {
        base.SOnDrop(_childIns);
        ChildIns.transform.localScale = ChildScale;
        
        ItemBase curSC = _childIns.GetComponentInChildren<ItemBase>();
        Gem _gemNew = curSC as Gem;
        //先清理之前的Slot的影子信息
        SlotManager.ClearGemSlot(_gemNew._data.CurSlot);
        _gemNew._data.CurSlot = this;
        MainID = _gemNew._data.ID;

        CurGemData = _gemNew._data;
        //在GemSlotInner部分创建一个影分身
        GameObject _newChildeIns = BagItemTools<GemInner>.CreateTempObjectGO(_gemNew._data,CreateItemType.MiniBagGem);
        if (CurGemSlotInner != null)
        {
            SlotManager.ClearSlot(CurGemSlotInner);
            CurGemSlotInner.SOnDrop(_newChildeIns);
        }
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
