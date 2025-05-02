using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BagRoot : MonoBehaviour
{
    public bool IsUnLockedItem = false;
    [Header("背包页签相关资源")]
    [SerializeField] GameObject BagBulletRootGO;
    [SerializeField] GameObject BagItemRootGO;
    [SerializeField] GameObject BagGemRootGO;
    [Header("背包页签按钮脚本")]
    [SerializeField] ButtonStateSwitch BtnBulletSC;
    [SerializeField] ButtonStateSwitch BtnItemSC;
    [SerializeField] ButtonStateSwitch BtnGemSC;

    [Header("道具特质相关")] 
    [SerializeField] TextMeshProUGUI txtSynergies;
    [SerializeField] TextMeshProUGUI txtSynergiesDesc;
    [SerializeField] TraitIcon traitIcon;
    
    GameObject _equipBulletSlotRoot => EternalCavans.Instance.EquipBulletSlotRoot;  //子弹槽
    
    void Start()
    {
        SwichGem();
        GM.Root.InventoryMgr._InventoryData.OnStructureChanged += RefreshSynergies;
        RefreshSynergies();//先同步一次
    }

    //特质触发，同步特质UI资产
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
        txtSynergiesDesc.text =  TextProcessor.Parse(SynergiesInfo.Desc);
        traitIcon.gameObject.SetActive(true);
        traitIcon.BindingData(traitInfos[0]);
    }

    #region 页签切换
    //页签切换为Bullet
    public void SwichBullet()
    {
        if (EternalCavans.Instance.TutoriaSwichBulletLock) return;
        
        BagBulletRootGO.SetActive(true);
        _equipBulletSlotRoot.SetActive(true);
        BagItemRootGO.SetActive(false);
        BagGemRootGO.SetActive(false);
        //页签选中状态
        BtnBulletSC.State = UILockedState.isSelected;
        BtnItemSC.State = UILockedState.isNormal;
        BtnGemSC.State = UILockedState.isNormal;
        GM.Root.InventoryMgr._BulletInvData.ProcessBulletRelations();
    }

    //页签切换为Item
    public void SwichItem()   
    {
        //if(!IsUnLockedItem) return;
        
        BagBulletRootGO.SetActive(false);
        _equipBulletSlotRoot.SetActive(false);
        BagItemRootGO.SetActive(true);
        BagGemRootGO.SetActive(false);
        //页签选中状态
        BtnBulletSC.State = UILockedState.isNormal;
        BtnItemSC.State = UILockedState.isSelected;
        BtnGemSC.State = UILockedState.isNormal;
        GM.Root.InventoryMgr._BulletInvData.ProcessBulletRelations();
    }
    
    //页签切换为Gem
    public void SwichGem()
    {
        if (EternalCavans.Instance.TutorialSwichGemLock) return;
        
        BagBulletRootGO.SetActive(false);
        _equipBulletSlotRoot.SetActive(true);
        BagItemRootGO.SetActive(false);
        BagGemRootGO.SetActive(true);
        //页签选中状态
        BtnBulletSC.State = UILockedState.isNormal;
        BtnItemSC.State = UILockedState.isNormal;
        BtnGemSC.State = UILockedState.isSelected;
        GM.Root.InventoryMgr._BulletInvData.ProcessBulletRelations();
    }
    #endregion

    void OnDestroy() =>
        GM.Root.InventoryMgr._InventoryData.OnStructureChanged -= RefreshSynergies;
}
