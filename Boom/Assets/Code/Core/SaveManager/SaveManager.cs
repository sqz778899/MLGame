using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public static class SaveManager
{
    public static void LoadSaveFile()
    {
        SaveFileJson saveFile = TrunkManager.Instance._saveFile;
        string SaveFileJsonString = File.ReadAllText(PathConfig.SaveFileJson);
        saveFile = JsonConvert.DeserializeObject<SaveFileJson>(SaveFileJsonString);
        
        #region Character
        PlayerManager.Instance._PlayerData.MaxHP = saveFile.MaxHP;
        PlayerManager.Instance._PlayerData.HP = saveFile.HP;
        PlayerManager.Instance._PlayerData.Score = saveFile.Score;
        PlayerManager.Instance._PlayerData.Coins = saveFile.Coins;
        PlayerManager.Instance._PlayerData.RoomKeys = saveFile.RoomKeys;
        
        //读取Item
        for (int i = 0; i < saveFile.UserItems.Count; i++)
        {
            ItemData curItem = LoadItemData(saveFile.UserItems[i]);
            BagItemTools<Item>.InitSaveFileObject(curItem,SlotType.BagSlot);
        }
        //读取Gem
        for (int i = 0; i < saveFile.UserGems.Count; i++)
        {
            GemData curGem = LoadGemData(saveFile.UserGems[i]);
            BagItemTools<Gem>.InitSaveFileObject(curGem,SlotType.GemBagSlot);
        }
        //读取子弹槽状态
        PlayerManager.Instance._PlayerData.CurBulletSlotLockedState = saveFile.UserBulletSlotLockedState;

        List<BulletData> curSpawners = InventoryManager.Instance._BulletInvData.BagBulletSpawners;
        curSpawners.Clear();
        curSpawners.AddRange(saveFile.UserBulletSpawner.Select(LoadBulletData));
        List<BulletData> CurBullets = InventoryManager.Instance._BulletInvData.EquipBullets;
        CurBullets.Clear();
        CurBullets.AddRange(saveFile.UserCurBullets.Select(LoadBulletData));
        
        //MainRoleManager.Instance.CurStandbyBulletMats = saveFile.UserStandbyBullet;
        #endregion

        #region Quest
        List<QuestSaveData> UserQuests = saveFile.UserQuests;
        List<Quest> curQuests = QuestManager.Instance.questDatabase.quests;
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
        #endregion
        
        #region Map
        List<MapSate> UserMapSate = saveFile.UserMapSate;
        MapSate curMapSate = null;
        foreach (var eachMapSate in UserMapSate)
        {
            if (eachMapSate.CurLevelID == 1)
            {
                curMapSate = eachMapSate;
                break;
            }
        }
        BattleManager.Instance.battleData.CurMapSate = curMapSate;
        #endregion
        
        LoadUserConfig();
    }

    public static void SaveFile()
    {
        SaveFileJson saveFile = TrunkManager.Instance._saveFile;
        #region Character
        saveFile.MaxHP = PlayerManager.Instance._PlayerData.MaxHP;
        saveFile.HP = PlayerManager.Instance._PlayerData.HP;
        saveFile.Score = PlayerManager.Instance._PlayerData.Score;
        saveFile.Coins = PlayerManager.Instance._PlayerData.Coins;
        saveFile.RoomKeys = PlayerManager.Instance._PlayerData.RoomKeys;
        
        saveFile.UserBulletSpawner.Clear();
        saveFile.UserBulletSpawner.AddRange(InventoryManager.Instance._BulletInvData.
            BagBulletSpawners.Select(each =>each.ToSaveData() as BulletBaseSaveData));
        saveFile.UserCurBullets.Clear();
        saveFile.UserCurBullets.AddRange(InventoryManager.Instance._BulletInvData.EquipBullets
            .Select(each =>each.ToSaveData() as BulletBaseSaveData));
        
        //存储Item数据信息
        saveFile.UserItems.Clear();
        List<ItemSaveData> UserItems = new List<ItemSaveData>();
        foreach (var each in InventoryManager.Instance._InventoryData.BagItems.Concat(
                     InventoryManager.Instance._InventoryData.EquipItems))
            UserItems.Add(new ItemSaveData(each));
        saveFile.UserItems = UserItems;
        
        //存储Gem信息
        saveFile.UserGems.Clear();
        List<GemBaseSaveData> UserGems = new List<GemBaseSaveData>();
        foreach (var each in InventoryManager.Instance._InventoryData.BagGems.Concat(
                     InventoryManager.Instance._InventoryData.EquipGems))
            UserGems.Add(new GemBaseSaveData(each));
        saveFile.UserGems = UserGems;
        
        //存子弹槽状态信息
        saveFile.UserBulletSlotLockedState = PlayerManager.Instance._PlayerData.CurBulletSlotLockedState;
        
        //saveFile.UserStandbyBullet = MainRoleManager.Instance.CurStandbyBulletMats;
        #endregion
        
        #region Quest
        List<QuestSaveData> UserQuests = new List<QuestSaveData>();
        QuestManager.Instance.questDatabase.quests.ForEach(each =>
        {
            UserQuests.Add(new QuestSaveData(each));
        });
        saveFile.UserQuests = UserQuests;
        #endregion
        
        #region Map
        List<MapSate> UserMapSate = new List<MapSate>();
        MapSate curMapState = new MapSate();
        curMapState = BattleManager.Instance.battleData.CurMapSate;
        UserMapSate.Add(curMapState);
        saveFile.UserMapSate = UserMapSate;
        #endregion
        
        string content01 = JsonConvert.SerializeObject(saveFile,(Formatting) Formatting.Indented);
        File.WriteAllText(PathConfig.SaveFileJson, content01);

        SaveUserConfig();
    }

    #region 不关心的私有方法
    static BulletData LoadBulletData(BulletBaseSaveData itemBaseSaveData)
    {
        SlotBase CurSlot = SlotManager.GetSlot(itemBaseSaveData.SlotID, itemBaseSaveData.SlotType);
        BulletData bulletData = new BulletData(itemBaseSaveData.ID,CurSlot);
        if (itemBaseSaveData is BulletBaseSaveData bulletSaveData)
            bulletData.SpawnerCount = bulletSaveData.SpawnerCount;
        return bulletData;
    }
    
    static GemData LoadGemData(GemBaseSaveData itemBaseSaveData)
    {
        SlotBase CurSlot = SlotManager.GetSlot(itemBaseSaveData.SlotID, itemBaseSaveData.SlotType);
        GemData curGemData = new GemData(itemBaseSaveData.ID,CurSlot);
        return curGemData;
    }
    
    static ItemData LoadItemData(ItemBaseSaveData itemBaseSaveData)
    {
        SlotBase CurSlot = SlotManager.GetSlot(itemBaseSaveData.SlotID, itemBaseSaveData.SlotType);
        ItemData curItemData = new ItemData(itemBaseSaveData.ID,CurSlot);
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