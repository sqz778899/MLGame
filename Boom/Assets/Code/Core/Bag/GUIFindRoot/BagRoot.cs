using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    public GameObject GroupBulletSpawnerSlot;
    public GameObject BagReadySlotGO;  //子弹槽
    public GameObject EquipItemRoot;  //装备栏
    BulletSlotRole[] _btnReadySlotSC;
    [Header("特效资源")]
    public GameObject SlotFx;
    ResonanceSlotCol[] _slotFxs;
    
    #region 初始化数据
    //游戏最开始时候需要初始化的数据
    public void InitData()
    {
        //共振特效资产初始化
        _slotFxs = SlotFx.GetComponentsInChildren<ResonanceSlotCol>();
        _slotFxs.ToList().ForEach(perFX => perFX.CloseEffect());
        //子弹槽初始化
        _btnReadySlotSC = BagReadySlotGO.GetComponentsInChildren<BulletSlotRole>(true);
        _btnReadySlotSC.ToList().ForEach(perSlot => perSlot.InitData());
    }
    
    void Start()
    {
        SwichGem();
        if (!IsUnLockedItem)
            BtnItemSC.State = UILockedState.isLocked;
        //注册事件。背包Slot解锁的话，这边会函数响应更新状态
        PlayerManager.Instance._PlayerData.BulletSlotStateChanged += RefreshBulletSlotLockedState;
        RefreshBulletSlotLockedState();//最开始先更新一次状态
    }
    #endregion
    
    public void RefreshBulletSlotLockedState()
    {
        Dictionary<int, bool> curDict = PlayerManager.Instance._PlayerData.CurBulletSlotLockedState;
        for (int i = 0; i < 5; i++)
            _btnReadySlotSC[i].State = curDict[i]?UILockedState.isNormal:UILockedState.isLocked;
    }

    #region 页签切换
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
    #endregion

    void OnDestroy()
    {
        PlayerManager.Instance._PlayerData.BulletSlotStateChanged -= RefreshBulletSlotLockedState;
    }
}
