using UnityEngine;

public class SlotIDCalculate : MonoBehaviour
{
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
        }
        
        GemSlot[] gemSlots = gameObject.GetComponentsInChildren<GemSlot>();
        for (int i = 0; i < gemSlots.Length; i++)
            gemSlots[i].SlotID = i + 1;
    }
    
    [Header("同步影分身Slot地址工具")]
    public GameObject SourceRoot;
    public GameObject TargetRoot;
    public void SyncSlotGem()
    {
        if (SourceRoot == null || TargetRoot == null) return;
        
        GemSlot[] sourceSlots = SourceRoot.GetComponentsInChildren<GemSlot>();
        GemSlotInner[] targetSlots = TargetRoot.GetComponentsInChildren<GemSlotInner>();

        foreach (var eachS in sourceSlots)
        {
            foreach (var eachT in targetSlots)
            {
                if (eachS.SlotID == eachT.SlotID)//双持，互相持有
                {
                    eachT.CurGemSlot = eachS;
                    eachS.CurGemSlotInner = eachT;
                }
            }
        }
    }
}
