using UnityEngine;
using UnityEngine.UI;

public class BulletInnerSlotView : SlotView
{
    public BulletInnerSlotController BulletInnerController;
    [SerializeField]
    GameObject bubble;
    [SerializeField]
    Image backGround;
    
    public override void Init()
    {
        BulletInnerController = new BulletInnerSlotController();
        BulletInnerController.Init(ViewSlotID, ViewSlotType); // 用公开方法初始化
        BulletInnerController.BindView(this);
        Controller = BulletInnerController;
        bubble.SetActive(false);
    }

    public void SetLocked(bool locked)
    {
        IsLocked = locked;
        backGround.color = locked ? 
            new Color(1, 1, 1, 0.21f) : new Color(0, 0, 0, 0.21f);
    }

    public override void Display(GameObject itemGO) => bubble.SetActive(true);

    public override void Clear() => bubble.SetActive(false);
}