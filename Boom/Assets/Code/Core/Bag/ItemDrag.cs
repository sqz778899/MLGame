using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDrag : DragBase
{
    internal override void DropSlot()
    {
        _dragIns.transform.position = _curSlot.transform.position;
    }
    
    internal override void NonFindSlot()
    {
        _dragIns.transform.position = originalPosition;
    }
}
