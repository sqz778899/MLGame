using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BagItemTools<T> where T:ItemBase
{
    #region 重要功能
    public static GameObject  CreateTempObjectGO<TData>(TData curObjectData,CreateItemType insType)where TData : ItemDataBase
    {
        //实例化宝石
        GameObject objectIns = null;
        T objectSC = null;
        string assetpath = "";
        switch (insType)
        {
            case CreateItemType.ShopGem:
                assetpath = PathConfig.RollGemPB;
                break;
            case CreateItemType.TempGem://临时既创建销毁的，比如开宝箱的表现用
                assetpath = PathConfig.GemTemplate;
                break;
            case CreateItemType.MiniBagGem:
                assetpath = PathConfig.GemInnerTemplate;
                break;
        }
        
        objectIns = ResManager.instance.CreatInstance(assetpath);
        objectSC = objectIns.GetComponent<T>();
        objectSC.BindData(curObjectData);
        objectIns.GetComponent<ItemInteractionHandler>()?.BindData(curObjectData);//影子模型没这个组件
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
        if (curSC is GemNew curGem)
            curGem.Data.CurSlotController.Unassign();
        
        if (curSC is Item curItem)
        {
            InventoryManager.Instance._InventoryData.RemoveItem(curItem._data);
            SlotManager.ClearSlot(curItem._data.CurSlot);
            GameObject.Destroy(objectIns);
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
    }
    #endregion
    
    public static void ClearAllObject()
    {
        SlotBase[] allSlot = SlotManager.GetAllSlotBase();
        foreach (SlotBase each in allSlot)
        {
            if (each.ChildIns == null) continue;
            GameObject.Destroy(each.ChildIns);
            SlotManager.ClearSlot(each, true);
        }
    }
    
    #region 私有方法
    // 私有方法：实例化对象
    static void InitObjectIns<TData>(TData curObjectData, ref GameObject objectIns,
        ref T objectSC) where TData : ItemDataBase
    {
        SlotType curSlotType = curObjectData.CurSlotController.SlotType;
        string assetPath = curSlotType == SlotType.GemBagSlot || curSlotType == SlotType.GemInlaySlot
            ? PathConfig.GemTemplate
            : PathConfig.ItemPB;
        
        objectIns = ResManager.instance.CreatInstance(assetPath);
        objectIns.transform.SetParent(UIManager.Instance.BagUI.ItemRoot.transform, false);
        objectSC = objectIns.GetComponent<T>();
        objectSC.BindData(curObjectData);
        objectIns.GetComponent<ItemInteractionHandler>().BindData(curObjectData);
        curObjectData.CurSlotController.Assign(curObjectData,objectIns);
    }
    #endregion
}
