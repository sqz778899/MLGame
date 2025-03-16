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

    public void InitAllBagGO()
    {
        InitEquipBullets();
        InitSpawners();
    }
    
    public void InitEquipBullets()
    {
        //...............Clear Old Data....................
        Bullet[] oldBullets = EquipBulletSlotRoot.GetComponentsInChildren<Bullet>();
        for (int i = oldBullets.Length - 1; i >= 0; i--)
            GameObject.Destroy(oldBullets[i].gameObject);
        SlotManager.GetEmptySlot(SlotType.CurBulletSlot);
        //..............Instance New Data..................
        foreach (BulletData each in InventoryManager.Instance._BulletInvData.EquipBullets)
        {
            GameObject BulletIns = BulletFactory.CreateBullet(each, BulletInsMode.EditB).gameObject;
            each.CurSlot.SOnDrop(BulletIns);
        }
    }
    
    public void InitSpawners()
    {
        //..............Clear Old Data..................
        DraggableBulletSpawner[] oldSpawner = SpawnerSlotRoot.GetComponentsInChildren<DraggableBulletSpawner>();
        for (int i = oldSpawner.Length - 1; i >= 0; i--)
            GameObject.Destroy(oldSpawner[i].gameObject);
        //..............Instance New Data..................
        BulletSlot[] slots = SpawnerSlotRoot.GetComponentsInChildren<BulletSlot>();
        BulletSlot[] slotMinis = SpawnerSlotRootMini.GetComponentsInChildren<BulletSlot>();
        InitSpawnersSingel(slots);
        InitSpawnersSingel(slotMinis,true);
    }
    
    void InitSpawnersSingel(BulletSlot[] slots, bool isMini = false)
    {
        foreach (BulletData each in InventoryManager.Instance._BulletInvData.BagBulletSpawners)
        {
            int curSpawnerFindID = each.ID % 10;
            var slot = slots.FirstOrDefault(s => s.SlotID == curSpawnerFindID);
            if (slot != null)
            {
                slot.MainID = each.ID;
                GameObject newSpawnerIns = null;
                if (isMini)
                    newSpawnerIns = BulletFactory.CreateBullet(each, BulletInsMode.SpawnerInner).gameObject;
                else
                    newSpawnerIns = BulletFactory.CreateBullet(each, BulletInsMode.Spawner).gameObject;
                newSpawnerIns.transform.SetParent(slot.gameObject.transform, false);
            }
        }
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

    public void HideBag() => BagRootGO.SetActive(false);
    
    public void HideMiniBag() => BagRootMiniGO.SetActive(false);
    #endregion
}