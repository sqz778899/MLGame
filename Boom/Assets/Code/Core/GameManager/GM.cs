using System;
using UnityEngine;


public class GM: MonoBehaviour
{
    public bool IsSkipStorylineMode;
    [Header("测试模式")]
    public bool IsTestMode;
    public int TestMapID;
    
    [Header("需要依赖的资产")]
    public QuestDatabaseOBJ questDatabase;
    public GlobalColorPalette globalColorPalette;//全局色板资产
    
    public PlayerManager PlayerMgr { get; private set; }
    public QuestManager QuestMgr { get; private set; }
    public InventoryManager InventoryMgr { get; private set; }
    public BattleManager BattleMgr{ get; private set; }
    public GlobalTicker GlobalTickerMgr { get; private set; }
    public HotkeyManager HotkeyMgr { get; private set; }
    
    public StorylineSystem StorylineSys{ get; private set; }

    public GameInitializer Initializer { get; private set; }
    
    #region 单例的加载卸载
    public static GM Root { get; private set; }
    
    void Awake()
    {
        if (Root == null)
        {
            Root = this;
            DontDestroyOnLoad(gameObject);
            // 添加核心管理器组件
            PlayerMgr = gameObject.AddComponent<PlayerManager>();
            PlayerMgr.InitData();
            QuestMgr = gameObject.AddComponent<QuestManager>();
            
            InventoryMgr = gameObject.AddComponent<InventoryManager>();
            BattleMgr = gameObject.AddComponent<BattleManager>();
            GlobalTickerMgr = gameObject.AddComponent<GlobalTicker>();
            HotkeyMgr = gameObject.AddComponent<HotkeyManager>();
            StorylineSys = gameObject.AddComponent<StorylineSystem>();
            
            Initializer = gameObject.AddComponent<GameInitializer>();
            Initializer.InitGameData();//初始化全局需要手动初始化的数据
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region 鼠标设置
    [SerializeField]
    private Texture2D cursorTexture; // 鼠标图标
    [SerializeField] private Vector2 hotspot = Vector2.zero; // 点击热点的偏移（例如箭头尖的位置）

    void Start() => Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
    #endregion
}