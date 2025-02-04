using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableBullet : Bullet, IPointerDownHandler, IPointerUpHandler, 
    IDragHandler,IPointerExitHandler,IPointerMoveHandler
{
    public Vector3 originalPosition;
    BulletInsMode preBulletInsMode;
    bool IsToolTipsDisplay;

    internal void Start()
    {
        base.Start();
        IsToolTipsDisplay = true;
        preBulletInsMode = BulletInsMode;
        SyncData();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 记录下我们开始拖动时的位置
        preBulletInsMode = BulletInsMode;
        originalPosition = Ins.transform.position;
        DestroyTooltips();
        MainRoleManager.Instance.RefreshAllItems();
    }

    void DragOneBullet(PointerEventData eventData)
    {
        if (BulletInsMode == BulletInsMode.EditB)
        {
            BulletInsMode = BulletInsMode.EditA;
            SyncData();
        }
        // 在拖动时，我们把子弹位置设置为鼠标位置
        RectTransform rectTransform = Ins.GetComponent<RectTransform>();
        Vector3 worldPoint;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out worldPoint))
        {
            rectTransform.position = worldPoint;
        }
        DestroyTooltips();
    }

    void ReturnToSpawner()
    {
        DraggableBulletSpawner[] allSpawner = UIManager.Instance
            .G_BulletSpawnerSlot.GetComponentsInChildren<DraggableBulletSpawner>();
        foreach (var each in allSpawner)
        {
            if (each.ID == ID)
            {
                MainRoleManager.Instance.SubBullet(ID,InstanceID);
                Destroy(Ins);
            }
        }
    }
    
    public void DropOneBullet(PointerEventData eventData)
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
                BulletInsMode = BulletInsMode.EditB;
                SyncData();
                BulletSlot curSlotSC = result.gameObject.GetComponent<BulletSlot>();
                bool InstanceIDIsInCurBullet = MainRoleManager.Instance.InstanceIDIsInCurBullet(InstanceID);
                if (curSlotSC.BulletID == 0)
                {
                    if (!InstanceIDIsInCurBullet)
                    {
                        //Add逻辑
                        MainRoleManager.Instance.AddBulletOnlyData(ID,curSlotSC.SlotID,InstanceID);
                        Ins.transform.SetParent(UIManager.Instance.DragObjRoot.transform,false);
                        Ins.transform.position = curSlotSC.transform.position;
                    }
                    else
                    {
                        //空拖逻辑
                        MainRoleManager.Instance.BulletInterchangePos(SlotID, curSlotSC.SlotID);
                    }
                }
                else if(InstanceIDIsInCurBullet)
                {
                    //交换逻辑
                    MainRoleManager.Instance.BulletInterchangePos(SlotID, curSlotSC.SlotID);
                }
                else
                {
                    ReturnToSpawner();
                }
                NonHappen = false;
            }

            if (result.gameObject.CompareTag("BulletSlot"))
            {
                //寻找母体
                ReturnToSpawner();
                NonHappen = false;
                break;
            }
        }

        // 如果这个位置下没有子弹槽，我们就将子弹位置恢复到原来的位置
        if (NonHappen)
        {
            if (preBulletInsMode == BulletInsMode.EditB)
            {
                BulletInsMode = BulletInsMode.EditB;
                SyncData();
            }
            Ins.transform.position = originalPosition;
        }
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        DragOneBullet(eventData);
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