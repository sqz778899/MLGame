using System.Collections.Generic;
using System.IO;
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
    List<BulletDataJson> _bulletDesignJsons;
    List<ItemJson> _itemDesignJsons;
    List<TalentDataJson> _buffDesignJsons;
    List<LevelBuff> _levelBuffDesignJsons;
    List<RoleBase> _roleDesignJsons;
    List<RollPREvent> _prDesignJsons;
    List<BulletEntry> _bulletEntryDesignJsons;
    public List<BulletDataJson> BulletDesignJsons
    {
        get
        {
            if (_bulletDesignJsons == null)
                _bulletDesignJsons = LoadBulletData();
            return _bulletDesignJsons;
        }
    }
    
    public List<ItemJson> ItemDesignJsons
    {
        get
        {
            if (_itemDesignJsons == null)
                _itemDesignJsons = LoadItemData();
            return _itemDesignJsons;
        }
    }
    
    public List<TalentDataJson> BuffDesignJsons
    {
        get
        {
            if (_buffDesignJsons == null)
                _buffDesignJsons = LoadBuffData();
            return _buffDesignJsons;
        }
    }

    public List<LevelBuff> LevelBuffDesignJsons
    {
        get
        {
            if (_levelBuffDesignJsons == null)
                _levelBuffDesignJsons = LoadLevelBuffData();
            return _levelBuffDesignJsons;
        }
    }
    
    public List<RoleBase> RoleDesignJsons
    {
        get
        {
            if (_roleDesignJsons == null)
                _roleDesignJsons = LoadRoleBaseData();
            return _roleDesignJsons;
        }
    }
    
    public List<RollPREvent> PRDesignJsons
    {
        get
        {
            _prDesignJsons = LoadPRDesignData();
            return _prDesignJsons;
        }
    }

    public List<BulletEntry> BulletEntryDesignJsons
    {
        get
        {
            _bulletEntryDesignJsons = LoadBulletEntryDesignData();
            return _bulletEntryDesignJsons;
        }
    }
    
    public List<BulletDataJson> LoadBulletData()
    {
        string BulletDesignString = File.ReadAllText(PathConfig.BulletDesignJson);
        List<BulletDataJson> BulletDataJsons = JsonConvert.DeserializeObject<List<BulletDataJson>>(BulletDesignString);
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
        MainRoleManager.Instance.Gold = _saveFile.Gold;
        for (int i = 0; i < _saveFile.SupremeCharms.Count; i++)
        {
            SupremeCharm curCharm = new SupremeCharm(_saveFile.SupremeCharms[i]);
            curCharm.GetSupremeCharmByID();
            MainRoleManager.Instance.SupremeCharms.Add(curCharm);
        }
        
        //读取Item
        for (int i = 0; i < _saveFile.UserItems.Count; i++)
        {
            Item curItem = _saveFile.UserItems[i];
            ItemManager.InitSaveFileItem(curItem);
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
            if (eachMapSate.CurMapID == 1)
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
        _saveFile.Gold = MainRoleManager.Instance.Gold;
        _saveFile.UserBulletSpawner = MainRoleManager.Instance.CurBulletSpawners;
        _saveFile.UserCurBullets = MainRoleManager.Instance.CurBullets;
        _saveFile.UserBulletEntries = MainRoleManager.Instance.CurBulletEntries;
        List<int> SupremeCharms = new List<int>();
        foreach (var each in MainRoleManager.Instance.SupremeCharms)
            SupremeCharms.Add(each.ID);
        
        //存储Item数据信息
        List<Item> UserItems = new List<Item>();
        foreach (var each in MainRoleManager.Instance.CurItems)
            UserItems.Add(each);
        foreach (var each in MainRoleManager.Instance.BagItems)
            UserItems.Add(each);
        _saveFile.UserItems = UserItems;

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
        List<BulletSpawner> UserBulletSpawner = new List<BulletSpawner>();
        UserBulletSpawner.Add(new BulletSpawner(1,5));
        UserBulletSpawner.Add(new BulletSpawner(2,1));
        UserBulletSpawner.Add(new BulletSpawner(3,1));
        
        //..............StandbyData.........................
        List<StandbyData> newGameSD = new List<StandbyData>();
        for (int i = 0; i < 5; i++)
            newGameSD.Add(new StandbyData(i,0));
        
        //...................Items.................................
        _saveFile.UserItems = new List<Item>();
        MainRoleManager.Instance.AddItem(1);
        MainRoleManager.Instance.AddItem(2);

        _saveFile.UserCurBullets = new List<BulletReady>();
        _saveFile.UserBulletEntries = new List<BulletEntry>();
        _saveFile.UserBulletSpawner = UserBulletSpawner;
        _saveFile.Score = 0;
        _saveFile.Gold = 1000;
        _saveFile.SupremeCharms = new List<int>();
        _saveFile.UserStandbyBullet = newGameSD;
        #endregion
        
        #region Map
        List<MapSate> curMapSate = new List<MapSate>();
        MapSate curMap = new MapSate();
        curMap.CurMapID = 1;
        curMap.CurMapNodeID = 1;
        curMap.IsFinishedRooms = new List<int>();
        curMap.IsFinishedMapNodes = new List<int>();
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
    public BulletDataJson GetBulletDesignData(int BulletID)
    {
        BulletDataJson curData = null;
        foreach (BulletDataJson each in BulletDesignJsons)
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