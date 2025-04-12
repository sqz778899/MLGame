using System;
using UnityEngine;


public class SlotView:MonoBehaviour
{
    public int ViewSlotID;
    public SlotType ViewSlotType;
    public int InstanceID;
    public bool IsLocked;
    public ISlotController Controller;

    public virtual void Init()
    {
        InstanceID = GetInstanceID();
        var controller = new GemSlotController(); // 或 BulletSlotController
        controller.Init(ViewSlotID, ViewSlotType); // 用公开方法初始化
        controller.BindView(this);
        Controller = controller;
    }

    public virtual void InitStep2() {}

    public virtual void Display(GameObject itemGO)
    {
        itemGO.transform.SetParent(transform);
        itemGO.transform.localPosition = Vector3.zero;
        itemGO.transform.localScale = Vector3.one;
    }

    public virtual void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
            Destroy(transform.GetChild(i).gameObject);
    }
}