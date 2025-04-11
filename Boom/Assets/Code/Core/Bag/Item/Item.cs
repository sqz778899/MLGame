using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : DragBase
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
    internal override void VOnDrag()
    {
        //在拖动中把Item显示后面的背景图关掉
        IsBGInBag = ItemBGInBag.gameObject.activeSelf;
        IsBGInEquip = ItemBGInEquip.gameObject.activeSelf;
        ItemBGInBag.gameObject.SetActive(false);
        ItemBGInEquip.gameObject.SetActive(false);
    }
    
    public override void OnPointerUp(PointerEventData eventData)
    {
        UIManager.Instance.IsLockedClick = false;
        if (eventData.button == PointerEventData.InputButton.Right)
            return;
        HideTooltips();
        
        // 在释放鼠标按钮时，我们检查这个位置下是否有一个Slot
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        bool NonHappen = true; // 发生Slot drop down 逻辑
        
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.TryGetComponent(out SlotBase curSlotSC))
            {
                //1）前置判断，如果Slot是GemSlot类型，才能继续
                ItemSlot curItemSlot = curSlotSC as ItemSlot;
                if (curItemSlot == null) continue;
                if (curItemSlot.CurItemData == _data) break;//如果是同一个宝石，不做任何操作
                
                //2）空槽逻辑
                if (curItemSlot.MainID == -1)
                {
                    OnDropEmptySlot(curItemSlot);
                    NonHappen = false;
                }
                else//3）满槽逻辑
                {
                    OnDropFillSlot(curItemSlot);
                    NonHappen = false;
                    break;
                }
            }
        }
        
        if (NonHappen)
            NonFindSlot();
    }
    
    //落下空槽逻辑
    public override void OnDropEmptySlot(SlotBase targetSlot)
    {
        if (EternalCavans.Instance.TutorialDragGemLock) return;
        ItemSlot slot = targetSlot as ItemSlot;
        SlotManager.ClearSlot(_data.CurSlotController);
        slot.SOnDrop(gameObject);
        if (slot.SlotType == SlotType.BagItemSlot)
        {
            ItemBGInBag.gameObject.SetActive(true);
        }
        if (slot.SlotType == SlotType.BagEquipSlot)
        {
            ItemBGInEquip.gameObject.SetActive(true);
        }
    }
    
    //落下交换逻辑
    internal override void OnDropFillSlot(SlotBase targetSlot)
    {
        /*//先把目标槽位的物品拿出来
        GameObject tagetChildIns = targetSlot.ChildIns;
        _data.CurSlot.SOnDrop(tagetChildIns);
        //再把自己放进去
        targetSlot.SOnDrop(gameObject);*/
    }
    
    internal override void NonFindSlot()
    {
        base.NonFindSlot();
        ItemBGInBag.gameObject.SetActive(IsBGInBag);
        ItemBGInEquip.gameObject.SetActive(IsBGInEquip);
    }
    #endregion
    
    #region ToolTips相关
    internal override void SetTooltipInfo()
    {
        TooltipsInfo curTooltipsInfo = new TooltipsInfo(_data.Name);
        CurTooltipsSC.SetInfo(curTooltipsInfo);
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
}
