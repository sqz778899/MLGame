using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBase : DragBase
{
    [Header("表现相关")]
    public Image ItemSprite;
    public Image ItemBGInBag;
    public Image ItemBGInElement;
    public Color RareColor;
    
    public Color Rare1;
    public Color Rare2;
    public Color Rare3;
    public Color Rare4;
    
    [Header("重要属性")]
    public Item CurItem;
    public int InstanceID;
    public int SlotID; //物品此刻的Slot位置，如果有移动的话，需要清除之前的Slot信息
    public SlotType CurItemSlotType;
    
    public void SetItemData()
    {
        CurItem.slotID = _curSlot.SlotID;
        CurItem.slotType = (int)_curSlot.SlotType;
        SetItemBG();
        SetRareColor();
    }
    
    public void SetItemData(SlotBase slot)
    {
        CurItem.slotID = slot.SlotID;
        CurItem.slotType = (int)slot.SlotType;
        SetItemBG();
    }

    public void SetRareColor()
    {
        switch (CurItem.rare)
        {
            case 1:
                RareColor = Rare1;
                break;
            case 2:
                RareColor = Rare2;
                break;
            case 3:
                RareColor = Rare3;
                break;
            case 4:
                RareColor = Rare4;
                break;
        }
        
        ItemBGInBag.color = RareColor;
        ItemBGInElement.color = RareColor;
    }

    public void ClocsBG()
    {
        ItemBGInBag.gameObject.SetActive(false);
        ItemBGInElement.gameObject.SetActive(false);
    }
    
    public void SetItemBG()
    {
       switch(CurItemSlotType)
       {
           case SlotType.BagSlot:
               ItemBGInBag.gameObject.SetActive(true);
               ItemBGInElement.gameObject.SetActive(false);
               break;
           case SlotType.ElementSlot:
               ItemBGInElement.gameObject.SetActive(true);
               ItemBGInBag.gameObject.SetActive(false);
               break;
       }
    }
}
