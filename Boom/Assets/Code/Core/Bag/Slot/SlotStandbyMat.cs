using UnityEngine;

public class SlotStandbyMat: BulletSlot
{
    public void AddIns(GameObject StandbyMat)
    {
        StandbyBulletMat curSC = StandbyMat.GetComponent<StandbyBulletMat>();
        BulletID = curSC.ID;
        InstanceID = StandbyMat.GetInstanceID();

        RectTransform curRect = GetComponent<RectTransform>();
        RectTransform StandebyMatRect = StandbyMat.GetComponent<RectTransform>();
        StandebyMatRect.anchoredPosition = curRect.anchoredPosition;
        StandebyMatRect.rotation = curRect.rotation;
    }
}