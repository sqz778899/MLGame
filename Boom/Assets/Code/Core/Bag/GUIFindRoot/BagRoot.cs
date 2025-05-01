using System;
using System.Collections.Generic;
using TMPro;
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

    [Header("道具特质相关")] 
    public TextMeshProUGUI txtSynergies;
    public TextMeshProUGUI txtSynergiesDesc;
    public TraitIcon traitIcon;
    
    void Start()
    {
        SwichGem();
        GM.Root.InventoryMgr._InventoryData.OnStructureChanged += RefreshSynergies;
        RefreshSynergies();//先同步一次
    }

    //特质触发
    public void RefreshSynergies()
    {
        List<TraitData> traitInfos = 
            GM.Root.InventoryMgr._ItemEffectMrg.GetCurrentSynergiesInfos();
        if (traitInfos.Count == 0)
        {
            txtSynergies.text = "";
            txtSynergiesDesc.text = "";
            traitIcon.gameObject.SetActive(false);
            return;
        }
        TraitData SynergiesInfo = traitInfos[0];
        txtSynergies.text = SynergiesInfo.Name;
        txtSynergiesDesc.text = SynergiesInfo.Desc;
        traitIcon.gameObject.SetActive(true);
        traitIcon.BindingData(traitInfos[0]);
    }

    #region 页签切换
    //页签切换为Bullet
    public void SwichBullet()
    {
        if (EternalCavans.Instance.TutoriaSwichBulletLock) return;
        
        BagBulletRootGO.SetActive(true);
        BagReadySlotGO.SetActive(true);
        BagItemRootGO.SetActive(false);
        BagGemRootGO.SetActive(false);
        //页签选中状态
        BtnBulletSC.State = UILockedState.isSelected;
        BtnItemSC.State = UILockedState.isNormal;
        BtnGemSC.State = UILockedState.isNormal;
    }

    //页签切换为Item
    public void SwichItem()   
    {
        //if(!IsUnLockedItem) return;
        
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
        if (EternalCavans.Instance.TutorialSwichGemLock) return;
        
        BagBulletRootGO.SetActive(false);
        BagReadySlotGO.SetActive(true);
        BagItemRootGO.SetActive(false);
        BagGemRootGO.SetActive(true);
        //页签选中状态
        BtnBulletSC.State = UILockedState.isNormal;
        BtnItemSC.State = UILockedState.isNormal;
        BtnGemSC.State = UILockedState.isSelected;
    }
    #endregion

    void OnDestroy() =>
        GM.Root.InventoryMgr._InventoryData.OnStructureChanged -= RefreshSynergies;
}
