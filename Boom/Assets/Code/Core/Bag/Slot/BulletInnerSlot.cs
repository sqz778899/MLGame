using UnityEngine;
using UnityEngine.UI;

public class BulletInnerSlot : SlotBase
{
    //去持有真正的角色槽。做到地址统一，处理同一份数据
    public BulletSlotRole CurBulletSlotRole;
    
    [Header("气泡表现资产")]
    public GameObject BubbleGO;
    [Header("锁定资产")] 
    public GameObject LockedGO;
    
    void Start()
    {
        CurBulletSlotRole.OnIsHaveBullet += OnOffBubble;
        OnOffBubble();
    }

    public void InitData()
    {
        if (CurBulletSlotRole.State == UILockedState.isLocked)
            GetComponent<Image>().color = new Color(1, 1, 1, 0.21f);
        else
            GetComponent<Image>().color =new Color(0, 0, 0, 0.21f);
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

    void OnDestroy() => CurBulletSlotRole.OnIsHaveBullet -= OnOffBubble;
}
