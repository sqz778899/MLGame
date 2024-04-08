using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;
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
    public List<BulletDataJson> BulletDesignJsons
    {
        get
        {
            if (_bulletDesignJsons == null)
                _bulletDesignJsons = LoadBulletData();
            return _bulletDesignJsons;
        }
    }
    
    
    
    public List<BulletDataJson> LoadBulletData()
    {
        string BulletDesignString = File.ReadAllText(PathConfig.BulletDesignJson);
        List<BulletDataJson> BulletDataJsons = JsonConvert.DeserializeObject<List<BulletDataJson>>(BulletDesignString);
        return BulletDataJsons;
    }
    #endregion
    
    
    public SaveFileJson _saveFile = new SaveFileJson();
    
    public void LoadSaveFile()
    {
        string SaveFileJsonString = File.ReadAllText(PathConfig.SaveFileJson);
        _saveFile = JsonConvert.DeserializeObject<SaveFileJson>(SaveFileJsonString);

        #region Character
        CharacterManager.Instance.Score = _saveFile.Score;
        CharacterManager.Instance.Gold = _saveFile.Gold;
        for (int i = 0; i < _saveFile.SupremeCharms.Count; i++)
        {
            SupremeCharm curCharm = new SupremeCharm();
            curCharm.ID = _saveFile.SupremeCharms[i];
            curCharm.GetSupremeCharmByID();
            CharacterManager.Instance.SupremeCharms.Add(curCharm);
        }
        CharacterManager.Instance.CurStandbyBullets = _saveFile.UserStandbyBullet;
        CharacterManager.Instance.InitData();
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
    }
    
    public void SaveFile()
    {
        #region Character
        _saveFile.BagData = CharacterManager.Instance.BagData.SetDataJson();
        _saveFile.Score = CharacterManager.Instance.Score;
        _saveFile.Gold = CharacterManager.Instance.Gold;
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
    }

    public List<RollProbability> GetRollProbability()
    {
        return _saveFile.UserProbabilitys;
    }

    #region NewGame
    public void SetSaveFileTemplate()
    {
        _saveFile = new SaveFileJson();
        #region Character
        BagDataJson bagDataJson = new BagDataJson();
        List<SingleSlot> bagSlots = new List<SingleSlot>();
        bagDataJson.bagSlots = bagSlots;
        _saveFile.BagData = bagDataJson;

        for (int i = 0; i < 12; i++)
        {
            SingleSlot temSlot = new SingleSlot();
            temSlot.slotID = i + 1;
            switch (i)
            {
                case 0:
                    temSlot.bulletCount = 5;
                    temSlot.bulletID = 1;
                    break;
                case 1:
                    temSlot.bulletCount = 1;
                    temSlot.bulletID = 2;
                    break;
                case 2:
                    temSlot.bulletCount = 1;
                    temSlot.bulletID = 3;
                    break;
                default:
                    temSlot.bulletCount = 0;
                    temSlot.bulletID = 0;
                    break;
            }
            bagSlots.Add(temSlot);
        }
        bagDataJson.slotRole01 = 0;
        bagDataJson.slotRole02 = 0;
        bagDataJson.slotRole03 = 0;
        bagDataJson.slotRole04 = 0;
        bagDataJson.slotRole05 = 0;

        _saveFile.Score = 0;
        _saveFile.Gold = 10000000;
        _saveFile.SupremeCharms = new List<int>();
        
        List<StandbyData> newGameSD = new List<StandbyData>();
        for (int i = 0; i < 5; i++)
            newGameSD.Add(new StandbyData(0,i+1));
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
}