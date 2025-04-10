﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableBullet : Bullet
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
            /*if (IsSpawnerCreate) InventoryManager.Instance.ReturnToSpawner(gameObject);
            else ResetPosition();*/
            ResetPosition();
        }
    }
    
    public override void OnPointerUp(PointerEventData eventData)
    {
        DropOneBullet(eventData);
        HideTooltips();
    }

    #region 私有方法
    bool HandleBulletSlotRole(RaycastResult result)
    {
        /*if (!result.gameObject.CompareTag("BulletSlotRole")) return false;
        var targetSlotSC = result.gameObject.GetComponent<BulletSlotRole>();
        if (targetSlotSC.State == UILockedState.isLocked)
        {
            InventoryManager.Instance.ReturnToSpawner(gameObject);
            return true;
        }

        //落入装备槽位，如果是Spawner诞生的，创建新的Data绑定
        if (IsSpawnerCreate)
        {
           // BindData(new BulletData(_data.ID,_data.CurSlot));
            IsSpawnerCreate = false;
        }
     
        BulletInsMode = BulletInsMode.EditB;
        if (targetSlotSC.MainID == -1)
        {
            SlotManager.ClearSlot(_data.CurSlotController);
            targetSlotSC.SOnDrop(gameObject);
            //这个时候槽位ID才确定下来
            InventoryManager.Instance._BulletInvData.EquipBullet(_data);
        }
        else//交换
        {
            GameObject orIns = targetSlotSC.ChildIns;
            SlotBase oldSlot = _data.CurSlot;
            targetSlotSC.SOnDrop(gameObject);
            oldSlot.SOnDrop(orIns);
            InventoryManager.Instance._BulletInvData.RefreshModifiers();
            InventoryManager.Instance._BulletInvData.SortEquipBullet();
        }*/
        return true;
    }
     
    bool HandleBulletSlot(RaycastResult result)
    {
        if (!result.gameObject.CompareTag("BulletSlot")) return false;
        //InventoryManager.Instance.ReturnToSpawner(gameObject);
        return true;
    }

    bool HandleBulletInnerSlot(RaycastResult result)
    {
        if (result.gameObject.CompareTag("BulletInnerSlot"))
        {
            BulletInnerSlot curSlotSC = result.gameObject.GetComponent<BulletInnerSlot>();
            //如果子弹槽是锁定状态，则无法拖拽
            BulletSlotRole curRealSlot = curSlotSC.CurBulletSlotRole;
            if (curRealSlot.State == UILockedState.isLocked)
                return false;
            
            //1) 查一下CurBullets,看看这个槽位下有无子弹,如果有，回退子弹
            InventoryManager.Instance._BulletInvData.UnEquipBullet(
                curSlotSC.CurBulletSlotRole.CurBulletData);//回退老子弹
            //2)添加新子弹，到数据层
            curSlotSC.CurBulletSlotRole.SOnDrop(_data);
            InventoryManager.Instance._BulletInvData.EquipBullet(_data);//添加新子弹
            //3)刷新GO
            //PlayerManager.Instance.RoleInFightGO.GetComponent<RoleInner>().CreateBulletInner(true);
            Destroy(gameObject);
            return true;
        }
        return false;
    }
    
    void ResetPosition()
    {
        BulletInsMode = BulletInsMode.EditB;
        gameObject.transform.position = originalPosition;
        gameObject.transform.SetParent(originalParent, true);
    }
    #endregion
}