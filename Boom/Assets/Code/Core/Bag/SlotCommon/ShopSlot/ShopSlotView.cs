using UnityEngine;

public class ShopSlotView : SlotView
{
    public override void Init()
    {
        var controller = new ShopSlotController();
        controller.Init(ViewSlotID, ViewSlotType);
        controller.BindView(this);
        Controller = controller;
    }

    public override void InitStep2() { }

    public override void Display(GameObject itemGO)
    {
        base.Display(itemGO);
        itemGO.transform.localScale = Vector3.one;
    }
}