using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BagItemManager<T> where T:ItemBase
{
    #region 重要功能
    //添加物品
    public static void AddObjectGO(int objectID,ref T objectSC,Transform parentTransform,SlotType slotType)
    {
        //实例化宝石
        GameObject objectIns = null;
        InitObjectIns(objectID, ref objectIns,ref objectSC,slotType);
        //放进背包
        SlotBase[] allSlot = parentTransform.GetComponentsInChildren<SlotBase>();
        SlotBase curTargetSlot = null;
        foreach (var each in allSlot)
        {
            if (each.MainID == -1)
            {
                curTargetSlot = each;
                break;
            }
        }

        if (curTargetSlot == null)
        {
            Debug.LogError("未找到 Bag Slot!!!!!!!!!");
            return;
        }
        
        // 绑定到槽位
        objectIns.transform.SetParent(curTargetSlot.transform,false);
        curTargetSlot.MainID = objectID;
        objectSC.SlotType = slotType;
        objectSC.SlotID = curTargetSlot.SlotID;
        objectSC.CurSlot = curTargetSlot;
        //objectSC
    }
    
    // 删除物品或宝石
    public static void DeleteObject(GameObject objectIns)
    {
        SlotBase curSlot = objectIns.GetComponent<ItemBase>().CurSlot;
        curSlot.MainID = -1;
        GameObject.DestroyImmediate(objectIns);
        MainRoleManager.Instance.RefreshAllItems();
        TrunkManager.Instance.SaveFile();
    }
    
    // 读档并实例化物品或宝石
    public static void InitSaveFileObject<TJson>(
        TJson curObjectJson,SlotType slotType)
        where TJson : ItemJsonBase
    {
        GameObject curObjectIns = null;
        T curObjectSC = null;
        InitObjectIns(curObjectJson.ID, ref curObjectIns, ref curObjectSC, slotType);
        
        // 找到对应的Slot槽位
        SlotBase curSlot = SlotManager.GetBagSlotByID(curObjectJson.SlotID, slotType);
        if (curSlot == null) return;
        curSlot.SOnDrop(curObjectIns);

        // 同步到 MainRoleManager
        curObjectJson.InstanceID = curObjectSC.InstanceID;
        switch (slotType)
        {
            case SlotType.BagSlot:
                MainRoleManager.Instance.BagItems.Add(curObjectJson as ItemJson);
                break;
            case SlotType.GemBagSlot:
                MainRoleManager.Instance.BagGems.Add(curObjectJson as GemJson);
                break;
            case SlotType.GemInlaySlot:
                MainRoleManager.Instance.InLayGems.Add(curObjectJson as GemJson);
                break;
            case SlotType.ElementSlot:
                MainRoleManager.Instance.EquipItems.Add(curObjectJson as ItemJson);
                break;
        }
    }
    #endregion
    
    #region 私有方法
    // 私有方法：实例化对象
    static void InitObjectIns(int objectID, ref GameObject objectIns, ref T objectSC, SlotType slotType)
    {
        string assetPath = slotType == SlotType.GemBagSlot || slotType == SlotType.GemInlaySlot
            ? PathConfig.GemTemplate
            : PathConfig.ItemPB;

        objectIns = ResManager.instance.CreatInstance(assetPath);
        objectIns.transform.SetParent(UIManager.Instance.BagItemRootGO.transform, false);
        objectSC = objectIns.GetComponent<T>();
        objectSC.ID = objectID;
        objectSC.InstanceID = objectIns.GetInstanceID();
        objectSC.SyncData();
    }
    #endregion
}
