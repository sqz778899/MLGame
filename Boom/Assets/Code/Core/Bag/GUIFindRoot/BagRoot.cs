using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BagRoot : MonoBehaviour
{
    [Header("背包页签相关资源")]
    public GameObject BagBulletRootGO;
    public GameObject BagItemRootGO;
    public GameObject BagGemRootGO;
    public GameObject BagReadySlotGO;
    public GameObject DragObjRootGO;
    
    [Header("特效资源")]
    public GameObject SlotFx;
    ResonanceSlotCol[] _slotFxs;
    void Start()
    {
        _slotFxs = SlotFx.GetComponentsInChildren<ResonanceSlotCol>();
    }

    public void InitData()
    {
        if(_slotFxs == null)
            _slotFxs = SlotFx.GetComponentsInChildren<ResonanceSlotCol>();
        foreach (var each in _slotFxs)
            each.CloseEffect();
        
        MainRoleManager.Instance.RefreshAllItems();
    }
    
    public void SwichBullet()
    {
        BagBulletRootGO.SetActive(true);
        BagReadySlotGO.SetActive(true);
        BagItemRootGO.SetActive(false);
        BagGemRootGO.SetActive(false);
    }

    public void SwichItem()
    {
        BagBulletRootGO.SetActive(false);
        BagReadySlotGO.SetActive(false);
        BagItemRootGO.SetActive(true);
        BagGemRootGO.SetActive(false);
    }
    
    public void SwichGem()
    {
        BagBulletRootGO.SetActive(false);
        BagReadySlotGO.SetActive(true);
        BagItemRootGO.SetActive(false);
        BagGemRootGO.SetActive(true);
    }
}
