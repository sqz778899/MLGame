using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class DraggableBulletSpawner :Bullet
{
    public int Count;
    public GameObject childBulletIns;
    public TextMeshProUGUI txtCount;

    internal override void Start()
    {
        base.Start();
        childBulletIns = null;
    }

    void Update()
    {
        txtCount.text = "X" + Count;
    }
    
    public override void OnPointerDown(PointerEventData eventData)
    {
        HideTooltips();
        if (childBulletIns == null && Count > 0)
        {
            childBulletIns = BulletManager.Instance.InstanceBullet(ID,BulletInsMode.EditA,transform.parent.position);
            childBulletIns.transform.SetParent(UIManager.Instance.DragObjRoot.transform);
            childBulletIns.transform.localScale = Vector3.one;
            Bullet DraBuSC = childBulletIns.GetComponentInChildren<Bullet>();
            DraBuSC.originalPosition = transform.position;
            MainRoleManager.Instance.TmpHongSpawner(ID);
            MainRoleManager.Instance.RefreshAllItems();
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        HideTooltips();
        if (Count >= 0 && childBulletIns != null)
        {
            childBulletIns.GetComponentInChildren<DraggableBullet>().DropOneBullet(eventData);
            childBulletIns = null;
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        HideTooltips();
        if (Count >= 0 && childBulletIns != null)
        {
            // 在拖动时，我们把子弹位置设置为鼠标位置
            RectTransform rectTransform = childBulletIns.transform.GetComponent<RectTransform>();
            Vector3 worldPoint;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, 
                    eventData.position, eventData.pressEventCamera, out worldPoint))
                rectTransform.position = worldPoint;
        }
    }
    public override void OnPointerMove(PointerEventData eventData)
    {
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, 
                eventData.position, eventData.pressEventCamera, out Vector3 worldPoint))
        {
            DisplayTooltips(eventData);
        }
    }
}
