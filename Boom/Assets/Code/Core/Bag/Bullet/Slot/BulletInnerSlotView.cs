using UnityEngine;
using UnityEngine.UI;

public class BulletInnerSlotView : SlotView
{
    public BulletInnerSlotController BulletInnerController;
    [SerializeField] GameObject bubble;
    
    public override void Init()
    {
        BulletInnerController = new BulletInnerSlotController();
        BulletInnerController.BindView(this);
    }

    public void SetLocked(bool locked) => GetComponent<Image>().color = locked ? 
            new Color(1, 1, 1, 0.21f) : new Color(0, 0, 0, 0.21f);

    public void UpdateBubble(bool show)
    {
        bubble.SetActive(show);
    }
}