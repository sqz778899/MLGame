using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableBulletSpawner : BulletBase, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public int Count;
    public GameObject childBulletIns;
    public TextMeshProUGUI txtCount;
    bool IsChangeCount;

    void Start()
    {
        IsChangeCount = false;
        childBulletIns = null;
    }

    void Update()
    {
        base.Update();
        txtCount.text = "X" + Count;
    }
    void AddCount()
    {
        Count++;
    }

    void SubCount()
    {
        Count--;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (childBulletIns == null && Count > 0)
        {
            IsChangeCount = true;
            childBulletIns = BulletManager.Instance.InstanceBullet(_bulletData,BulletInsMode.EditA,transform.parent.position);
            childBulletIns.transform.SetParent(UIManager.Instance.GroupBullet.transform);
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
                if (curSlot.BulletID != 0)
                    continue;
                //........ChangeData.................
                CharacterManager.Instance.AddBullet(_bulletData.ID,curSlot.SlotID);
                DestroyImmediate(childBulletIns);
                return;
            }
        }

        // 如果这个位置下没有子弹槽，我们就将子弹位置恢复到原来的位置
        AddCount();
        DestroyImmediate(childBulletIns);
    }
    #endregion
}
