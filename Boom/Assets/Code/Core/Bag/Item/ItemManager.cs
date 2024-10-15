using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ItemManager
{
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
                allSlot = UIManager.Instance.BagRoot.GetComponentsInChildren<SlotBase>();
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
    public static void ClearBagSlotByID(int SlotID)
    {
        SlotBase curSlot = GetBagSlotByID(SlotID);
        curSlot.MainID = -1;
        curSlot.InstanceID = -1;
    }
    
    //初始化Item
    public static void InstanceItemByID(int ItemID)
    {
        ItemJson curItemJson = GetItemJsonByID(ItemID);
        //容错
        if (curItemJson == null)
        {
            Debug.LogError("ItemID 未找到");
            return;
        }
        //
        Item curItem = new Item(ItemID);
        //实例化GO
        GameObject curItemIns = null;
        ItemBase curItemSc = null;
        InitItemIns(curItem,PathConfig.ItemPB, ref curItemIns, ref curItemSc);
        
        //在背包找到一个位置，把GO放进去
        SlotBase[] allSlot = UIManager.Instance.BagRoot.GetComponentsInChildren<SlotBase>();
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
        MainRoleManager.Instance.BagItems.Add(curItemSc.CurItem);
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

    //读档Item
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
    }

    public static void DeleteItem(GameObject ItemIns)
    {
        GameObject.DestroyImmediate(ItemIns);
        MainRoleManager.Instance.RefreshAllItems();
        TrunkManager.Instance.SaveFile();
    }
    
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
}
