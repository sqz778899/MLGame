using UnityEngine;
using System;
using Sirenix.Utilities;


public class GameInitializer:MonoBehaviour
{
    public static GameInitializer Instance { get; private set; }
    void Awake() =>  Instance = this;

    public void InitGameData()
    {
        GameObject.Find("CanvasQ01").GetComponent<EternalCavans>().InitData();
        //初始化背包Slot数据
        SlotView[] views = EternalCavans.Instance.Bag.GetComponentsInChildren<SlotView>(true);
        views.ForEach(v => v.Init());
        views.ForEach(v => v.InitStep2());
        //初始化GUI相关的Manager数据
        EternalCavans.Instance.TooltipsManager.Init();
        EternalCavans.Instance.DragManager.Init();
        EternalCavans.Instance.RightClickMenuManager.Init();
    }
}