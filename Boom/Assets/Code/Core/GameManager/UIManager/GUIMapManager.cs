using UnityEngine;

public class GUIMapManager
{
    public GameObject GUIMapRootGO{ get; private set; }//地图根GUI
    public GameObject WarReportGO{ get; private set; }//战报根GUI
    public GameObject WinGUI{ get; private set; }//胜利GUI
    public GameObject FailGUI{ get; private set; }//失败GUI
    public GameObject EnemyMiniMapGO{ get; private set; }//敌人预览小地图
    public GameObject ShopRoot{ get; private set; }//商店根GUI
    public GameObject RewardRoot{ get; private set; }//奖励根GUI
    
    public GameObject GUIFightMapRootGO{ get; private set; }//战斗地图根GUI
    
    public GUIMapManager()
    {
        GUIMapRootGO = EternalCavans.Instance.GUIMapRootGO;
        WarReportGO = EternalCavans.Instance.WarReportGO;
        ShopRoot = EternalCavans.Instance.ShopRoot;
        RewardRoot = EternalCavans.Instance.RewardRoot;
        GUIFightMapRootGO = EternalCavans.Instance.GUIFightMapRootGO;
        WinGUI = EternalCavans.Instance.WinGUI;
        FailGUI = EternalCavans.Instance.FailGUI;
        EnemyMiniMapGO = EternalCavans.Instance.EnemyMiniMapGO;
    }
}