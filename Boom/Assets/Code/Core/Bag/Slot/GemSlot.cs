using System;
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
                InventoryManager.Instance._BulletInvData.RefreshModifiers();
            }
        }
    }
    public GemSlotInner CurGemSlotInner;
    
    [Header("锁定的美术资源")] 
    public GameObject Locked;
    [Header("功能参数")]
    public Vector3 ChildScale = Vector3.one;
    
    void Awake() => ChangeState();

    public override void SOnDrop(GameObject _childIns)
    {
        base.SOnDrop(_childIns);
        ChildIns.transform.localScale = ChildScale;
        
        ItemBase curSC = _childIns.GetComponentInChildren<ItemBase>();
        Gem _gemNew = curSC as Gem;
        //设置一下ToolTips根据不同Slot的偏移
        if (SlotType == SlotType.GemBagSlot)
            _gemNew.ToolTipsOffset = new Vector3(1.01f, -0.5f, 0);
        if (SlotType == SlotType.GemInlaySlot)
            _gemNew.ToolTipsOffset = new Vector3(-0.92f, -0.52f, 0);
        
        //先清理之前的Slot的影子信息
        if (_gemNew._data.CurSlot.SlotType == SlotType.GemBagSlot)
            InventoryManager.Instance._InventoryData.RemoveGemToBag(_gemNew._data);
        else
            InventoryManager.Instance._InventoryData.UnEquipGem(_gemNew._data);
        SlotManager.ClearGemSlot(_gemNew._data.CurSlot);
        _gemNew._data.CurSlot = this;
        MainID = _gemNew._data.ID;
        
        //同步到数据层
        CurGemData = _gemNew._data;
        if (SlotType == SlotType.GemBagSlot)
            InventoryManager.Instance._InventoryData.AddGemToBag(CurGemData);
        else
            InventoryManager.Instance._InventoryData.EquipGem(CurGemData);
        
        //在GemSlotInner部分创建一个影分身
        GameObject _newChildeIns = BagItemTools<GemInner>.
            CreateTempObjectGO(_gemNew._data,CreateItemType.MiniBagGem);
        if (CurGemSlotInner != null)
        {
            SlotManager.ClearSlot(CurGemSlotInner);
            CurGemSlotInner.SOnDrop(_newChildeIns);
        }
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
