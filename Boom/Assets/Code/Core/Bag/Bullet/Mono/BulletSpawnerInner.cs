﻿using UnityEngine.EventSystems;

public class BulletSpawnerInner: BulletSpawner
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        Spawner(eventData, BulletCreateFlag.SpawnerInner);
        //通知战斗镜头（战斗UI专属）
        EternalCavans.Instance.BagRootMini.GetComponent<BagRootMini>().BulletDragged();
    }
}