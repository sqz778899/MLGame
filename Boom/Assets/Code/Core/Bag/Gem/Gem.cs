using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Gem : DragBase
{
    public GemData _data;
    [Header("资产")]
    public Image ItemSprite;
    
    #region UI交互逻辑
    //鼠标抬起
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
                GemSlot curGemSlot = curSlotSC as GemSlot;
                if (curGemSlot == null) continue;
                if (curGemSlot.CurGemData == _data) break;//如果是同一个宝石，不做任何操作
                
                //2）空槽逻辑
                if (curGemSlot.MainID == -1)
                {
                    OnDropEmptySlot(curGemSlot);
                    NonHappen = false;
                }
                else//3）满槽逻辑
                {
                    OnDropFillSlot(curGemSlot);
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
        GemSlot slot = targetSlot as GemSlot;
        SlotManager.ClearSlot(_data.CurSlot);
        slot.SOnDrop(gameObject);
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
        if (EternalCavans.Instance.TutorialDragGemLock) return;
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
        gameObject.name = gemDesignData.Name + _data.InstanceID;
        ItemSprite.sprite = ResManager.instance.GetAssetCache<Sprite>(
            PathConfig.GetGemPath(gemDesignData.ImageName));
    }
    
    public override void BindData(ItemDataBase data)
    {
        if (_data != null)
        {
            _data.OnDataChanged -= OnDataChangeGem; // 先退订旧Data的事件
            PlayerManager.Instance.OnTalentLearned -= _data.AddTalentGemBonus;
        }
        
        _data = data as GemData;
        if (_data != null)
        {
            _data.OnDataChanged += OnDataChangeGem;
            PlayerManager.Instance.OnTalentLearned += _data.AddTalentGemBonus;
            OnDataChangeGem(); // 立即刷新一遍
        }
        
        data.InstanceID = GetInstanceID();
    }

    void OnDestroy()
    {
        _data.OnDataChanged -= OnDataChangeGem;
        PlayerManager.Instance.OnTalentLearned -= _data.AddTalentGemBonus;
    }

    #endregion
}
