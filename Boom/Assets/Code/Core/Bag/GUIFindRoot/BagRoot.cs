using TMPro;
using UnityEngine;

public class BagRoot : MonoBehaviour
{
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
        _equipBulletSlotRoot.SetActive(true);
    }

    #region 页签切换
    //页签切换为Bullet
    public void SwichBullet()
    {
        if (EternalCavans.Instance.TutoriaSwichBulletLock) return;
        
        BagBulletRootGO.SetActive(true);
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
        BagItemRootGO.SetActive(false);
        BagGemRootGO.SetActive(true);
        //页签选中状态
        BtnBulletSC.State = UILockedState.isNormal;
        BtnItemSC.State = UILockedState.isNormal;
        BtnGemSC.State = UILockedState.isSelected;
        GM.Root.InventoryMgr._BulletInvData.ProcessBulletRelations();
    }
    #endregion
}
