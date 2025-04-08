using System;
using UnityEngine;


public class SlotView:MonoBehaviour
{
    public int ViewSlotID;
    public SlotType ViewSlotType;

    private void Awake()
    {
        Init(new SlotController{
            SlotID = ViewSlotID,
            SlotType = ViewSlotType
        });
    }

    public SlotController Controller { get; private set; }

    public void Init(SlotController controller)
    {
        Controller = controller;
        Controller.BindView(this);
    }

    public void Display(GameObject itemGO)
    {
        itemGO.transform.SetParent(transform);
        itemGO.transform.localPosition = Vector3.zero;
        itemGO.transform.localScale = Vector3.one;
    }

    public void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
            Destroy(transform.GetChild(i));
    }
}