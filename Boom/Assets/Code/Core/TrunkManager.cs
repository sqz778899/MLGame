using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
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
    List<BuffDataJson> _buffDesignJsons;
    List<LevelBuff> _levelBuffDesignJsons;
    public List<BulletDataJson> BulletDesignJsons
    {
        get
        {
            if (_bulletDesignJsons == null)
                _bulletDesignJsons = LoadBulletData();
            return _bulletDesignJsons;
        }
    }
    
    public List<BuffDataJson> BuffDesignJsons
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
    
    public List<BulletDataJson> LoadBulletData()
    {
        string BulletDesignString = File.ReadAllText(PathConfig.BulletDesignJson);
        List<BulletDataJson> BulletDataJsons = JsonConvert.DeserializeObject<List<BulletDataJson>>(BulletDesignString);
        return BulletDataJsons;
    }
    
    public List<BuffDataJson> LoadBuffData()
    {
        string BuffDesignString = File.ReadAllText(PathConfig.BuffDesignJson);
        List<BuffDataJson> BuffDataJsons = JsonConvert.DeserializeObject<List<BuffDataJson>>(BuffDesignString);
        return BuffDataJsons;
    }

    public List<LevelBuff> LoadLevelBuffData()
    {
        string LBDesignStr = File.ReadAllText(PathConfig.LevelBuffDesignJson);
        List<LevelBuff> LBuffDataJsons = JsonConvert.DeserializeObject<List<LevelBuff>>(LBDesignStr);
        return LBuffDataJsons;
    }
    #endregion
    
    public SaveFileJson _saveFile = new SaveFileJson();
    public UserConfig _userConfig = new UserConfig();

    public List<RollProbability> GetRollProbability()
    {
        return _saveFile.UserProbabilitys;
    }

    #region 存档相关
    public void LoadSaveFile()
    {
        string SaveFileJsonString = File.ReadAllText(PathConfig.SaveFileJson);
        _saveFile = JsonConvert.DeserializeObject<SaveFileJson>(SaveFileJsonString);

        #region Character
        CharacterManager.Instance.Score = _saveFile.Score;
        CharacterManager.Instance.Gold = _saveFile.Gold;
        for (int i = 0; i < _saveFile.SupremeCharms.Count; i++)
        {
            SupremeCharm curCharm = new SupremeCharm(_saveFile.SupremeCharms[i]);
            curCharm.GetSupremeCharmByID();
            CharacterManager.Instance.SupremeCharms.Add(curCharm);
        }
        CharacterManager.Instance.CurBulletSpawners = _saveFile.UserBulletSpawner;
        CharacterManager.Instance.CurBullets = _saveFile.UserCurBullets;
        CharacterManager.Instance.CurStandbyBullets = _saveFile.UserStandbyBullet;
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
        MSceneManager.Instance.CurMapSate = curMapSate;
        #endregion

        LoadUserConfig();
    }
    
    public void SaveFile()
    {
        #region Character
        _saveFile.Score = CharacterManager.Instance.Score;
        _saveFile.Gold = CharacterManager.Instance.Gold;
        _saveFile.UserBulletSpawner = CharacterManager.Instance.CurBulletSpawners;
        _saveFile.UserCurBullets = CharacterManager.Instance.CurBullets;
        List<int> SupremeCharms = new List<int>();
        foreach (var each in CharacterManager.Instance.SupremeCharms)
            SupremeCharms.Add(each.ID);
        _saveFile.UserStandbyBullet = CharacterManager.Instance.CurStandbyBullets;
        _saveFile.SupremeCharms = SupremeCharms;
        #endregion

        #region Map
        List<MapSate> UserMapSate = new List<MapSate>();
        MapSate curMapState = new MapSate();
        curMapState = MSceneManager.Instance.CurMapSate;
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
            newGameSD.Add(new StandbyData(0,i+1));
        
        _saveFile.UserCurBullets = new List<BulletReady>();
        _saveFile.UserBulletSpawner = UserBulletSpawner;
        _saveFile.Score = 0;
        _saveFile.Gold = 10000000;
        _saveFile.SupremeCharms = new List<int>();
        _saveFile.UserStandbyBullet = newGameSD;
        #endregion
        
        #region Map
        List<MapSate> curMapSate = new List<MapSate>();
        MapSate curMap = new MapSate();
        curMap.CurMapID = 1;
        curMap.MapID = 1;
        curMap.LevelID = 1;
        curMap.IsFinishedLevels = new List<int>();
        curMapSate.Add(curMap);
        _saveFile.UserMapSate = curMapSate;
        #endregion
        
        #region RollProbability
        List<RollProbability> userProbabilitys = new List<RollProbability>();
        RollProbability ScorePro = new RollProbability();
        ScorePro.ID = 0;
        ScorePro.Probability = 0.9f;
        RollProbability Bullet01Pro = new RollProbability();
        Bullet01Pro.ID = 1;
        Bullet01Pro.Probability = 0.1f;
        userProbabilitys.Add(ScorePro);
        userProbabilitys.Add(Bullet01Pro);
        _saveFile.UserProbabilitys = userProbabilitys;
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