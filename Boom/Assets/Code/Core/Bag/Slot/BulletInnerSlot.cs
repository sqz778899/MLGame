using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInnerSlot : SlotBase
{
    //去持有真正的角色槽。做到地址统一，处理同一份数据
    public BulletSlotRole CurBulletSlotRole;
}
