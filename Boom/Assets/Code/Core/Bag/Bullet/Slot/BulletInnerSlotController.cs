using UnityEngine;

public class BulletInnerSlotController
{
    BulletInnerSlotView _view;

    public void BindView(BulletInnerSlotView view) => _view = view;

    public void SyncFrom(BulletData data)
    {
        GameObject shadow = BulletFactory.CreateBullet(data, BulletInsMode.EditB).gameObject;
        _view.Display(shadow);
    }

    public void Clear() => _view?.Clear();
}