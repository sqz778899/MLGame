using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public class TrunkManager: ScriptableObject
{
    #region 单例
    static TrunkManager s_instance;
    
    public static TrunkManager Instance
    {
        get
        {
            if (s_instance == null)
                s_instance = ResManager.instance.GetAssetCache<TrunkManager>(PathConfig.TrunkManagerOBJ);
            return s_instance;
        }
    }
    #endregion

    #region 策划数据
    List<BulletJson> _bulletDesignJsons;
    List<ItemJson> _itemDesignJsons;
    List<TalentDataJson> _buffDesignJsons;
    List<LevelBuff> _levelBuffDesignJsons;
    List<RoleBase> _roleDesignJsons;
    List<RollPREvent> _prDesignJsons;
    List<BulletEntry> _bulletEntryDesignJsons;
    List<GemJson> _gemDesignJsons;
    
    Dictionary<string,List<DiaSingle>> _dialogueDesignJsons;//对话相关
    public List<BulletJson> BulletDesignJsons
    {
        get
        {
            if (_bulletDesignJsons == null|| _bulletDesignJsons.Count == 0)
                _bulletDesignJsons = LoadBulletData();
            return _bulletDesignJsons;
        }
    }
    
    public List<ItemJson> ItemDesignJsons
    {
        get
        {
            _itemDesignJsons = LoadItemData();
            /*if (_itemDesignJsons == null||_itemDesignJsons.Count ==0)
                _itemDesignJsons = LoadItemData();*/
            return _itemDesignJsons;
        }
    }
    
    public List<TalentDataJson> BuffDesignJsons
    {
        get
        {
            if (_buffDesignJsons == null||_bulletDesignJsons.Count == 0)
                _buffDesignJsons = LoadBuffData();
            return _buffDesignJsons;
        }
    }

    public List<LevelBuff> LevelBuffDesignJsons
    {
        get
        {
            if (_levelBuffDesignJsons == null || _levelBuffDesignJsons.Count == 0)
                _levelBuffDesignJsons = LoadLevelBuffData();
            return _levelBuffDesignJsons;
        }
    }
    
    public List<RoleBase> RoleDesignJsons
    {
        get
        {
            if (_roleDesignJsons == null || _roleDesignJsons.Count == 0)
                _roleDesignJsons = LoadRoleBaseData();
            return _roleDesignJsons;
        }
    }
    
    public List<RollPREvent> PRDesignJsons
    {
        get
        {
            if (_prDesignJsons == null || _prDesignJsons.Count == 0)
                _prDesignJsons = LoadPRDesignData();
            return _prDesignJsons;
        }
    }

    public List<BulletEntry> BulletEntryDesignJsons
    {
        get
        {
            if (_bulletEntryDesignJsons == null || _bulletEntryDesignJsons.Count == 0)
                _bulletEntryDesignJsons = LoadBulletEntryDesignData();
            return _bulletEntryDesignJsons;
        }
    }
    
    public List<GemJson> GemDesignJsons
    {
        get
        {
            if (_gemDesignJsons == null || _gemDesignJsons.Count == 0)
                _gemDesignJsons = LoadGemData();
            return _gemDesignJsons;
        }
    }

    public Dictionary<string, List<DiaSingle>> DialogueDesignJsons
    {
        get
        {
            if (_dialogueDesignJsons == null || _dialogueDesignJsons.Count == 0)
                _dialogueDesignJsons = LoadDialogueDesignData();
            return _dialogueDesignJsons;
        }
    }

    public Dictionary<string, List<DiaSingle>> LoadDialogueDesignData()
    {
        string dialogueString = File.ReadAllText(PathConfig.DialogueDesignJson);
        Dictionary<string, List<DiaSingle>> dialogueJson = JsonConvert.
            DeserializeObject<Dictionary<string, List<DiaSingle>>>(dialogueString);
        return dialogueJson;
    }
    
    public List<BulletJson> LoadBulletData()
    {
        string BulletDesignString = File.ReadAllText(PathConfig.BulletDesignJson);
        List<BulletJson> BulletDataJsons = JsonConvert.DeserializeObject<List<BulletJson>>(BulletDesignString);
        return BulletDataJsons;
    }
    
    public List<ItemJson> LoadItemData()
    {
        string ItemDesignString = File.ReadAllText(PathConfig.ItemDesignJson);
        List<ItemJson> ItemDataJsons = JsonConvert.DeserializeObject<List<ItemJson>>(ItemDesignString);
        return ItemDataJsons;
    }
    
    public List<TalentDataJson> LoadBuffData()
    {
        string BuffDesignString = File.ReadAllText(PathConfig.BuffDesignJson);
        List<TalentDataJson> BuffDataJsons = JsonConvert.DeserializeObject<List<TalentDataJson>>(BuffDesignString);
        return BuffDataJsons;
    }

    public List<LevelBuff> LoadLevelBuffData()
    {
        string LBDesignStr = File.ReadAllText(PathConfig.LevelBuffDesignJson);
        List<LevelBuff> LBuffDataJsons = JsonConvert.DeserializeObject<List<LevelBuff>>(LBDesignStr);
        return LBuffDataJsons;
    }
    
    public List<RoleBase> LoadRoleBaseData()
    {
        string RoleDesignStr = File.ReadAllText(PathConfig.RoleDesignJson);
        List<RoleBase> RoleDesignJsons = JsonConvert.DeserializeObject<List<RoleBase>>(RoleDesignStr);
        return RoleDesignJsons;
    }
    
    public List<RollPREvent> LoadPRDesignData()
    {
        string PRDesignStr = File.ReadAllText(PathConfig.PREventDesignJson);
        List<RollPREvent> PRDesignJsons = JsonConvert.DeserializeObject<List<RollPREvent>>(PRDesignStr);
        return PRDesignJsons;
    }
    
    public List<BulletEntry> LoadBulletEntryDesignData()
    {
        string BulletEntryDesignStr = File.ReadAllText(PathConfig.BulletEntryDesignJson);
        List<BulletEntry> BulletEntryJsons = JsonConvert.DeserializeObject<List<BulletEntry>>(BulletEntryDesignStr);
        return BulletEntryJsons;
    }
    
    public List<GemJson> LoadGemData()
    {
        string GemDesignStr = File.ReadAllText(PathConfig.GemDesignJson);
        List<GemJson> GemDesignJsons = JsonConvert.DeserializeObject<List<GemJson>>(GemDesignStr);
        return GemDesignJsons;
    }
    #endregion
    
    public SaveFileJson _saveFile = new SaveFileJson();
    public UserConfig _userConfig = new UserConfig();

    //游戏状态
    public bool IsGamePause = false;

    //RandomEvent
    public List<int> RandomEvents = new List<int>();

    #region 存档相关
    public void LoadSaveFile()
    {
        string SaveFileJsonString = File.ReadAllText(PathConfig.SaveFileJson);
        _saveFile = JsonConvert.DeserializeObject<SaveFileJson>(SaveFileJsonString);

        #region Character
        MainRoleManager.Instance.Score = _saveFile.Score;
        MainRoleManager.Instance.Coins = _saveFile.Coins;
        MainRoleManager.Instance.RoomKeys = _saveFile.RoomKeys;
        for (int i = 0; i < _saveFile.SupremeCharms.Count; i++)
        {
            SupremeCharm curCharm = new SupremeCharm(_saveFile.SupremeCharms[i]);
            curCharm.GetSupremeCharmByID();
            MainRoleManager.Instance.SupremeCharms.Add(curCharm);
        }
        
        //读取Item
        for (int i = 0; i < _saveFile.UserItems.Count; i++)
        {
            ItemJson curItem = _saveFile.UserItems[i];
            BagItemManager<Item>.InitSaveFileObject(curItem,SlotType.BagSlot);
        }
        //读取Gem
        for (int i = 0; i < _saveFile.UserGems.Count; i++)
        {
            GemJson curGem = _saveFile.UserGems[i];
            BagItemManager<Gem>.InitSaveFileObject(curGem,SlotType.GemBagSlot);
        }
        
        MainRoleManager.Instance.CurBulletSpawners = _saveFile.UserBulletSpawner;
        MainRoleManager.Instance.CurBullets = _saveFile.UserCurBullets;
        MainRoleManager.Instance.CurBulletEntries = _saveFile.UserBulletEntries;
        MainRoleManager.Instance.CurStandbyBulletMats = _saveFile.UserStandbyBullet;
        MainRoleManager.Instance.CurRollPREveIDs = _saveFile.CurRollPREveIDs;
        #endregion
        
        #region Map
        List<MapSate> UserMapSate = _saveFile.UserMapSate;
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
    
    public void SaveFile()
    {
        #region Character
        _saveFile.Score = MainRoleManager.Instance.Score;
        _saveFile.Coins = MainRoleManager.Instance.Coins;
        _saveFile.RoomKeys = MainRoleManager.Instance.RoomKeys;
        _saveFile.UserBulletSpawner = MainRoleManager.Instance.CurBulletSpawners;
        _saveFile.UserCurBullets = MainRoleManager.Instance.CurBullets;
        _saveFile.UserBulletEntries = MainRoleManager.Instance.CurBulletEntries;
        List<int> SupremeCharms = new List<int>();
        foreach (var each in MainRoleManager.Instance.SupremeCharms)
            SupremeCharms.Add(each.ID);
        
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
        _saveFile.UserItems = UserItems;
        
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
        _saveFile.UserGems = UserGems;
        
        _saveFile.UserStandbyBullet = MainRoleManager.Instance.CurStandbyBulletMats;
        _saveFile.SupremeCharms = SupremeCharms;
        #endregion

        #region Map
        List<MapSate> UserMapSate = new List<MapSate>();
        MapSate curMapState = new MapSate();
        curMapState = MainRoleManager.Instance.CurMapSate;
        UserMapSate.Add(curMapState);
        _saveFile.UserMapSate = UserMapSate;
        #endregion
        
        string content01 = JsonConvert.SerializeObject(_saveFile,(Formatting) Formatting.Indented);
        File.WriteAllText(PathConfig.SaveFileJson, content01);

        SaveUserConfig();
    }

    public void LoadUserConfig()
    {
        string SaveFileJsonString = File.ReadAllText(PathConfig.UserConfigJson);
        _userConfig = JsonConvert.DeserializeObject<UserConfig>(SaveFileJsonString);

        MultiLa.Instance.CurLanguage = (MultiLaEN)_userConfig.UserLanguage;
        MSceneManager.Instance.SetScreenResolution(_userConfig.UserScreenResolution);
        MSceneManager.Instance.SetScreenMode(_userConfig.UserScreenMode);
        Debug.Log("LoadUserConfig");
    }

    public void SaveUserConfig()
    {
        _userConfig.UserLanguage = (int)MultiLa.Instance.CurLanguage;
        string content = JsonConvert.SerializeObject(_userConfig,(Formatting) Formatting.Indented);
        File.WriteAllText(PathConfig.UserConfigJson, content);
        Debug.Log("SaveUserConfig");
    }
    #endregion

    #region NewGame
    public void SetSaveFileTemplate()
    {
        _saveFile = new SaveFileJson();
        #region Character
        //................UserBulletSpawner...........................
        List<BulletJson> UserBulletSpawner = new List<BulletJson>();
        BulletJson spawner01 = new BulletJson{ID=1};
        spawner01.SyncData();
        spawner01.SpawnerCount = 5;
        UserBulletSpawner.Add(spawner01);
        
        BulletJson spawner02 = new BulletJson{ID=2};
        spawner02.SyncData();
        spawner02.SpawnerCount = 1;
        UserBulletSpawner.Add(spawner02);
        
        BulletJson spawner03 = new BulletJson{ID=3};
        spawner03.SyncData();
        spawner03.SpawnerCount = 1;
        UserBulletSpawner.Add(spawner03);
        
        //..............StandbyData.........................
        List<StandbyData> newGameSD = new List<StandbyData>();
        for (int i = 0; i < 5; i++)
            newGameSD.Add(new StandbyData(i,0));
        
        //...................Items.................................
        _saveFile.UserItems = new List<ItemJson>();
        MainRoleManager.Instance.AddItem(1);
        MainRoleManager.Instance.AddItem(2);
        //...................Gems..................................
        _saveFile.UserGems = new List<GemJson>();

        _saveFile.UserCurBullets = new List<BulletJson>();
        _saveFile.UserBulletEntries = new List<BulletEntry>();
        _saveFile.UserBulletSpawner = UserBulletSpawner;
        _saveFile.Score = 0;
        _saveFile.Coins = 1000;
        _saveFile.RoomKeys = 0;
        _saveFile.SupremeCharms = new List<int>();
        _saveFile.UserStandbyBullet = newGameSD;
        #endregion
        
        #region Map
        List<MapSate> curMapSate = new List<MapSate>();
        MapSate curMap = new MapSate();
        curMap.CurLevelID = 1;
        curMap.IsFinishedRooms = new List<int>();
        curMapSate.Add(curMap);
        _saveFile.UserMapSate = curMapSate;
        #endregion

        #region 随机事件概率
        _saveFile.CurRollPREveIDs = new List<int>();
        #endregion

        string content01 = JsonConvert.SerializeObject(_saveFile,(Formatting) Formatting.Indented);
        File.WriteAllText(PathConfig.SaveFileJson, content01);
    }
    #endregion

    #region MyRegion
    public BulletJson GetBulletDesignData(int BulletID)
    {
        BulletJson curData = null;
        foreach (BulletJson each in BulletDesignJsons)
        {
            if (each.ID == BulletID)
            {
                curData = each;
                break;
            }
        }
        return curData;
    }
    #endregion
}