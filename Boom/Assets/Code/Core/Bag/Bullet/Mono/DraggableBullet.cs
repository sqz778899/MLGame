using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableBullet : BulletNew
{
    public bool IsSpawnerCreate = false;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        BulletInsMode = BulletInsMode.EditA;
    }

    public void DropOneBullet(PointerEventData eventData)
    {
        UIManager.Instance.IsLockedClick = false;
        if (eventData.button == PointerEventData.InputButton.Right) return;
        
        HideTooltips();
        // 在释放鼠标按钮时，我们检查这个位置下是否有一个子弹槽
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        bool nonHappen = true; // 发生逻辑，如果未发生，则之后子弹弹回原位
        
        foreach (var result in results)
        {
            if (HandleBulletSlotRole(result) || HandleBulletSlot(result) || HandleBulletInnerSlot(result))
            {
                nonHappen = false;
                break;
            }
        }
        // 如果这个位置下没有子弹槽，我们就将子弹位置恢复到原来的位置
        if (nonHappen)
        {
            if (IsSpawnerCreate) ReturnToSpawner();
            else ResetPosition();
        }
        MainRoleManager.Instance.RefreshAllItems();
    }
    
    public override void OnPointerUp(PointerEventData eventData)
    {
        DropOneBullet(eventData);
        HideTooltips();
    }

    #region 私有方法
    bool HandleBulletSlotRole(RaycastResult result)
    {
        if (!result.gameObject.CompareTag("BulletSlotRole")) return false;
        var curSlotSC = result.gameObject.GetComponent<BulletSlotRole>();
        if (curSlotSC.State == UILockedState.isLocked)
        {
            ReturnToSpawner();
            return true;
        }

        BulletInsMode = BulletInsMode.EditB;
        if (curSlotSC.MainID == -1)
        {
            CurSlot = curSlotSC;
            SlotManager.ClearBagSlotByID(SlotID, SlotType.CurBulletSlot);
            CurSlot.SOnDrop(gameObject);
        }
        else
        {
            var orIns = curSlotSC.ChildIns;
            CurSlot.SOnDrop(orIns);
            curSlotSC.SOnDrop(gameObject);
        }
        MainRoleManager.Instance.RefreshAllItems();
        return true;
    }
    
    bool HandleBulletSlot(RaycastResult result)
    {
        if (!result.gameObject.CompareTag("BulletSlot")) return false;
        ReturnToSpawner();
        return true;
    }

    bool HandleBulletInnerSlot(RaycastResult result)
    {
        if (result.gameObject.CompareTag("BulletInnerSlot"))
        {
            //1) 查一下CurBullets,看看这个槽位下有无子弹,如果有，回退子弹
            BulletInnerSlot curSlotSC = result.gameObject.GetComponent<BulletInnerSlot>();
            GameObject oldBullet = MainRoleManager.Instance.GetReadyBulletBySlotID(curSlotSC.SlotID);
            if (oldBullet != null)
                MainRoleManager.Instance.SubBullet(curSlotSC.SlotID);
            //2)添加当前子弹进入战场
            MainRoleManager.Instance.RefreshCurBullets(MutMode.Add, _data.ID,TargetSlotID: curSlotSC.SlotID);
            MainRoleManager.Instance.InstanceCurBullets();
            UIManager.Instance.RoleIns.GetComponent<RoleInner>().InitData();
            //3)
            Destroy(gameObject);
            return true;
        }
        return false;
    }
    
    void ReturnToSpawner()
    {
        DraggableBulletSpawner[] allSpawner = UIManager.Instance
            .G_BulletSpawnerSlot.GetComponentsInChildren<DraggableBulletSpawner>();
        
        foreach (var spawner in allSpawner.Where(spawner => spawner.ID == ID))
        {
            MainRoleManager.Instance.SubBullet(_data.ID, InstanceID);
            Destroy(gameObject);
            return;
        }
    }
    
    void ResetPosition()
    {
        BulletInsMode = BulletInsMode.EditB;
        gameObject.transform.position = originalPosition;
        _dragIns.transform.SetParent(originalParent, true);
    }
    #endregion
}