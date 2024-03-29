using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableBullet : BulletBase, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public BulletEditMode BulletState = BulletEditMode.Non;
    public int CurBagSlotID = 0;
    public Vector3 originalPosition;
    GameObject GroupBulletSlot;

    void Start()
    {
        InitBulletData();
        GroupBulletSlot = GameObject.Find("GroupBulletSlot");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 记录下我们开始拖动时的位置
        originalPosition = transform.parent.position;
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
                SetBulletState(result.gameObject);
                BulletSlot curSlotSC = result.gameObject.GetComponent<BulletSlot>();
                if (curSlotSC == null)
                    CurBagSlotID = 0;
                else
                    CurBagSlotID = curSlotSC.SlotID;
                CharacterManager.Instance.SetBullet();
                // 如果有一个子弹槽，我们就将子弹放到子弹槽中
                transform.parent.position = result.gameObject.transform.position;
                return;
            }

            if (result.gameObject.CompareTag("BulletSlot"))
            {
                //寻找母体
                DraggableBulletSpawner[] allSpawner = GroupBulletSlot.GetComponentsInChildren<DraggableBulletSpawner>();
                foreach (var each in allSpawner)
                {
                    if (each._bulletData.ID == _bulletData.ID)
                    {
                        each.AddCount();
                        DestroyImmediate(transform.parent.gameObject);
                        CharacterManager.Instance.SetBullet();
                        return;
                    }
                }
            }
        }

        // 如果这个位置下没有子弹槽，我们就将子弹位置恢复到原来的位置
        transform.parent.position = originalPosition;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        Transform curTrans = transform.parent;
        DragOneBullet(eventData, curTrans);
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        DropOneBullet(eventData);
    }

    public void SetBulletState(GameObject Slot)
    {
        BulletEditMode curBulletState = BulletEditMode.Non;
        string SlotName = Slot.name;
        switch (SlotName)
        {
            case "imSlotRole01":
                curBulletState = BulletEditMode.SlotRole01;
                break;
            case "imSlotRole02":
                curBulletState = BulletEditMode.SlotRole02;
                break;
            case "imSlotRole03":
                curBulletState = BulletEditMode.SlotRole03;
                break;
            case "imSlotRole04":
                curBulletState = BulletEditMode.SlotRole04;
                break;
            case "imSlotRole05":
                curBulletState = BulletEditMode.SlotRole05;
                break;
            default:
                break;
        }
        BulletState = curBulletState;
    }
}