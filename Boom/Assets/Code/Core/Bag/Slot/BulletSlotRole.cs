using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletSlotRole: SlotBase
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

    [Header("持有子弹的重要数据")] 
    BulletData _curBulletData;
    public BulletData CurBulletData
    {
        get => _curBulletData;
        set
        {
            if (_curBulletData != value)
            {
                _curBulletData = value;
                OnBulletDataChange();
            }
        }
    }
    
    [Header("锁定的美术资源")] 
    public GameObject Locked;
    [Header("Gems")]
    public List<GemSlot> GemSlots;
    
    void Awake()
    {
        ChangeState();
        foreach (var each in GemSlots)
            each.OnGemDataChange += RefreshModifiers;
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

    void RefreshModifiers()
    {
        if (CurBulletData == null) return;
        CurBulletData.ClearModifiers();
        foreach (var each in GemSlots)
        {
            if(each.CurGemData == null)
                continue;
            CurBulletData.AddModifier(new BulletModifierGem(each.CurGemData));
        }
    }

    void OnBulletDataChange()
    {
        RefreshModifiers();
    }

    public void SOnDrop(BulletData bulletData)
    {
        bulletData.CurSlot = this;
        MainID = bulletData.ID;
        CurBulletData = bulletData;
    }

    public override void SOnDrop(GameObject _childIns)
    {
        base.SOnDrop(_childIns);
        ItemBase curSC = _childIns.GetComponentInChildren<ItemBase>();
        Bullet _bulletNew = curSC as Bullet;
        _bulletNew._data.CurSlot = this;
        MainID = _bulletNew._data.ID;
        
        //持有这个BulletData
        CurBulletData = _bulletNew._data;
    }
}