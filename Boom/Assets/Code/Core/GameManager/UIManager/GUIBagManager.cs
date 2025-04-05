using System.Linq;
using UnityEngine;

public class GUIBagManager
{
    //根节点
    public GameObject BagRootGO { get; private set; }
    public BagRoot BagRootSC;
    public GameObject BagRootMiniGO { get; private set; }
    public BagRootMini BagRootMiniSC;
    
    //子节点
    public GameObject SpawnerSlotRoot{ get; private set; } //子弹孵化器的Group
    public GameObject SpawnerSlotRootMini{ get; private set; }//子弹孵化器的GroupMini背包
    public GameObject EquipBulletSlotRoot{ get; private set; }//人物右侧的子弹槽
    
    //物品根节点
    public GameObject ItemRoot { get; private set; }//背包物品根节点
    public GameObject EquipItemRoot { get; private set; }//装备栏
    public GameObject GemRoot { get; private set; }//宝石根节点

    public GUIBagManager()
    {
        BagRootGO = EternalCavans.Instance.BagRoot;
        BagRootSC = BagRootGO.GetComponent<BagRoot>();
        BagRootMiniGO = EternalCavans.Instance.BagRootMini;
        BagRootMiniSC = BagRootMiniGO.GetComponent<BagRootMini>();
        
        SpawnerSlotRoot = BagRootSC.GroupBulletSpawnerSlot;
        SpawnerSlotRootMini = BagRootMiniSC.GroupBulletSpawnerSlot;
        
        EquipBulletSlotRoot = BagRootSC.BagReadySlotGO;
        
        ItemRoot = BagRootSC.BagItemRootGO;
        EquipItemRoot = BagRootSC.EquipItemRoot;
        GemRoot = BagRootSC.BagGemRootGO;
    }
    
    #region 显示/隐藏背包相关
    public void ShowBag()
    {
        BagRootGO.SetActive(true);
        BagRootSC.InitData();
    }
    
    public void ShowMiniBag()
    {
        BagRootMiniGO.SetActive(true);
        BagRootMiniSC.InitData();
    }

    public void HideBag()
    {
        BagRootGO.SetActive(false);
    }

    public void HideMiniBag() => BagRootMiniGO.SetActive(false);
    #endregion
}