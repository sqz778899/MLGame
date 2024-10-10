using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDrag : ItermBase
{
    internal override void DropSlot()
    {
        _dragIns.transform.position = _curSlot.transform.position;
        CurItemSlotType = _curSlot.SlotType;//GO层同步
        //数据层同步
        SetItemData();
        MainRoleManager.Instance.RefreshCurItems();
    }
    
    internal override void NonFindSlot()
    {
        // 如果没有找到槽位，那么物品回到原始位置
        _dragIns.transform.position = originalPosition;
    }
}
