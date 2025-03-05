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
        MainRoleManager.Instance.MaxHP = saveFile.MaxHP;
        MainRoleManager.Instance.HP = saveFile.HP;
        MainRoleManager.Instance.Score = saveFile.Score;
        MainRoleManager.Instance.Coins = saveFile.Coins;
        MainRoleManager.Instance.RoomKeys = saveFile.RoomKeys;
        
        //读取Item
        for (int i = 0; i < saveFile.UserItems.Count; i++)
        {
            ItemJson curItem = saveFile.UserItems[i];
            BagItemManager<Item>.InitSaveFileObject(curItem,SlotType.BagSlot);
        }
        //读取Gem
        for (int i = 0; i < saveFile.UserGems.Count; i++)
        {
            GemJson curGem = saveFile.UserGems[i];
            BagItemManager<Gem>.InitSaveFileObject(curGem,SlotType.GemBagSlot);
        }
        //读取子弹槽状态
        MainRoleManager.Instance.CurBulletSlotLockedState = saveFile.UserBulletSlotLockedState;

        MainRoleManager.Instance.CurBulletSpawners.Clear();
        MainRoleManager.Instance.CurBulletSpawners.AddRange(saveFile.UserBulletSpawner.Select(LoadBulletData));
        MainRoleManager.Instance.CurBullets.Clear();
        MainRoleManager.Instance.CurBullets.AddRange(saveFile.UserCurBullets.Select(LoadBulletData));
        
        MainRoleManager.Instance.CurStandbyBulletMats = saveFile.UserStandbyBullet;
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
        MainRoleManager.Instance.CurMapSate = curMapSate;
        #endregion
        
        LoadUserConfig();
    }

    public static void SaveFile()
    {
        SaveFileJson saveFile = TrunkManager.Instance._saveFile;
        #region Character
        saveFile.MaxHP = MainRoleManager.Instance.MaxHP;
        saveFile.HP = MainRoleManager.Instance.HP;
        saveFile.Score = MainRoleManager.Instance.Score;
        saveFile.Coins = MainRoleManager.Instance.Coins;
        saveFile.RoomKeys = MainRoleManager.Instance.RoomKeys;
        
        saveFile.UserBulletSpawner.Clear();
        saveFile.UserBulletSpawner.AddRange(MainRoleManager.Instance
            .CurBulletSpawners.Select(each =>each.ToSaveData() as BulletSaveData));
        saveFile.UserCurBullets.Clear();
        saveFile.UserCurBullets.AddRange(MainRoleManager.Instance
            .CurBullets.Select(each =>each.ToSaveData() as BulletSaveData));
        
        //存储Item数据信息
        List<ItemJson> UserItems = new List<ItemJson>();
        foreach (var each in MainRoleManager.Instance.BagItems.Concat(MainRoleManager.Instance.EquipItems))
        {
            UserItems.Add(new ItemJson
            {
                ID = each.ID,          
                InstanceID = each.InstanceID,
                Rare = each.Rare,
                Name = each.Name,
                ImageName = each.ImageName,
                SlotID = each.SlotID,
                SlotType = each.SlotType,
                Attribute = new ItemAttribute
                {
                    waterElement = each.Attribute.waterElement,
                    fireElement = each.Attribute.fireElement,
                    thunderElement = each.Attribute.thunderElement,
                    lightElement = each.Attribute.lightElement,
                    darkElement = each.Attribute.darkElement,

                    extraWaterDamage = each.Attribute.extraWaterDamage,
                    extraFireDamage = each.Attribute.extraFireDamage,
                    extraThunderDamage = each.Attribute.extraThunderDamage,
                    extraLightDamage = each.Attribute.extraLightDamage,
                    extraDarkDamage = each.Attribute.extraDarkDamage,

                    maxDamage = each.Attribute.maxDamage
                }
            });
        }
        saveFile.UserItems = UserItems;
        
        //存储Gem信息
        List<GemJson> UserGems = new List<GemJson>();
        foreach (var each in MainRoleManager.Instance.BagGems.Concat(MainRoleManager.Instance.InLayGems))
        {
            UserGems.Add(new GemJson
            {
                ID = each.ID,
                InstanceID = each.InstanceID,
                Name = each.Name,
                Attribute = new GemAttribute
                {
                    Damage = each.Attribute.Damage,
                    Piercing = each.Attribute.Piercing,
                    Resonance = each.Attribute.Resonance
                },
                ImageName = each.ImageName,
                SlotID = each.SlotID,
                SlotType = each.SlotType
            });
        }
        saveFile.UserGems = UserGems;
        //存子弹槽状态信息
        saveFile.UserBulletSlotLockedState = MainRoleManager.Instance.CurBulletSlotLockedState;
        
        saveFile.UserStandbyBullet = MainRoleManager.Instance.CurStandbyBulletMats;
        #endregion
        
        #region Map
        List<MapSate> UserMapSate = new List<MapSate>();
        MapSate curMapState = new MapSate();
        curMapState = MainRoleManager.Instance.CurMapSate;
        UserMapSate.Add(curMapState);
        saveFile.UserMapSate = UserMapSate;
        #endregion
        
        string content01 = JsonConvert.SerializeObject(saveFile,(Formatting) Formatting.Indented);
        File.WriteAllText(PathConfig.SaveFileJson, content01);

        SaveUserConfig();
    }

    #region 不关心的私有方法
    static BulletData LoadBulletData(BulletSaveData _itemSaveData)
    {
        BulletData bulletData = new BulletData(_itemSaveData.ID);
        bulletData.SlotID = _itemSaveData.SlotID;
        bulletData.SlotType = _itemSaveData.SlotType;
        if (_itemSaveData is BulletSaveData bulletSaveData)
            bulletData.SpawnerCount = bulletSaveData.SpawnerCount;
        return bulletData;
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