using UnityEngine;
using Sirenix.Utilities;


public class GameInitializer:MonoBehaviour
{
    public static GameInitializer Instance { get; private set; }
    void Awake() =>  Instance = this;

    public void InitGameData()
    {
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
        //4) 读档
        SaveManager.LoadSaveFile();
        
        
        //5)重置随机概率
        //ProbabilityService.Reset("WeaponRackLoot");
    }
}