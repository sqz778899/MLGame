using UnityEngine;

public class SlotStandbyMat: BulletSlot
{
    public void AddIns(GameObject StandbyMat)
    {
        StandbyBulletMat curSC = StandbyMat.GetComponent<StandbyBulletMat>();
        MainID = curSC.ID;

        RectTransform curRect = GetComponent<RectTransform>();
        RectTransform StandebyMatRect = StandbyMat.GetComponent<RectTransform>();
        StandebyMatRect.anchoredPosition = curRect.anchoredPosition;
        StandebyMatRect.rotation = curRect.rotation;
    }
}