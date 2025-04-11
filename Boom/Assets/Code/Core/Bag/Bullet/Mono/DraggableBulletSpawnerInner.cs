using System;
using UnityEngine.EventSystems;

public class DraggableBulletSpawnerInner:DraggableBulletSpawner
{
    BagRootMini _bagRootMini;
    public Action OnBulletDragged;
    
    internal override void Start()
    {
        base.Start();
        _bagRootMini = UIManager.Instance.BagUI.BagRootMiniGO.GetComponent<BagRootMini>();
        OnBulletDragged += _bagRootMini.BulletDragged;
    }
    
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        OnBulletDragged?.Invoke();
    }
    
    void OnDestroy()
    {
        if (_data != null)
            _data.OnDataChanged -= OnDataChangedSpawner;
        if (_bagRootMini != null)
            OnBulletDragged -= _bagRootMini.BulletDragged;
    }
}