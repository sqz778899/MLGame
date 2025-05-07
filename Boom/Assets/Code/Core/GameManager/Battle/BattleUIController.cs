using System;
using Unity.VisualScripting;
using UnityEngine;

public class BattleUIController
{
    //一些脚本
    BagRootMini BagRootMiniSC => EternalCavans.Instance.BagRootMiniSC;
    EnemyMiniMapView EnemyMiniMapSC => EternalCavans.Instance.EnemyMiniMapSC;
    BattleData _battleData => GM.Root.BattleMgr.battleData;
    Simulator _simulator => EternalCavans.Instance.SimulatorSC;
    
    //一些GUI")]
    GameObject warReportRootGUI =>EternalCavans.Instance.WarReportGO;
    GameObject winGUI=> EternalCavans.Instance.WinGUI;
    GameObject failGUI=> EternalCavans.Instance.FailGUI;
    

    public void InitFightData()
    {
        //初始化小地图背包界面
        BagRootMiniSC.InitData();
        //初始化小地图敌人信息界面
        EnemyMiniMapSC.Bind(_battleData.CurEnemy.Controller._data);
        //初始化胜率界面
        _simulator.InitData();
    }
    
    public void ShowWarReport()
    {
        warReportRootGUI.SetActive(true);
        warReportRootGUI.GetComponent<GUIWarReport>().SyncReport();
    }
    
    public void WarReportContinue()
    {
        warReportRootGUI.SetActive(false);
        if (_battleData.CurWarReport.IsWin)
        {
            winGUI.SetActive(true);
            winGUI.GetComponent<GUIWin>().
                Win(_battleData.CurEnemy.Controller.GetAward());
        }
        else
        {
            failGUI.SetActive(true);
            failGUI.GetComponent<GUIFail>().SetHertAni();
        }
    }

    public void StopSimulate() =>
        _simulator.StopSimulate();

    public void InitWinFailGUI()
    {
        warReportRootGUI.SetActive(false);
        winGUI.SetActive(false);
        failGUI.SetActive(false);
    }
}