using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BagItemManager<T> where T:ItemBase
{
    #region 重要功能
    public static GameObject CreateTempObjectGO<TData>(TData curObjectData,bool isInner = false)where TData : ItemDataBase
    {
        //实例化宝石
        GameObject objectIns = null;
        T objectSC = null;
        InitObjectInsTemp(curObjectData, ref objectIns,ref objectSC,isInner);
        return objectIns;
    }
    
    //添加物品到背包
    public static void AddObjectGO<TData>(TData curObjectData)where TData : ItemDataBase
    {
        //实例化宝石
        GameObject objectIns = null;
        T objectSC = null;
        InitObjectIns(curObjectData, ref objectIns,ref objectSC);
    }
    
    // 删除物品或宝石
    public static void DeleteObject(GameObject objectIns)
    {
        ItemBase curSC = objectIns.GetComponent<ItemBase>();
        if (curSC is Gem curGem)
        {
            MainRoleManager.Instance.SubGem(curGem._data);
            SlotManager.ClearSlot(curGem._data.CurSlot);
            GameObject.DestroyImmediate(objectIns);
        }
        
        if (curSC is Item curItem)
        {
            MainRoleManager.Instance.SubItem(curItem._data);
            SlotManager.ClearSlot(curItem._data.CurSlot);
            GameObject.DestroyImmediate(objectIns);
        }
    }
    
    // 读档并实例化物品或宝石
    public static void InitSaveFileObject<TData>(
        TData curObjectData,SlotType slotType)
        where TData : ItemDataBase
    {
        GameObject curObjectIns = null;
        T curObjectSC = null;
        InitObjectIns(curObjectData, ref curObjectIns, ref curObjectSC);

        // 同步到 MainRoleManager
        switch (slotType)
        {
            case SlotType.BagSlot:
                MainRoleManager.Instance.BagItems.Add(curObjectData as ItemData);
                break;
            case SlotType.GemBagSlot:
                MainRoleManager.Instance.BagGems.Add(curObjectData as GemData);
                break;
            case SlotType.GemInlaySlot:
                MainRoleManager.Instance.InLayGems.Add(curObjectData as GemData);
                break;
            case SlotType.ElementSlot:
                MainRoleManager.Instance.EquipItems.Add(curObjectData as ItemData);
                break;
        }
    }
    #endregion
    
    #region 私有方法
    // 私有方法：实例化对象
    static void InitObjectIns<TData>(TData curObjectData, ref GameObject objectIns,
        ref T objectSC) where TData : ItemDataBase
    {
        SlotType curSlotType = curObjectData.CurSlot.SlotType;
        string assetPath = curSlotType == SlotType.GemBagSlot || curSlotType == SlotType.GemInlaySlot
            ? PathConfig.GemTemplate
            : PathConfig.ItemPB;

        if (curObjectData.CurSlot is GemSlotInner)
            assetPath = PathConfig.GemInnerTemplate;
        
        objectIns = ResManager.instance.CreatInstance(assetPath);
        objectIns.transform.SetParent(UIManager.Instance.BagItemRoot.transform, false);
        objectSC = objectIns.GetComponent<T>();
        objectSC.BindData(curObjectData);
        curObjectData.CurSlot.SOnDrop(objectIns);
    }
    
    static void InitObjectInsTemp<TData>(TData curObjectData, ref GameObject objectIns,
        ref T objectSC,bool isInner = false) where TData : ItemDataBase
    {
        SlotType curSlotType = curObjectData.CurSlot.SlotType;
        string assetPath = curSlotType == SlotType.GemBagSlot || curSlotType == SlotType.GemInlaySlot
            ? PathConfig.GemTemplate
            : PathConfig.ItemPB;
        
        if (isInner)
            assetPath = curSlotType == SlotType.GemBagSlot || curSlotType == SlotType.GemInlaySlot
                ? PathConfig.GemInnerTemplate
                : PathConfig.ItemPB;
        
        objectIns = ResManager.instance.CreatInstance(assetPath);
        objectSC = objectIns.GetComponent<T>();
        objectSC.BindData(curObjectData);
    }
    #endregion
}
