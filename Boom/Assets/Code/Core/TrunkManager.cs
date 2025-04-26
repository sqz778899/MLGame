using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public class TrunkManager: ScriptableObject
{
    #region 策划数据
    List<BulletJson> _bulletDesignJsons;
    List<ItemJson> _itemDesignJsons;
    List<GemJson> _gemDesignJsons;
    Dictionary<string,List<DiaSingle>> _dialogueDesignJsons;//对话相关
    List<QuestJson> _questDesignJsons;
    List<TalentJson> _talentDesignJsons;
    List<DropTableJson> _dropTableDesignJsons;
    
    public List<BulletJson> BulletDesignJsons => _bulletDesignJsons ??= LoadBulletData();
    public List<BulletJson> LoadBulletData() => 
        JsonConvert.DeserializeObject<List<BulletJson>>(File.ReadAllText(PathConfig.BulletDesignJson));
    public BulletJson GetBulletJson(int ID)=>BulletDesignJsons.FirstOrDefault(each => each.ID == ID) ?? new BulletJson();
    
    public List<ItemJson> ItemDesignJsons => _itemDesignJsons ??= LoadItemData();
    public List<ItemJson> LoadItemData()=>
        JsonConvert.DeserializeObject<List<ItemJson>>(File.ReadAllText(PathConfig.ItemDesignJson));
    public ItemJson GetItemJson(int ID)=>ItemDesignJsons.FirstOrDefault(each => each.ID == ID) ?? new ItemJson();
    public List<GemJson> GemDesignJsons => _gemDesignJsons ??= LoadGemData();
    public List<GemJson> LoadGemData() => 
        JsonConvert.DeserializeObject<List<GemJson>>(File.ReadAllText(PathConfig.GemDesignJson));
    public GemJson GetGemJson(int ID)=>GemDesignJsons.FirstOrDefault(each => each.ID == ID) ?? new GemJson();

    public Dictionary<string, List<DiaSingle>> DialogueDesignJsons => _dialogueDesignJsons ??= LoadDialogueDesignData();
    public Dictionary<string, List<DiaSingle>> LoadDialogueDesignData()=>
        JsonConvert.DeserializeObject<Dictionary<string, List<DiaSingle>>>(File.ReadAllText(PathConfig.DialogueDesignJson));
    public List<DiaSingle> GetDialogueJson(string ID)=>DialogueDesignJsons[ID];
    
    public List<QuestJson> QuestDesignJsons => _questDesignJsons ??= LoadQuestData();
    public List<QuestJson> LoadQuestData()=>
        JsonConvert.DeserializeObject<List<QuestJson>>(File.ReadAllText(PathConfig.QuestDesignJson));
    public QuestJson GetQuestJson(int ID)=>QuestDesignJsons.FirstOrDefault(each => each.ID == ID) ?? new QuestJson();
    
    public List<TalentJson> TalentDesignJsons => _talentDesignJsons ??= LoadTalentData();
    public List<TalentJson> LoadTalentData()=>
        JsonConvert.DeserializeObject<List<TalentJson>>(File.ReadAllText(PathConfig.TalentDesignJson));
    public TalentJson GetTalentJson(int ID)=>TalentDesignJsons.FirstOrDefault(each => each.ID == ID) ?? new TalentJson();
    
    public List<DropTableJson> DropTableDesignJsons => _dropTableDesignJsons ??= LoadDropTableData();
    public List<DropTableJson> LoadDropTableData()=>
        JsonConvert.DeserializeObject<List<DropTableJson>>(File.ReadAllText(PathConfig.DropedDesignJson));
    public List<DropTableJson> GetDropTableJson() => DropTableDesignJsons;
    
    public void ForceRefresh()
    {
        _bulletDesignJsons = LoadBulletData();
        _itemDesignJsons = LoadItemData();
        _gemDesignJsons = LoadGemData();
        _dialogueDesignJsons = LoadDialogueDesignData();
        _questDesignJsons = LoadQuestData();
        _talentDesignJsons = LoadTalentData();
        _dropTableDesignJsons = LoadDropTableData();
    }
    #endregion
    
    public SaveFileJson _saveFile = new SaveFileJson();
    public UserConfig _userConfig = new UserConfig();

    //游戏状态
    public bool IsGamePause = false;
    
    #region NewGame
    public void SetSaveFileTemplate()
    {
        _saveFile = new SaveFileJson();
        #region Character
        _saveFile.MaxHP = 3;
        _saveFile.HP = 3;
        //................UserBulletSpawner...........................
        List<BulletBaseSaveData> UserBulletSpawner = new List<BulletBaseSaveData>();
        BulletBaseSaveData spawner01 = new BulletBaseSaveData(1,1,SlotType.SpawnnerSlot,1);
        spawner01.SpawnerCount = 0;
        UserBulletSpawner.Add(spawner01);
        
        BulletBaseSaveData spawner02 = new BulletBaseSaveData(2,2,SlotType.SpawnnerSlot,1);
        spawner02.SpawnerCount = 0;
        UserBulletSpawner.Add(spawner02);
        
        BulletBaseSaveData spawner03 = new BulletBaseSaveData(3,3,SlotType.SpawnnerSlot,0);
        spawner03.SpawnerCount = 0;
        UserBulletSpawner.Add(spawner03);
        
        //...................Items.................................
        _saveFile.UserItems = new List<ItemSaveData>();
        //...................Gems..................................
        _saveFile.UserGems = new List<GemBaseSaveData>();
        //...................子弹槽状态..............................
        _saveFile.UserBulletSlotLockedState = new Dictionary<int, bool>
        { {0, true},{1, true},{2,false},{3,false},{4,false} };
      
        _saveFile.UserCurBullets = new List<BulletBaseSaveData>();
        _saveFile.UserBulletSpawner = UserBulletSpawner;
        _saveFile.Score = 0;
        _saveFile.Coins = 0;
        _saveFile.RoomKeys = 0;
        _saveFile.MagicDust = 0;
        //_saveFile.UserStandbyBullet = newGameSD;
        //.................新手教程完成情况........................
        _saveFile.UserTutorial = new TutorialCompletionStatus();
        //.................天赋情况........................
        _saveFile.UserTalents = new List<TalentData>();
        foreach (var each in TalentDesignJsons)
        {
            TalentData newTalent = new TalentData(each.ID);
            _saveFile.UserTalents.Add(newTalent);
        }
        #endregion
        
        #region Quest
        int questDesignCount = 3;
        List<QuestSaveData> UserQuests = new List<QuestSaveData>();
        for (int i = 0; i < questDesignCount; i++)
        {
            QuestSaveData newQuest = new QuestSaveData(new Quest(i+1));
            UserQuests.Add(newQuest);
        }
        _saveFile.UserQuests = UserQuests;
        _saveFile.UserMainStoryProgress = 0;
        _saveFile.UserStorylineNodesState = new List<StorylineNodeStateData>();
        #endregion

        string content01 = JsonConvert.SerializeObject(_saveFile,(Formatting) Formatting.Indented);
        File.WriteAllText(PathConfig.SaveFileJson, content01);
    }
    
    public void SetSaveFileFiveSlotsTemplate()
    {
        _saveFile = new SaveFileJson();
        #region Character
        _saveFile.MaxHP = 3;
        _saveFile.HP = 3;
        //................UserBulletSpawner...........................
        List<BulletBaseSaveData> UserBulletSpawner = new List<BulletBaseSaveData>();
        BulletBaseSaveData spawner01 = new BulletBaseSaveData(1,1,SlotType.SpawnnerSlot,1);
        spawner01.SpawnerCount = 5;
        UserBulletSpawner.Add(spawner01);
        
        BulletBaseSaveData spawner02 = new BulletBaseSaveData(2,2,SlotType.SpawnnerSlot,1);
        spawner02.SpawnerCount = 0;
        UserBulletSpawner.Add(spawner02);
        
        BulletBaseSaveData spawner03 = new BulletBaseSaveData(3,3,SlotType.SpawnnerSlot,0);
        spawner03.SpawnerCount = 0;
        UserBulletSpawner.Add(spawner03);
        
        //...................Items.................................
        _saveFile.UserItems = new List<ItemSaveData>();
        //...................Gems..................................
        _saveFile.UserGems = new List<GemBaseSaveData>();
        //...................子弹槽状态..............................
        _saveFile.UserBulletSlotLockedState = new Dictionary<int, bool>
        { {0, true},{1, true},{2,true},{3,true},{4,true} };
      
        _saveFile.UserCurBullets = new List<BulletBaseSaveData>();
        _saveFile.UserBulletSpawner = UserBulletSpawner;
        _saveFile.Score = 0;
        _saveFile.Coins = 0;
        _saveFile.RoomKeys = 0;
        _saveFile.MagicDust = 9999;
        //_saveFile.UserStandbyBullet = newGameSD;
        //.................新手教程完成情况........................
        _saveFile.UserTutorial = new TutorialCompletionStatus();
        
        _saveFile.UserTalents = new List<TalentData>();
        foreach (var each in TalentDesignJsons)
        {
            TalentData newTalent = new TalentData(each.ID);
            _saveFile.UserTalents.Add(newTalent);
        }
        #endregion
        
        #region Quest
        int questDesignCount = 3;
        List<QuestSaveData> UserQuests = new List<QuestSaveData>();
        for (int i = 0; i < questDesignCount; i++)
        {
            QuestSaveData newQuest = new QuestSaveData(new Quest(i+1));
            UserQuests.Add(newQuest);
        }
        _saveFile.UserQuests = UserQuests;
        _saveFile.UserMainStoryProgress = 0;
        _saveFile.UserStorylineNodesState = new List<StorylineNodeStateData>();
        #endregion

        string content01 = JsonConvert.SerializeObject(_saveFile,(Formatting) Formatting.Indented);
        File.WriteAllText(PathConfig.SaveFileJson, content01);
    }
    
    public void SetSaveFileTest()
    {
        _saveFile = new SaveFileJson();
        #region Character
        _saveFile.MaxHP = 3;
        _saveFile.HP = 3;
        //................UserBulletSpawner...........................
        List<BulletBaseSaveData> UserBulletSpawner = new List<BulletBaseSaveData>();
        BulletBaseSaveData spawner01 = new BulletBaseSaveData(1,1,SlotType.SpawnnerSlot,1);
        UserBulletSpawner.Add(spawner01);
        
        BulletBaseSaveData spawner02 = new BulletBaseSaveData(2,2,SlotType.SpawnnerSlot,1);
        spawner02.SpawnerCount = 1;
        UserBulletSpawner.Add(spawner02);
        
        BulletBaseSaveData spawner03 = new BulletBaseSaveData(3,3,SlotType.SpawnnerSlot,0);
        spawner03.SpawnerCount = 0;
        UserBulletSpawner.Add(spawner03);
        
        //...................Items.................................
        _saveFile.UserItems = new List<ItemSaveData>();
        //...................Gems..................................
        _saveFile.UserGems = new List<GemBaseSaveData>();
        //...................子弹槽状态..............................
        _saveFile.UserBulletSlotLockedState = new Dictionary<int, bool>
        { {0, true},{1, true},{2,false},{3,false},{4,false} };
      
        _saveFile.UserCurBullets = new List<BulletBaseSaveData>();
        _saveFile.UserBulletSpawner = UserBulletSpawner;
        _saveFile.Score = 0;
        _saveFile.Coins = 0;
        _saveFile.RoomKeys = 0;
        _saveFile.MagicDust = 9999;
        //.................新手教程完成情况........................
        _saveFile.UserTutorial = new TutorialCompletionStatus();
        
        _saveFile.UserTalents = new List<TalentData>();
        foreach (var each in TalentDesignJsons)
        {
            TalentData newTalent = new TalentData(each.ID);
            _saveFile.UserTalents.Add(newTalent);
        }
        #endregion
        
        #region Quest
        int questDesignCount = 3;
        List<QuestSaveData> UserQuests = new List<QuestSaveData>();
        for (int i = 0; i < questDesignCount; i++)
        {
            QuestSaveData newQuest = new QuestSaveData(new Quest(i+1));
            UserQuests.Add(newQuest);
        }
        _saveFile.UserQuests = UserQuests;
        _saveFile.UserMainStoryProgress = 0;
        _saveFile.UserStorylineNodesState = new List<StorylineNodeStateData>();
        #endregion

        string content01 = JsonConvert.SerializeObject(_saveFile,(Formatting) Formatting.Indented);
        File.WriteAllText(PathConfig.SaveFileJson, content01);
    }
    #endregion
    
    #region 单例
    static TrunkManager s_instance;
    public static TrunkManager Instance
    {
        get
        {
            if (s_instance == null)
                s_instance = ResManager.instance.GetAssetCache<TrunkManager>(PathConfig.TrunkManagerOBJ);
            if (s_instance != null) { DontDestroyOnLoad(s_instance); }
            return s_instance;
        }
    }
    
    void OnEnable()
    {
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(this);
        }
    }
    #endregion
}