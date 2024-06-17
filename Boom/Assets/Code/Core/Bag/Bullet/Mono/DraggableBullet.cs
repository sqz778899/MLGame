using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableBullet : BulletBase, IPointerDownHandler, IPointerUpHandler, 
    IDragHandler,IPointerExitHandler,IPointerMoveHandler
{
    public int curSlotID = 0;
    public Vector3 originalPosition;
    bool IsToolTipsDisplay;

    void Start()
    {
        IsToolTipsDisplay = true;
        InitBulletData();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 记录下我们开始拖动时的位置
        originalPosition = transform.parent.position;
        DestroyTooltips();
    }

    void DragOneBullet(PointerEventData eventData,Transform curTrans)
    {
        // 在拖动时，我们把子弹位置设置为鼠标位置
        RectTransform rectTransform = curTrans.GetComponent<RectTransform>();
        Vector3 worldPoint;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out worldPoint))
        {
            rectTransform.position = worldPoint;
        }
        DestroyTooltips();
    }
    
    void DropOneBullet(PointerEventData eventData)
    {
        DestroyTooltips();
        IsToolTipsDisplay = true;
        // 在释放鼠标按钮时，我们检查这个位置下是否有一个子弹槽
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        bool NonHappen = true; // 发生逻辑，如果未发生，则之后子弹弹回原位
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("BulletSlotRole"))
            {
                BulletSlot curSlotSC = result.gameObject.GetComponent<BulletSlot>();
                //交换逻辑
                int curBulletInsID = eventData.pointerDrag.GetComponent<DraggableBullet>().InstanceID;
                bool isNullDrage = MainRoleManager.Instance.IsNullDrag(curBulletInsID);
                if (curSlotSC.BulletID != 0 || isNullDrage)
                {
                    MainRoleManager.Instance.BulletInterchangePos(curSlotID, curSlotSC.SlotID);
                }
                else
                {
                    //Add逻辑
                    MainRoleManager.Instance.AddBullet(_bulletData.ID,curSlotSC.SlotID,InstanceID);
                    MainRoleManager.Instance.SetBulletPos(transform.parent, result.gameObject.transform);
                }
                NonHappen = false;
            }

            if (result.gameObject.CompareTag("BulletSlot"))
            {
                //寻找母体
                DraggableBulletSpawner[] allSpawner = UIManager.Instance
                    .G_BulletSpawnerSlot.GetComponentsInChildren<DraggableBulletSpawner>();
                foreach (var each in allSpawner)
                {
                    if (each._bulletData.ID == _bulletData.ID)
                    {
                        MainRoleManager.Instance.SubBullet(_bulletData.ID,InstanceID);
                        Destroy(transform.parent.gameObject);
                    }
                }
                NonHappen = false;
            }
        }

        // 如果这个位置下没有子弹槽，我们就将子弹位置恢复到原来的位置
        if (NonHappen)
            transform.parent.position = originalPosition;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        Transform curTrans = transform.parent;
        DragOneBullet(eventData, curTrans);
        DestroyTooltips();
        IsToolTipsDisplay = false;
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        DropOneBullet(eventData);
        DestroyTooltips();
    }
    
    public void OnPointerMove(PointerEventData eventData)
    {
        if (!IsToolTipsDisplay) return;
        
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        Vector3 worldPoint;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, 
                eventData.position, eventData.pressEventCamera, out worldPoint))
        {
            DisplayTooltips(worldPoint);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DestroyTooltips();
    }
}