using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ItemManager
{
    #region 重要功能
    //实例化一个Item
    public static void AddItem(int ItemID)
    {
        ItemJson curItemJson = GetItemJsonByID(ItemID);
        //容错
        if (curItemJson == null)
        {
            Debug.LogError("ItemID 未找到");
            return;
        }
        //实例化GO
        Item curItem = new Item(ItemID);
        GameObject curItemIns = null;
        ItemBase curItemSc = null;
        InitItemIns(curItem,PathConfig.ItemPB, ref curItemIns, ref curItemSc);
        
        //在背包找到一个位置，把GO放进去
        SlotBase[] allSlot = UIManager.Instance.G_Bag.GetComponentsInChildren<SlotBase>();
        SlotBase curTargetSlot = null;
        foreach (var each in allSlot)
        {
            if (each.MainID == -1)
            {
                curTargetSlot = each;
                break;
            }
        }
        
        //同步新的Slot信息
        curItemIns.transform.position = curTargetSlot.transform.position;
        curTargetSlot.MainID = ItemID;
        curTargetSlot.InstanceID = curItemSc.InstanceID;
        curItemSc.CurItemSlotType = SlotType.BagSlot;
        //清除旧的Slot信息
        curItemSc.SlotID = curTargetSlot.SlotID;
        //数据层同步
        curItemSc.SetItemData(curTargetSlot);
    }
    //删除一个Item
    public static void DeleteItem(GameObject ItemIns)
    {
        GameObject.DestroyImmediate(ItemIns);
        MainRoleManager.Instance.RefreshAllItems();
        TrunkManager.Instance.SaveFile();
    }
    //读档并在游戏内实例化Item
    public static void InitSaveFileItem(Item CurItem)
    {
        //实例化GO
        GameObject curItemIns = null;
        ItemBase curItemSc = null;
        InitItemIns(CurItem,PathConfig.ItemPB, ref curItemIns, ref curItemSc);
        
        //找到这个Item对应的Slot槽位
        SlotType curSlotType = (SlotType)CurItem.slotType;
        SlotBase curSlot = GetBagSlotByID(CurItem.slotID,curSlotType);
        
        //同步新的Slot信息
        curItemIns.transform.position = curSlot.transform.position;
        curSlot.MainID = CurItem.ID;
        curItemSc.SlotID = curSlot.SlotID;
        curSlot.InstanceID = curItemSc.InstanceID;
        curItemSc.CurItemSlotType = curSlotType;
        //数据层同步到RoleManager，作为总管理
        switch (curSlotType)
        {
            case SlotType.BagSlot:
                MainRoleManager.Instance.BagItems.Add(curItemSc.CurItem);
                break;
            case SlotType.ElementSlot:
                MainRoleManager.Instance.CurItems.Add(curItemSc.CurItem);
                break;
        }
        curItemSc.SetItemBG();
        curItemSc.SetRareColor();
    }
    #endregion

    #region 次要功能
    //清空背包内Slot
    public static void ClearBagSlotByID(int SlotID)
    {
        SlotBase curSlot = GetBagSlotByID(SlotID);
        curSlot.MainID = -1;
        curSlot.InstanceID = -1;
    }
    //初始化Item缩略图
    public static GameObject InstanceItemThumbnailByID(Item CurItem)
    {
        //实例化GO
        GameObject curItemIns = null;
        ItemBase curItemSc = null;
        InitItemIns(CurItem, PathConfig.ItemThumbnailPB,ref curItemIns, ref curItemSc);
        return curItemIns;
    }
    #endregion

    #region 私有方法
    static void InitItemIns(Item CurItem,string assetPath,ref GameObject itemIns,ref ItemBase itemSc)
    {
        //实例化GO
        itemIns = ResManager.instance.CreatInstance(assetPath);
        itemIns.transform.SetParent(UIManager.Instance.ItemRoot.transform,false);
        Sprite curItemSprite = ResManager.instance.GetAssetCache<Sprite>(CurItem.resAllPath);
        itemSc = itemIns.GetComponent<ItemBase>();
        itemSc.ItemSprite.sprite = curItemSprite;
        itemSc.CurItem = CurItem;
        itemSc.InstanceID = itemIns.GetInstanceID();
    }
    
    static SlotBase GetBagSlotByID(int SlotID,SlotType slotType = SlotType.BagSlot)
    {
        SlotBase curTargetSlot = null;
        SlotBase[] allSlot;
        switch (slotType)
        {
            case SlotType.ElementSlot:
                allSlot = UIManager.Instance.ElementSlotRoot.GetComponentsInChildren<SlotBase>();
                break;
            default:
                allSlot = UIManager.Instance.G_Bag.GetComponentsInChildren<SlotBase>();
                break;
        }
        foreach (var each in allSlot)
        {
            if (each.SlotID == SlotID)
            {
                curTargetSlot = each;
                break;
            }
        }
        return curTargetSlot;
    }

    static ItemJson GetItemJsonByID(int ItemID)
    {
        ItemJson curItemJson = null;
        List<ItemJson> itemDesignJsons = TrunkManager.Instance.ItemDesignJsons;
        foreach (var each in itemDesignJsons)
        {
            if (each.ID == ItemID)
            {
                curItemJson = each;
                break;
            }
        }

        return curItemJson;
    }
    #endregion
}
