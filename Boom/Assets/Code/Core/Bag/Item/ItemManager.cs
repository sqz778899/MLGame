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
        GameObject curItemIns = ResManager.instance.CreatInstance(PathConfig.ItemPB);
        curItemIns.transform.SetParent(UIManager.Instance.ItemRoot.transform,false);
        Sprite curItemSprite = ResManager.instance.GetAssetCache<Sprite>(curItem.resAllPath);
        curItemIns.GetComponent<Image>().sprite = curItemSprite;
        ItemBase curSC = curItemIns.GetComponent<ItemBase>();
        curSC.CurItem = curItem;
        curSC.InstanceID = curItemIns.GetInstanceID();
        
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
        curTargetSlot.InstanceID = curSC.InstanceID;
        curSC.CurItemSlotType = SlotType.BagSlot;
        //清除旧的Slot信息
        curSC.SlotID = curTargetSlot.SlotID;
        //数据层同步
        curSC.SetItemData(curTargetSlot);
        MainRoleManager.Instance.BagItems.Add(curSC.CurItem);
        //MainRoleManager.Instance.RefreshCurItems();
        //curTargetSlot
    }

    public static void InitSaveFileItem(Item CurItem)
    {
        //实例化GO
        GameObject curItemIns = ResManager.instance.CreatInstance(PathConfig.ItemPB);
        curItemIns.transform.SetParent(UIManager.Instance.ItemRoot.transform,false);
        Sprite curItemSprite = ResManager.instance.GetAssetCache<Sprite>(CurItem.resAllPath);
        curItemIns.GetComponent<Image>().sprite = curItemSprite;
        ItemBase curSC = curItemIns.GetComponent<ItemBase>();
        curSC.CurItem = CurItem;
        curSC.InstanceID = curItemIns.GetInstanceID();
        
        //找到这个Item对应的Slot槽位
        SlotType curSlotType = (SlotType)CurItem.slotType;
        SlotBase curSlot = GetBagSlotByID(CurItem.slotID,curSlotType);
        
        //同步新的Slot信息
        curItemIns.transform.position = curSlot.transform.position;
        curSlot.MainID = CurItem.ID;
        curSC.SlotID = curSlot.SlotID;
        curSlot.InstanceID = curSC.InstanceID;
        curSC.CurItemSlotType = curSlotType;
        
        //数据层同步到RoleManager，作为总管理
        switch (curSlotType)
        {
            case SlotType.BagSlot:
                MainRoleManager.Instance.BagItems.Add(curSC.CurItem);
                break;
            case SlotType.ElementSlot:
                MainRoleManager.Instance.CurItems.Add(curSC.CurItem);
                break;
        }
    }

    public static void DeleteItem(GameObject ItemIns)
    {
        GameObject.DestroyImmediate(ItemIns);
        MainRoleManager.Instance.RefreshAllItems();
        TrunkManager.Instance.SaveFile();
    }
}
