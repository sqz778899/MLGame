using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableBulletSpawner : Bullet, IPointerDownHandler, 
    IPointerUpHandler, IDragHandler,IPointerExitHandler,IPointerMoveHandler
{
    public int Count;
    public GameObject childBulletIns;
    public TextMeshProUGUI txtCount;

    internal void Start()
    {
        base.Start();
        childBulletIns = null;
    }

    void Update()
    {
        txtCount.text = "X" + Count;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        DestroyTooltips();
        if (childBulletIns == null && Count > 0)
        {
            childBulletIns = BulletManager.Instance.InstanceBullet(ID,BulletInsMode.EditA,transform.parent.position);
            childBulletIns.transform.SetParent(UIManager.Instance.DragObjRoot.transform);
            childBulletIns.transform.localScale = Vector3.one;
            DraggableBullet DraBuSC = childBulletIns.GetComponentInChildren<DraggableBullet>();
            DraBuSC.originalPosition = transform.position;
            MainRoleManager.Instance.TmpHongSpawner(ID);
            MainRoleManager.Instance.RefreshAllItems();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        DestroyTooltips();
        if (Count >= 0 && childBulletIns != null)
        {
            childBulletIns.GetComponentInChildren<DraggableBullet>().DropOneBullet(eventData);
            childBulletIns = null;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        DestroyTooltips();
        if (Count >= 0 && childBulletIns != null)
        {
            DragOneBullet(eventData, childBulletIns.transform);
        }
    }
    public void OnPointerMove(PointerEventData eventData)
    {
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        Vector3 worldPoint;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out worldPoint))
        {
            DisplayTooltips(worldPoint);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DestroyTooltips();
    }
    
    #region MyRegion
    void DragOneBullet(PointerEventData eventData,Transform curTrans)
    {
        // 在拖动时，我们把子弹位置设置为鼠标位置
        RectTransform rectTransform = curTrans.GetComponent<RectTransform>();
        Vector3 worldPoint;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out worldPoint))
        {
            rectTransform.position = worldPoint;
        }
    }
    #endregion
    
}
