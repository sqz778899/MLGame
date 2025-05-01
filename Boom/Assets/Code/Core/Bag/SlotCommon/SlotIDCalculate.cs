using UnityEngine;

public class SlotIDCalculate : MonoBehaviour
{
    public void InitSlotID()
    {
        SlotView[] slots = GetComponentsInChildren<SlotView>(true);
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].ViewSlotID = i + 1;
        }
    }
}
