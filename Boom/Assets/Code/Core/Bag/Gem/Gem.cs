using UnityEngine;
using UnityEngine.UI;

public class Gem : DragBase
{
    public GemData _data;
    
    public override void BindData(ItemDataBase data)
    {
        if (_data != null)
            _data.OnDataChanged -= OnDataChangeGem; // 先退订旧Data的事件
        
        _data = data as GemData;
        if (_data != null)
        {
            _data.OnDataChanged += OnDataChangeGem;
            OnDataChangeGem(); // 立即刷新一遍
        }
    }
    
    [Header("资产")]
    public Image ItemSprite;

    #region UI交互逻辑
    //落下空槽逻辑
    public override void OnDropEmptySlot(SlotBase targetSlot)
    {
        GemSlot slot = targetSlot as GemSlot;
        SlotManager.ClearSlot(_data.CurSlot);
        switch (slot.SlotType)
        {
            case SlotType.GemBagSlot:
                slot.SOnDrop(gameObject);
                ToolTipsOffset = new Vector3(1.01f, -0.5f, 0);
                break;
            case SlotType.GemInlaySlot:
                slot.SOnDrop(gameObject);
                ToolTipsOffset = new Vector3(-0.92f, -0.52f, 0);
                break;
            default:
                break;
        }
    }
    
    //落下交换逻辑
    internal override void OnDropFillSlot(SlotBase targetSlot)
    {
        //先把目标槽位的物品拿出来
        GameObject tagetChildIns = targetSlot.ChildIns;
        _data.CurSlot.SOnDrop(tagetChildIns);
        //再把自己放进去
        targetSlot.SOnDrop(gameObject);
    }
    //双击逻辑
    internal override void DoubleClick()
    {
        SlotBase curEmptySlot = null;
        if (_data.CurSlot.SlotType == SlotType.GemBagSlot)
            curEmptySlot = SlotManager.GetEmptySlot(SlotType.GemInlaySlot);//在角色栏找到一个空的GemSlot
        else if(_data.CurSlot.SlotType == SlotType.GemInlaySlot)
            curEmptySlot = SlotManager.GetEmptySlot(SlotType.GemBagSlot);//在角色栏找到一个空的GemSlot
        
        if (!curEmptySlot) return;
        
        SlotManager.ClearSlot(_data.CurSlot);//清除旧的Slot信息
        curEmptySlot.SOnDrop(gameObject);
    }
    //右击逻辑
    internal override void RightClick()
    {
        if (_data.CurSlot.SlotType == SlotType.GemBagSlot)
            DisplayRightClickMenu(_eventData);
        else if (_data.CurSlot.SlotType == SlotType.GemInlaySlot)//如果是镶嵌的宝石.右键直接卸下
            DoubleClick();
    }
    #endregion

    #region ToolTips相关
    internal override void SetTooltipInfo()
    {
        ToolTipsInfo curToolTipsInfo = new ToolTipsInfo(_data.Name,_data.Level);
        if (_data.Damage != 0)
        {
            ToolTipsAttriSingleInfo curInfo = new ToolTipsAttriSingleInfo(
                ToolTipsAttriType.Damage,_data.Damage);
            curToolTipsInfo.AttriInfos.Add(curInfo);
        }
        if (_data.Piercing != 0)
        {
            ToolTipsAttriSingleInfo curInfo = new ToolTipsAttriSingleInfo(
                ToolTipsAttriType.Piercing, _data.Piercing);
            curToolTipsInfo.AttriInfos.Add(curInfo);
        }
        if (_data.Resonance != 0)
        {
            ToolTipsAttriSingleInfo curInfo = new ToolTipsAttriSingleInfo(
                ToolTipsAttriType.Resonance, _data.Resonance);
            curToolTipsInfo.AttriInfos.Add(curInfo);
        }
        CurTooltipsSC.SetInfo(curToolTipsInfo);
    }
    #endregion
    
    #region 同步各种数据
    //数据同步
    public void OnDataChangeGem()
    {
        _data.InstanceID = gameObject.GetInstanceID();
        GemJson gemDesignData = TrunkManager.Instance.GetGemJson(_data.ID);
        ItemSprite.sprite = ResManager.instance.GetAssetCache<Sprite>(
            PathConfig.GetGemPath(gemDesignData.ImageName));
    }
    #endregion
}
