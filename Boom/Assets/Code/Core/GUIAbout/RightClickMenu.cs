using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RightClickMenu : MonoBehaviour,IPointerExitHandler
{
    public GameObject CurIns;

    public void DeleteIns()
    {
        if (CurIns==null)
            return;
        ItemManager.DeleteItem(CurIns);
        Destroy(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(gameObject);
    }
}
