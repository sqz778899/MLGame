using UnityEngine;
using UnityEngine.UI;

public class ItemNew : ItemBase,IItemInteractionBehaviour
{
    public ItemData Data { get; private set; }
   
    [Header("UI表现")]
    public Image Icon;
    RectTransform rectTransform;

    void Awake() => rectTransform = GetComponent<RectTransform>();

    public void BindData(ItemDataBase data)
    {
        Data = data as ItemData;
        RefreshUI();
    }

    void RefreshUI()
    {
        Icon.sprite = ResManager.instance.GetAssetCache<Sprite>(
            PathConfig.GetItemPath(Data.ImageName));
        // TODO: 设置稀有度边框颜色等
    }

    #region 双击与右键逻辑
    void IItemInteractionBehaviour.OnDoubleClick()
    {
        ItemSlotController from = Data.CurSlotController as ItemSlotController;
        var toSlot = (from.SlotType == SlotType.GemInlaySlot)
            ? SlotManager.GetEmptySlotController(SlotType.BagItemSlot)
            : SlotManager.GetEmptySlotController(SlotType.BagEquipSlot);
        
        toSlot.Assign(Data, gameObject);
    }

    void IItemInteractionBehaviour.OnRightClick() =>
        RightClickMenuManager.Instance.Show(gameObject, UTools.GetWPosByMouse(rectTransform));
    #endregion
}