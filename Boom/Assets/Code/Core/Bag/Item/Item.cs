﻿using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item : ItemBase,IItemInteractionBehaviour
{
    public ItemData Data { get; private set; }
   
    [Header("UI表现")]
    public Image Icon;
    public Image ItemBGInBag;
    public TextMeshProUGUI StackCountText;
    public Color RareColor;
    
    public Color Rare1;
    public Color Rare2;
    public Color Rare3;
    public Color Rare4;
    RectTransform rectTransform;

    void Awake() => rectTransform = GetComponent<RectTransform>();

    #region 数据交互相关
    public override void BindData(ItemDataBase data)
    {
        Data = data as ItemData;
        Data.InstanceID = GetInstanceID();
        Data.OnDataChanged += RefreshUI;
        RefreshUI();
    }

    void RefreshUI()
    {
        Icon.sprite = ResManager.instance.GetItemIcon(Data.ID);
        gameObject.name = Data.Name + Data.InstanceID;
        //同步背景形状
        SyncBackground();
        //同步稀有度颜色
        switch (Data.Rarity)
        {
            case DropedRarity.Common:
                RareColor = Rare1;
                break;
            case DropedRarity.Rare:
                RareColor = Rare2;
                break;
            case DropedRarity.Epic:
                RareColor = Rare3;
                break;
            case DropedRarity.Legendary:
                RareColor = Rare4;
                break;
        }
        ItemBGInBag.color = RareColor;
        
        // 堆叠数量显示
        if (Data.IsStackable && Data.StackCount > 1)
        {
            StackCountText.text = $"x{Data.StackCount}";
            StackCountText.gameObject.SetActive(true);
        }
        else
            StackCountText.gameObject.SetActive(false);
    }

    void OnDestroy() => Data.OnDataChanged -= RefreshUI;
    #endregion

    #region 双击与右键逻辑
    public void OnBeginDrag() => HideBackground();
    public void OnEndDrag() {}
    public void OnClick() {}
    public bool CanDrag => true;
    void HideBackground() =>  ItemBGInBag.gameObject.SetActive(false);
    public void SyncBackground() => ItemBGInBag.gameObject.SetActive(Data.CurSlotController.SlotType == SlotType.ItemBagSlot);

    void IItemInteractionBehaviour.OnDoubleClick(){}
    void IItemInteractionBehaviour.OnRightClick()
    {
        if (Data.CurSlotController.SlotType == SlotType.ItemBagSlot)
            RightClickMenuManager.Instance.Show(
                gameObject, UTools.GetWPosByMouse(rectTransform));
    }
    #endregion
}