using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class BagRoot : MonoBehaviour
{
    public bool IsUnLockedItem = false;
    [Header("背包页签相关资源")]
    public GameObject BagBulletRootGO;
    public GameObject BagItemRootGO;
    public GameObject BagGemRootGO;
    [Header("背包页签按钮脚本")]
    public ButtonStateSwitch BtnBulletSC;
    public ButtonStateSwitch BtnItemSC;
    public ButtonStateSwitch BtnGemSC;
    
    [Header("背包其它资源")]
    public GameObject BagReadySlotGO;  //子弹槽
    BulletSlotRole[] _btnReadySlotSC;
    public GameObject DragObjRootGO;
    [Header("特效资源")]
    public GameObject SlotFx;
    ResonanceSlotCol[] _slotFxs;

    #region 初始化数据
    private void Awake()
    {
        _slotFxs = SlotFx.GetComponentsInChildren<ResonanceSlotCol>();
        _btnReadySlotSC = BagReadySlotGO.GetComponentsInChildren<BulletSlotRole>();
    }

    void Start()
    {
        SwichGem();
        if (!IsUnLockedItem)
            BtnItemSC.State = UILockedState.isLocked;
        //注册事件。背包Slot解锁的话，这边会函数响应更新状态
        MainRoleManager.Instance.BulletSlotStateChanged += RefreshBulletSlotLockedState;
        RefreshBulletSlotLockedState();//最开始先更新一次状态
    }

    public void InitData()
    {
        _slotFxs ??= SlotFx.GetComponentsInChildren<ResonanceSlotCol>();
        _slotFxs.ToList().ForEach(perFX => perFX.CloseEffect());
        MainRoleManager.Instance.RefreshAllItems();
    }
    #endregion

    public void RefreshBulletSlotLockedState()
    {
        Dictionary<int, bool> curDict = MainRoleManager.Instance.CurBulletSlotLockedState;
        for (int i = 0; i < 5; i++)
            _btnReadySlotSC[i].State = curDict[i]?UILockedState.isNormal:UILockedState.isLocked;
    }
    
    //页签切换为Bullet
    public void SwichBullet()
    {
        BagBulletRootGO.SetActive(true);
        BagReadySlotGO.SetActive(true);
        BagItemRootGO.SetActive(false);
        BagGemRootGO.SetActive(false);
        //页签选中状态
        BtnBulletSC.State = UILockedState.isSelected;
        BtnItemSC.State = IsUnLockedItem? UILockedState.isNormal : UILockedState.isLocked;
        BtnGemSC.State = UILockedState.isNormal;
    }

    //页签切换为Item
    public void SwichItem()   
    {
        if(!IsUnLockedItem) return;
        
        BagBulletRootGO.SetActive(false);
        BagReadySlotGO.SetActive(false);
        BagItemRootGO.SetActive(true);
        BagGemRootGO.SetActive(false);
        //页签选中状态
        BtnBulletSC.State = UILockedState.isNormal;
        BtnItemSC.State = UILockedState.isSelected;
        BtnGemSC.State = UILockedState.isNormal;
    }
    
    //页签切换为Gem
    public void SwichGem()
    {
        BagBulletRootGO.SetActive(false);
        BagReadySlotGO.SetActive(true);
        BagItemRootGO.SetActive(false);
        BagGemRootGO.SetActive(true);
        //页签选中状态
        BtnBulletSC.State = UILockedState.isNormal;
        BtnItemSC.State = IsUnLockedItem? UILockedState.isNormal : UILockedState.isLocked;
        BtnGemSC.State = UILockedState.isSelected;
    }
}
