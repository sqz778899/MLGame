using UnityEngine;

public class SlotIDCalculate : MonoBehaviour
{
    void Start()
    {
        InitSlotID();
    }

    public void InitSlotID()
    {
        BulletSlot[] bulletSlots = gameObject.GetComponentsInChildren<BulletSlot>();
        for (int i = 0; i < bulletSlots.Length; i++)
            bulletSlots[i].SlotID = i + 1;
    }
}
