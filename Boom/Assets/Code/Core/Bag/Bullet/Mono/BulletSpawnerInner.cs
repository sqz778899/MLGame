using UnityEngine.EventSystems;

public class BulletSpawnerInner: BulletSpawnerNew
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        Spawner(eventData, BulletCreateFlag.SpawnerInner);
        //通知战斗镜头（战斗UI专属）
        EternalCavans.Instance.BagRootMini.GetComponent<BagRootMini>().BulletDragged();
    }
}