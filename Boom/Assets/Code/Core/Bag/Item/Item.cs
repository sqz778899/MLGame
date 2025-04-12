using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : ItemBase,IItemInteractionBehaviour
{
    public ItemData _data;
    
    [Header("重要属性")]
    public int Rare;
    
    [Header("表现相关")]
    public Image ItemSprite;
    public Image ItemBGInBag;
    public Image ItemBGInEquip;
    public Color RareColor;
    
    public Color Rare1;
    public Color Rare2;
    public Color Rare3;
    public Color Rare4;
    
    #region UI交互逻辑
    bool IsBGInBag;
    bool IsBGInEquip;
    internal void VOnDrag()
    {
        //在拖动中把Item显示后面的背景图关掉
        IsBGInBag = ItemBGInBag.gameObject.activeSelf;
        IsBGInEquip = ItemBGInEquip.gameObject.activeSelf;
        ItemBGInBag.gameObject.SetActive(false);
        ItemBGInEquip.gameObject.SetActive(false);
    }
    
    
    #endregion

    #region 同步各种数据
    public void OnDataChangeItem()
    {
        //同步数据
        ItemJson itemDesignData = TrunkManager.Instance.GetItemJson(_data.ID);
        Rare = itemDesignData.Rare;
        ItemSprite.sprite = ResManager.instance.GetAssetCache<Sprite>(
            PathConfig.GetItemPath(itemDesignData.ResName));
        gameObject.name = itemDesignData.Name + _data.InstanceID;
        //同步背景形状
        switch (_data.CurSlotController.SlotType)
        {
            case SlotType.BagItemSlot:
                ItemBGInBag.gameObject.SetActive(true);
                ItemBGInEquip.gameObject.SetActive(false);
                break;
            case SlotType.BagEquipSlot:
                ItemBGInEquip.gameObject.SetActive(true);
                ItemBGInBag.gameObject.SetActive(false);
                break;
        }

        //同步稀有度颜色
        switch (Rare)
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

        if (ItemBGInBag != null) ItemBGInBag.color = RareColor;
        if (ItemBGInEquip != null) ItemBGInEquip.color = RareColor;
    }
    
    public override void BindData(ItemDataBase data)
    {
        if (_data != null)
            _data.OnDataChanged -= OnDataChangeItem; // 先退订旧Data的事件
        
        _data = data as ItemData;
        if (_data != null)
        {
            data.InstanceID = GetInstanceID();
            _data.OnDataChanged += OnDataChangeItem;
            OnDataChangeItem(); // 立即刷新一遍
        }
    }
    
    void OnDestroy() => _data.OnDataChanged -= OnDataChangeItem;
    #endregion

    public void OnDoubleClick()
    {
        throw new System.NotImplementedException();
    }

    public void OnRightClick()
    {
        throw new System.NotImplementedException();
    }
}
