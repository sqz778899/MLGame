using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BulletEditInner:ItemBase
{
    public BulletData _data; //绝对核心数据
    
    [Header("表现资产")]
    public Image Icon;
    public GameObject GroupStar;

    public void InitData()
    {
        for (int i = 0; i < GroupStar.transform.childCount; i++)
            GroupStar.transform.GetChild(i).gameObject.SetActive(i < _data.Level);
        Icon.sprite = ResManager.instance.GetAssetCache<Sprite>(
            PathConfig.GetBulletImageOrSpinePath(_data.ID, BulletInsMode.EditA));
    }
    
    public bool EndDragSimulation()
    {
        // 模拟PointerEventData
        PointerEventData simulatedEvent = new PointerEventData(EventSystem.current)
        { position = Input.mousePosition };
        return DropOneBullet(simulatedEvent);
    }

    bool DropOneBullet(PointerEventData eventData)
    {
        Debug.Log("Drop");
        
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        bool nonHappen = true; // 发生逻辑，如果未发生，则之后子弹弹回原位

        foreach (var result in results)
        {
            if (result.gameObject.CompareTag("BulletSlot"))
            {
                ReturnToSpawner();
                //3)刷新GO
                UIManager.Instance.RoleIns.GetComponent<RoleInner>().InitData();
                nonHappen = false;
                break;
            }

            if (result.gameObject.CompareTag("BulletInnerSlot"))
            {
                BulletInnerSlot targetSlotSC = result.gameObject.GetComponent<BulletInnerSlot>();
                if(targetSlotSC.CurBulletSlotRole.CurBulletData == _data) break;//如果是同一个子弹，不做任何操作
                if (targetSlotSC.CurBulletSlotRole.CurBulletData != null) //走交换逻辑
                {
                    GameObject targerChildIns = targetSlotSC.CurBulletSlotRole.ChildIns;
                    SlotManager.ClearSlot(_data.CurSlot);
                    _data.CurSlot.SOnDrop(targerChildIns);
                    targetSlotSC.CurBulletSlotRole.SOnDrop(_data);
                }
                else
                {
                    SlotManager.ClearSlot(_data.CurSlot);
                    targetSlotSC.CurBulletSlotRole.SOnDrop(_data);//走空插逻辑
                }
             
                //3)刷新GO
                UIManager.Instance.RoleIns.GetComponent<RoleInner>().CreateBulletInner();
                Destroy(gameObject);
                nonHappen = false;
                break;
            }
        }
        return nonHappen;
    }
    
    void ReturnToSpawner()
    {
        foreach (var eachBulletData in MainRoleManager.Instance.CurBulletSpawners)
        {
            if (eachBulletData.ID == _data.ID)
            {
                eachBulletData.SpawnerCount++;
                MainRoleManager.Instance.SubCurBullet(_data);
                Destroy(gameObject);
                SlotManager.ClearSlot(_data.CurSlot);
                break;
            }
        }
    }
}