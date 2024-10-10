using UnityEngine;

public class SlotIDCalculate : MonoBehaviour
{
    public SlotType SlotType;
    void Start()
    {
        InitSlotID();
    }

    public void InitSlotID()
    {
        BulletSlot[] bulletSlots = gameObject.GetComponentsInChildren<BulletSlot>();
        for (int i = 0; i < bulletSlots.Length; i++)
            bulletSlots[i].SlotID = i + 1;
        
        SlotBase[] bagSlots = gameObject.GetComponentsInChildren<SlotBase>();
        for (int i = 0; i < bagSlots.Length; i++)
        {
            SlotBase curBase = bagSlots[i];
            curBase.SlotID = i + 1;
            curBase.MainID = -1;
            curBase.InstanceID = -1;
            curBase.SlotType = SlotType;
        }
    }
}
