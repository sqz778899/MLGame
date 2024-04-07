using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableBulletSpawner : BulletBase, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public int Count;
    public GameObject childBulletIns;
    public TextMeshProUGUI txtCount;
    GameObject GroupBullet;
    private bool IsChangeCount;

    void Start()
    {
        InitData();
    }

    public void InitData()
    {
        IsChangeCount = false;
        childBulletIns = null;
        GroupBullet = GameObject.Find("GroupBullet");
        txtCount.text = "X" + Count;
    }
    public void AddCount()
    {
        Count++;
        txtCount.text = "X" + Count;
    }
    
    public void SubCount()
    {
        Count--;
        txtCount.text = "X" + Count;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (childBulletIns == null && Count > 0)
        {
            IsChangeCount = true;
            childBulletIns = BulletManager.Instance.InstanceBullet(_bulletData,BulletInsMode.EditA,transform.parent.position);
            childBulletIns.transform.SetParent(GroupBullet.transform);
            childBulletIns.transform.localScale = Vector3.one;
            DraggableBullet DraBuSC = childBulletIns.GetComponentInChildren<DraggableBullet>();
            DraBuSC.originalPosition = transform.position;
        }
    }
    

    public void OnPointerUp(PointerEventData eventData)
    {
        if (Count >= 0 && childBulletIns != null)
            DropOneBullet(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Count >= 0 && childBulletIns != null)
        {
            if (IsChangeCount)
            {
                SubCount();
                IsChangeCount = false;
            }

            DragOneBullet(eventData, childBulletIns.transform);
        }
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
    
    void DropOneBullet(PointerEventData eventData)
    {
        // 在释放鼠标按钮时，我们检查这个位置下是否有一个子弹槽
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("BulletSlotRole"))
            {
                BulletSlotRole curSlot = result.gameObject.GetComponent<BulletSlotRole>();
                if (curSlot.IsHaveBullet)
                    continue;
                
                DraggableBullet tempSC = childBulletIns.GetComponentInChildren<DraggableBullet>();
                tempSC.SetBulletState(result.gameObject);
                BulletSlot curSlotSC = result.gameObject.GetComponent<BulletSlot>();
                if (curSlotSC == null)
                    tempSC.CurBagSlotID = 0;
                else
                    tempSC.CurBagSlotID = curSlotSC.SlotID;
                CharacterManager.Instance.InitData();
                // 如果有一个子弹槽，真正Spwan出来一个Bullet
                childBulletIns.transform.position = result.gameObject.transform.position;
                childBulletIns = null;
                curSlot.IsHaveBullet = true;
                return;
            }
        }

        // 如果这个位置下没有子弹槽，我们就将子弹位置恢复到原来的位置
        AddCount();
        DestroyImmediate(childBulletIns);
    }
    #endregion
}
