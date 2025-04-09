using System;
using UnityEngine;
using UnityEngine.UI;

public class GemNew: ItemBase,IItemInteractionBehaviour
{
    public GemData Data { get; private set; }
    [Header("UI表现")]
    public Image Icon;
    RectTransform rectTransform;

    void Awake() => rectTransform = GetComponent<RectTransform>();
    
    
    #region 数据交互相关
    public override void BindData(ItemDataBase data)
    {
        Data = data as GemData;
        PlayerManager.Instance.OnTalentLearned -= Data.AddTalentGemBonus;
        PlayerManager.Instance.OnTalentLearned += Data.AddTalentGemBonus;
        RefreshUI();
    }

    void RefreshUI()
    {
        Icon.sprite = ResManager.instance.GetAssetCache<Sprite>(
            PathConfig.GetGemPath(Data.ImageName));
        // TODO: 设置稀有度边框颜色等
    }

    void OnDestroy() => PlayerManager.Instance.OnTalentLearned -= Data.AddTalentGemBonus;
    #endregion

    #region 双击与右键逻辑
    void IItemInteractionBehaviour.OnDoubleClick()
    {
        SlotController from = Data.CurSlotController as SlotController;
        var toSlot = (from.SlotType == SlotType.GemInlaySlot)
            ? SlotManager.GetEmptySlotController(SlotType.GemBagSlot)
            : SlotManager.GetEmptySlotController(SlotType.GemInlaySlot);
        
        toSlot.Assign(Data, gameObject);
    }

    void IItemInteractionBehaviour.OnRightClick() =>
        RightClickMenuManager.Instance.Show(gameObject, UTools.GetWPosByMouse(rectTransform));
    #endregion
}