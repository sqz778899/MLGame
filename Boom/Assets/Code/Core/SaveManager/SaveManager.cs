using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public static class SaveManager
{
    public static void LoadSaveFile()
    {
        SaveFileJson saveFile = TrunkManager.Instance._saveFile;
        string SaveFileJsonString = File.ReadAllText(PathConfig.SaveFileJson);
        saveFile = JsonConvert.DeserializeObject<SaveFileJson>(SaveFileJsonString);
        
        #region Character
        GM.Root.PlayerMgr._PlayerData.MaxHP = saveFile.MaxHP;
        GM.Root.PlayerMgr._PlayerData.HP = saveFile.HP;
        GM.Root.PlayerMgr._PlayerData.LostHPs = saveFile.LostHPs;
        GM.Root.PlayerMgr._PlayerData.Score = saveFile.Score;
        GM.Root.PlayerMgr._PlayerData.Coins = saveFile.Coins;
        GM.Root.PlayerMgr._PlayerData.RoomKeys = saveFile.RoomKeys;
        GM.Root.PlayerMgr._PlayerData.MagicDust = saveFile.MagicDust;
        GM.Root.PlayerMgr._PlayerData.WallwalkSkillCount = saveFile.WallwalkSkillCount;
        
        GM.Root.InventoryMgr._InventoryData.ClearData();
        //读取Item
        for (int i = 0; i < saveFile.UserItems.Count; i++)
        {
            ItemData curItem = LoadItemData(saveFile.UserItems[i]);
            GM.Root.InventoryMgr._InventoryData.AddItemToBag(curItem);
        }
        //读取Gem
        for (int i = 0; i < saveFile.UserGems.Count; i++)
        {
            GemData curGem = LoadGemData(saveFile.UserGems[i]);
            if (curGem.CurSlotController.SlotType == SlotType.GemBagSlot)
                GM.Root.InventoryMgr._InventoryData.AddGemToBag(curGem);
            if (curGem.CurSlotController.SlotType == SlotType.GemInlaySlot)
                GM.Root.InventoryMgr._InventoryData.EquipGem(curGem);
        }
        //读取奇迹物件
        Debug.Log("读取奇迹物件");
        for (int i = 0; i < saveFile.UserMiracleOddities.Count; i++)
            GM.Root.InventoryMgr.EquipMiracleOddity(saveFile.UserMiracleOddities[i].ID);
        
        //读取子弹槽状态
        GM.Root.PlayerMgr._PlayerData.CurBulletSlotLockedState = saveFile.UserBulletSlotLockedState;

        List<BulletData> curSpawners = GM.Root.InventoryMgr._BulletInvData.BagBulletSpawners;
        curSpawners.Clear();
        curSpawners.AddRange(saveFile.UserBulletSpawner.Select(LoadBulletData));
        List<BulletData> curBullets = GM.Root.InventoryMgr._BulletInvData.EquipBullets;
        curBullets.Clear();
        curBullets.AddRange(saveFile.UserCurBullets.Select(LoadBulletData));
        
        GM.Root.InventoryMgr.InitAllBagGO();//初始化背包数据
        
        //读取天赋信息
        GM.Root.PlayerMgr._PlayerData.Talents = saveFile.UserTalents;
        #endregion

        #region Quest
        List<QuestSaveData> UserQuests = saveFile.UserQuests;
        List<Quest> designQuests = GM.Root.QuestMgr.questDatabase.quests;
        List<Quest> curQuests = GM.Root.PlayerMgr._QuestData.Quests;
        foreach (var eachSave in UserQuests)
        {
            bool isExist = false;
            foreach (var eachQ in curQuests)
            {
                if (eachQ.ID == eachSave.ID)
                {
                    isExist = true;
                    eachQ.LoadQuest(eachSave);
                    break;
                }
            }
            if (!isExist)
            {
                Quest newQuest = new Quest(eachSave.ID);
                newQuest.LoadQuest(eachSave);
                curQuests.Add(newQuest);
            }
        }
        //把策划配置的新的任务更新进去。已有的更新，没有的添加
        foreach (var eachDesignQuest in designQuests)
        {
            var existingQuest = curQuests.FirstOrDefault(q => q.ID == eachDesignQuest.ID);
            if (existingQuest != null)
            {
                // Update existing quest
                existingQuest.Name = eachDesignQuest.Name;
                existingQuest.Level = eachDesignQuest.Level;
                existingQuest.Description = eachDesignQuest.Description;
            }
            else
            {
                // Add new quest
                Quest newQuest = new Quest(eachDesignQuest.ID)
                {
                    Name = eachDesignQuest.Name,
                    Level = eachDesignQuest.Level,
                    Description = eachDesignQuest.Description
                };
                curQuests.Add(newQuest);
            }
        }
        //读取剧情节点状态
        GM.Root.StorylineSys.StorylineSaveData = saveFile.UserStorylineNodesState;
        //读取主线剧情推进状态
        GM.Root.PlayerMgr._QuestData.MainStoryProgress = saveFile.UserMainStoryProgress;
        //读取新手教程完成情况
        GM.Root.PlayerMgr._PlayerData._TutorialCompletionStatus = saveFile.UserTutorial;
        #endregion
        
        LoadUserConfig();
    }

    public static void SaveFile()
    {
        SaveFileJson saveFile = TrunkManager.Instance._saveFile;
        #region Character
        saveFile.MaxHP = GM.Root.PlayerMgr._PlayerData.MaxHP;
        saveFile.HP = GM.Root.PlayerMgr._PlayerData.HP;
        saveFile.LostHPs = GM.Root.PlayerMgr._PlayerData.LostHPs;
        saveFile.Score = GM.Root.PlayerMgr._PlayerData.Score;
        saveFile.Coins = GM.Root.PlayerMgr._PlayerData.Coins;
        saveFile.RoomKeys = GM.Root.PlayerMgr._PlayerData.RoomKeys;
        saveFile.MagicDust = GM.Root.PlayerMgr._PlayerData.MagicDust;
        saveFile.WallwalkSkillCount = GM.Root.PlayerMgr._PlayerData.WallwalkSkillCount;
        
        saveFile.UserBulletSpawner.Clear();
        saveFile.UserBulletSpawner.AddRange(GM.Root.InventoryMgr._BulletInvData.
            BagBulletSpawners.Select(each =>each.ToSaveData() as BulletBaseSaveData));
        saveFile.UserCurBullets.Clear();
        saveFile.UserCurBullets.AddRange(GM.Root.InventoryMgr._BulletInvData.EquipBullets
            .Select(each =>each.ToSaveData() as BulletBaseSaveData));
        
        //存储Item数据信息
        saveFile.UserItems.Clear();
        List<ItemSaveData> UserItems = new List<ItemSaveData>();
        foreach (var each in GM.Root.InventoryMgr._InventoryData.BagItems.Concat(
                     GM.Root.InventoryMgr._InventoryData.EquipItems))
            UserItems.Add(new ItemSaveData(each));
        saveFile.UserItems = UserItems;
        
        //存储Gem信息
        saveFile.UserGems.Clear();
        List<GemBaseSaveData> UserGems = new List<GemBaseSaveData>();
        foreach (var each in GM.Root.InventoryMgr._InventoryData.BagGems)
            UserGems.Add(new GemBaseSaveData(each));
        foreach (var each in GM.Root.InventoryMgr._InventoryData.EquipGems)
            UserGems.Add(new GemBaseSaveData(each));
        saveFile.UserGems = UserGems;
        
        //存储奇迹物件信息
        saveFile.UserMiracleOddities.Clear();
        List<MiracleOdditySaveData> UserMiracleOddities = new List<MiracleOdditySaveData>();
        foreach (var each in GM.Root.InventoryMgr._InventoryData.EquipMiracleOddities)
            UserMiracleOddities.Add(new MiracleOdditySaveData(each));
        saveFile.UserMiracleOddities = UserMiracleOddities;
        
        //存子弹槽状态信息
        saveFile.UserBulletSlotLockedState = GM.Root.PlayerMgr._PlayerData.CurBulletSlotLockedState;
        //存天赋信息状态
        saveFile.UserTalents = GM.Root.PlayerMgr._PlayerData.Talents;
        #endregion
        
        #region 任务剧情新手教程等
        List<QuestSaveData> UserQuests = new List<QuestSaveData>();
        GM.Root.PlayerMgr._QuestData.Quests.ForEach(each => {UserQuests.Add(new QuestSaveData(each)); });
        saveFile.UserQuests = UserQuests;
        saveFile.UserMainStoryProgress = GM.Root.PlayerMgr._QuestData.MainStoryProgress;
        //存新手教程完成情况
        saveFile.UserTutorial = GM.Root.PlayerMgr._PlayerData._TutorialCompletionStatus;
        //存储剧情节点状态
        saveFile.UserStorylineNodesState = GM.Root.StorylineSys.SaveStorylineData();
        #endregion
        
        string content01 = JsonConvert.SerializeObject(saveFile,(Formatting) Formatting.Indented);
        File.WriteAllText(PathConfig.SaveFileJson, content01);

        SaveUserConfig();
    }

    #region 不关心的私有方法
    static BulletData LoadBulletData(BulletBaseSaveData itemBaseSaveData)
    {
        ISlotController CurSlot = SlotManager.GetSlotController(itemBaseSaveData.SlotID, itemBaseSaveData.SlotType);
        BulletData bulletData = new BulletData(itemBaseSaveData.ID,CurSlot as BulletSlotController);
        if (itemBaseSaveData is BulletBaseSaveData bulletSaveData)
            bulletData.SpawnerCount = bulletSaveData.SpawnerCount;
        return bulletData;
    }
    
    static GemData LoadGemData(GemBaseSaveData itemBaseSaveData)
    {
        ISlotController CurSlot = SlotManager.GetSlotController(itemBaseSaveData.SlotID, itemBaseSaveData.SlotType);
        GemData curGemData = new GemData(itemBaseSaveData.ID,CurSlot as GemSlotController);
        return curGemData;
    }
    
    static ItemData LoadItemData(ItemBaseSaveData itemBaseSaveData)
    {
        ISlotController CurSlot = SlotManager.GetSlotController(itemBaseSaveData.SlotID, itemBaseSaveData.SlotType);
        ItemData curItemData = new ItemData(itemBaseSaveData.ID,CurSlot as ItemSlotController);
        return curItemData;
    }

    
    static void LoadUserConfig()
    {
        UserConfig userConfig = TrunkManager.Instance._userConfig;
        string SaveFileJsonString = File.ReadAllText(PathConfig.UserConfigJson);
        userConfig = JsonConvert.DeserializeObject<UserConfig>(SaveFileJsonString);

        MultiLa.Instance.CurLanguage = (MultiLaEN)userConfig.UserLanguage;
        MSceneManager.Instance.SetScreenResolution(userConfig.UserScreenResolution);
        MSceneManager.Instance.SetScreenMode(userConfig.UserScreenMode);
    }
    
    public static void SaveUserConfig()
    {
        UserConfig userConfig = TrunkManager.Instance._userConfig;
        userConfig.UserLanguage = (int)MultiLa.Instance.CurLanguage;
        string content = JsonConvert.SerializeObject(userConfig,(Formatting) Formatting.Indented);
        File.WriteAllText(PathConfig.UserConfigJson, content);
        Debug.Log("SaveUserConfig");
    }
    #endregion
}