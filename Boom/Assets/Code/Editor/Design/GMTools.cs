using Sirenix.OdinInspector;

public class GMTools
{
    [TitleGroup("魔尘&&金币&&钥匙&&血量")] public int s;
    [Button("魔尘添加",ButtonSizes.Large),PropertyOrder(0)]
    [HorizontalGroup("魔尘&&金币添加",0.2f)]
    void AddDust() => GM.Root.PlayerMgr._PlayerData.ModifyMagicDust(10000);
    
    [Button("金币添加",ButtonSizes.Large),PropertyOrder(0)]
    [HorizontalGroup("魔尘&&金币添加",0.2f)]
    void AddCoins() => GM.Root.PlayerMgr._PlayerData.ModifyCoins(1000);
    
    [Button("钥匙添加",ButtonSizes.Large),PropertyOrder(0)]
    [HorizontalGroup("魔尘&&金币添加",0.2f)]
    void AddKeys() => GM.Root.PlayerMgr._PlayerData.ModifyRoomKeys(1);
    
    [Button("血量添加",ButtonSizes.Large),PropertyOrder(0)]
    [HorizontalGroup("魔尘&&金币添加",0.2f)]
    void AddHPs() => GM.Root.PlayerMgr._PlayerData.ModifyHP(1);
    
    [Button("穿墙术添加",ButtonSizes.Large),PropertyOrder(0)]
    [HorizontalGroup("魔尘&&金币添加",0.2f)]
    void AddSkillWalk() => GM.Root.PlayerMgr._PlayerData.ModifyWallwalkSkillCount(1);
    
    [Title("剧情解锁程度")]
    [PropertyOrder(1)]
    [InlineButton("SetStoryLine","剧情解锁程度")]
    public int progress;
    void SetStoryLine() => GM.Root.PlayerMgr._QuestData.MainStoryProgress = progress;
    
    [Title("道具测试")]
    [PropertyOrder(2)]
    [InlineButton("AddItem","获得道具")]
    public int ItemID;
    void AddItem() => GM.Root.InventoryMgr.AddItemToBag(ItemID);
  
    [Title("宝石测试")]
    [PropertyOrder(3)]
    [InlineButton("AddGem","获得宝石")]
    public int GemID;
    void AddGem() => GM.Root.InventoryMgr.AddGemToBag(GemID);
 
    [Title("奇迹物件测试")]
    [PropertyOrder(4)]
    [InlineButton("AddMiracleOddity","获得奇迹物件")]
    public int MiracleOddityID;
    void AddMiracleOddity() => GM.Root.InventoryMgr.EquipMiracleOddity(MiracleOddityID);
    
    [Title("解锁子弹槽")]
    [Button("解锁子弹槽",ButtonSizes.Large),PropertyOrder(5)]
    void UnlockBulletSlot()
    {
        for (int i = 0; i < 5; i++)
            GM.Root.PlayerMgr._PlayerData.SetBulletSlotLockedState(i,true);
    }
}
