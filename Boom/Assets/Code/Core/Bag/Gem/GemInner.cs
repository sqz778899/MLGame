using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GemInner:DragBase
{
    public GemData _data;
    
    [Header("资产")]
    public Image ItemSprite;
    
    BagRootMini _bagRootMini;
    public Action OnGemDragged;
    
    internal override void Start()
    {
        base.Start();
        _bagRootMini = UIManager.Instance.BagUI.BagRootMiniGO.GetComponent<BagRootMini>();
        OnGemDragged += _bagRootMini.BulletDragged;
    }
    
    #region UI交互逻辑
    //鼠标按下
    public override void OnPointerDown(PointerEventData eventData)
    {
        UIManager.Instance.IsLockedClick = true;
        _eventData = eventData;
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            originalParent = gameObject.transform.parent;//记录原始父层级
            gameObject.transform.SetParent(UIManager.Instance.CommonUI.DragObjRoot.transform);//改变父层级
            originalPosition = gameObject.transform.position;
        }
        
        if (eventData.button == PointerEventData.InputButton.Right)
            RightClick();
        HideTooltips();
        
        OnGemDragged?.Invoke();
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
                GemSlotInner curGemSlot = curSlotSC as GemSlotInner;
                
                if (curGemSlot == null) continue;
                if (curGemSlot.CurGemSlot.CurGemData == _data) break;//如果是同一个宝石，不做任何操作
                
                //2）空槽逻辑
                if (curGemSlot.CurGemSlot.MainID == -1)
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
        GemSlotInner slotInner = targetSlot as GemSlotInner;
        SlotManager.ClearSlot(_data.CurSlot);
        switch (slotInner.SlotType)
        {
            case SlotType.GemBagSlot:
                slotInner.CurGemSlot.SOnDrop(_data.CurSlot.ChildIns);
                ToolTipsOffset = new Vector3(1.01f, -0.5f, 0);
                Destroy(gameObject);
                break;
            case SlotType.GemInlaySlot:
                slotInner.CurGemSlot.SOnDrop(_data.CurSlot.ChildIns);
                ToolTipsOffset = new Vector3(-0.92f, -0.52f, 0);
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }
    
    //落下交换逻辑
    internal override void OnDropFillSlot(SlotBase targetSlot)
    {
        //先把目标槽位的物品拿出来
        GemSlotInner slotInner = targetSlot as GemSlotInner;
        GameObject sorChildIns = _data.CurSlot.ChildIns;
        GemSlot curGemSlot = slotInner.CurGemSlot;
        GameObject targetChildIns = curGemSlot.ChildIns;
        _data.CurSlot.SOnDrop(targetChildIns);
        //再把自己放进去
        curGemSlot.SOnDrop(sorChildIns);
    }
    
    internal override void NonFindSlot()
    {
        // 如果没有找到槽位，那么物品回到原始位置
        gameObject.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        gameObject.transform.SetParent(originalParent,false);//还原父层级
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
    
    public void OnDataChangeGem()
    {
        _data.InstanceID = gameObject.GetInstanceID();
        GemJson gemDesignData = TrunkManager.Instance.GetGemJson(_data.ID);
        gameObject.name = gemDesignData.Name + _data.InstanceID;
        ItemSprite.sprite = ResManager.instance.GetAssetCache<Sprite>(
            PathConfig.GetGemPath(gemDesignData.ImageName));
    }
    
    void OnDestroy()
    {
        if (_bagRootMini != null)
            OnGemDragged -= _bagRootMini.BulletDragged;
    }
    #endregion
}