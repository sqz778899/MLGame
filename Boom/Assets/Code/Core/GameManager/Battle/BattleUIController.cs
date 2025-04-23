using System;
using Unity.VisualScripting;
using UnityEngine;

public class BattleUIController
{
    [Header("一些脚本")] 
    MapManager _MapManager;
    BagRootMini BagRootMiniSC;
    EnemyMiniMapView EnemyMiniMapSC;
    BattleData _battleData;
    
    [Header("一些GUI")] 
    GameObject warReportRootGUI;
    GameObject winGUI;
    GameObject failGUI;

    public void InitFightData()
    {
        InitData();
        //初始化小地图背包界面
        BagRootMiniSC.InitData();
        //初始化小地图敌人信息界面
        EnemyMiniMapSC.Bind(_battleData.CurEnemy.Controller._data);
    }
    
    public void ShowWarReport()
    {
        InitData();
        warReportRootGUI.SetActive(true);
        warReportRootGUI.GetComponent<GUIWarReport>().SyncReport();
    }
    
    public void WarReportContinue()
    {
        InitData();
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

    public void InitWinFailGUI()
    {
        warReportRootGUI.SetActive(false);
        winGUI.SetActive(false);
        failGUI.SetActive(false);
    }
    
    #region 不关心的私有方法
    void InitData()
    {
        _MapManager ??=  BattleManager.Instance._MapManager;
        BagRootMiniSC ??=UIManager.Instance.BagUI.BagRootMiniGO.GetComponent<BagRootMini>();
        EnemyMiniMapSC ??= EternalCavans.Instance.EnemyMiniMapGO.GetComponent<EnemyMiniMapView>();
        _battleData ??= BattleManager.Instance.battleData;
        warReportRootGUI ??= EternalCavans.Instance.WarReportGO.transform.GetChild(0).gameObject;
        winGUI??= EternalCavans.Instance.WinGUI;
        failGUI??= EternalCavans.Instance.FailGUI;
    }
    #endregion
}