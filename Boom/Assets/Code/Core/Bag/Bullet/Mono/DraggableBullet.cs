using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableBullet : Bullet
{
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

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        BulletInsMode = BulletInsMode.EditA;
    }

    public void DropOneBullet(PointerEventData eventData)
    {
        UIManager.Instance.IsLockedClick = false;
        
        if (eventData.button == PointerEventData.InputButton.Right)
            return;
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
                BulletSlot curSlotSC = result.gameObject.GetComponent<BulletSlot>();
                //MainRoleManager.Instance.AddBulletOnlyData(ID,curSlotSC.SlotID,InstanceID);
                if (curSlotSC.MainID == -1)
                {
                    CurSlot = curSlotSC;
                    //清除旧的Slot信息
                    SlotManager.ClearBagSlotByID(SlotID,SlotType.CurBulletSlot);
                    //同步新的Slot信息
                    CurSlot.SOnDrop(Ins,SlotType.CurBulletSlot);
                    MainRoleManager.Instance.RefreshAllItems();
                }
                else
                {
                    //
                    GameObject orIns = curSlotSC.ChildIns;
                    CurSlot.SOnDrop(orIns,SlotType.CurBulletSlot);
                    curSlotSC.SOnDrop(Ins,SlotType.CurBulletSlot);
                    MainRoleManager.Instance.RefreshAllItems();
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
            BulletInsMode = BulletInsMode.EditB;
            Ins.transform.position = originalPosition;
            _dragIns.transform.SetParent(originalParent,true);
        }
        
        MainRoleManager.Instance.RefreshAllItems();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        DropOneBullet(eventData);
        DestroyTooltips();
    }
}