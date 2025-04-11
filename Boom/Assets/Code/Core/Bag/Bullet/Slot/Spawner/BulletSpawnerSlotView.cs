public class BulletSpawnerSlotView: SlotView
{
    BulletSlotSpawnerController _controller;
    
    public override void Init()
    {
        var controller = new BulletSlotSpawnerController();
        controller.Init(ViewSlotID, ViewSlotType); // 用公开方法初始化
        controller.BindView(this);
        Controller = controller;
    }
}