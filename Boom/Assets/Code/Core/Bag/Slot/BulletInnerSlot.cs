using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInnerSlot : SlotBase
{
    //去持有真正的角色槽。做到地址统一，处理同一份数据
    public BulletSlotRole CurBulletSlotRole;
    
    [Header("气泡表现资产")]
    public GameObject BubbleGO;
    
    void Start()
    {
        CurBulletSlotRole.OnIsHaveBullet += OnOffBubble;
        OnOffBubble();
    }

    public override void SOnDrop(GameObject _childIns)
    {
        base.SOnDrop(_childIns);
        
        ItemBase curSC = _childIns.GetComponentInChildren<ItemBase>();
        Bullet _bulletNew = curSC as Bullet;
        _bulletNew._data.CurSlot = CurBulletSlotRole;
        CurBulletSlotRole.MainID = _bulletNew._data.ID;
    }

    void OnOffBubble()
    {
        if (CurBulletSlotRole.CurBulletData == null)
            BubbleGO.SetActive(false);
        else
            BubbleGO.SetActive(true);
    }
}
