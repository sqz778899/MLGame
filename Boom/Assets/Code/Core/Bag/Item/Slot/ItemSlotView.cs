using UnityEngine;

public class ItemSlotView:SlotView
{
    [SerializeField] 
    Vector3 customScale = Vector3.one;
    ItemSlotController _controller;
    public override void Init()
    {
        InstanceID = GetInstanceID();
        ItemSlotController controller = new ItemSlotController();
        controller.Init(ViewSlotID, ViewSlotType); // 用公开方法初始化
        controller.BindView(this);
        Controller = controller;
        _controller = controller;
    }
    
    public override void Display(GameObject itemGO)
    {
        base.Display(itemGO);
        itemGO.transform.localScale = customScale;
        itemGO.GetComponent<ItemNew>().SyncBackground();
    }
}