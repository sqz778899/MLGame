using UnityEngine;
using Sirenix.Utilities;
using TMPro;


public class GameInitializer:MonoBehaviour
{
    public static GameInitializer Instance { get; private set; }
    void Awake() =>  Instance = this;

    public void InitGameData()
    { 
        //0)是否是测试关卡
        GM.Root.QuestMgr.IsTestMode = GM.Root.IsTestMode;
        GM.Root.QuestMgr.TestMapID = GM.Root.TestMapID;
        //1）初始化GUI相关的Manager数据
        GameObject.Find("CanvasQ01").GetComponent<EternalCavans>().InitData();
        EternalCavans.Instance.TooltipsManager.Init();
        EternalCavans.Instance.DragManager.Init();
        EternalCavans.Instance.RightClickMenuManager.Init();
        //2）初始化背包Slot数据
        SlotView[] views = EternalCavans.Instance.Bag.GetComponentsInChildren<SlotView>(true);
        views.ForEach(v => v.Init());
        views.ForEach(v => v.InitStep2());
        //3) 初始化全局Manager依赖UI的容器数据
        BulletSlotView[] bulletViews = EternalCavans.Instance.
            Bag.GetComponentsInChildren<BulletSlotView>(true);
        BulletInnerSlotView[] bulletInnerViews = EternalCavans.Instance.
            Bag.GetComponentsInChildren<BulletInnerSlotView>(true);
        GM.Root.InventoryMgr.InitStep2(bulletViews,bulletInnerViews);
        //4)初始化任务管理器
        GM.Root.QuestMgr.questDatabase = GM.Root.questDatabase;
        GM.Root.QuestMgr.InitData();
        //5) 读档
        SaveManager.LoadSaveFile();
        GM.Root.InventoryMgr.InitStep3();//初始化需要读档之后的信息
        
        //5)初始化各种静态类需要的配置
        //............跳字静态类...............
        FloatingTextFactory.DamageFontAsset =
            ResManager.instance.GetAssetCache<TMP_FontAsset>(PathConfig.DamageFontAsset);
        FloatingTextFactory.DamageFontMaterial =
            ResManager.instance.GetAssetCache<Material>(PathConfig.DamageFontMaterial);
        FloatingTextFactory.MapHintFontAsset =
            ResManager.instance.GetAssetCache<TMP_FontAsset>(PathConfig.MapHintFontAsset);
        //............道具掉落静态类...............
        DropTableService.LoadFromJson();
        //5)重置随机概率
        //ProbabilityService.Reset("WeaponRackLoot");
        
        //99)
        UIManager.Instance.InitStartGame();
    }
}