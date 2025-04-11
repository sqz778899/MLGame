using UnityEngine;

public class GemSlotInnerView:SlotView
{
    [Header("功能参数")]
    public Vector3 ChildScale = Vector3.one;
    public GemInnerSlotController GemInnerController;

    public override void Init()
    {
        GemInnerController = new GemInnerSlotController();
        GemInnerController.BindView(this);
    }
    
    public override void Display(GameObject itemGO)
    {
        base.Display(itemGO);
        itemGO.transform.localScale = ChildScale;
    }
}