using System;
using Unity.VisualScripting;
using UnityEngine;

public class BattleUIController
{
    [Header("一些脚本")] 
    MapManager _MapManager;
    BagRootMini BagRootMiniSC;
    EnemyMiniMap EnemyMiniMapSC;
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
        EnemyMiniMapSC.InitData(_battleData.CurEnemy);
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
            winGUI.GetComponent<GUIWin>().Win(_battleData.CurEnemy.CurAward);
        }
        else
        {
            failGUI.SetActive(true);
            failGUI.GetComponent<GUIFail>().SetHertAni();
        }
    }
    
    #region 不关心的私有方法
    void InitData()
    {
        _MapManager ??= UIManager.Instance.Logic.MapManagerSC;
        BagRootMiniSC ??=UIManager.Instance.BagUI.BagRootMiniGO.GetComponent<BagRootMini>();
        EnemyMiniMapSC ??=UIManager.Instance.MapUI.EnemyMiniMapGO.GetComponent<EnemyMiniMap>();
        _battleData ??= BattleManager.Instance.battleData;
        warReportRootGUI ??= UIManager.Instance.MapUI.WarReportGO.transform.GetChild(0).gameObject;
        winGUI??=UIManager.Instance.MapUI.WinGUI;
        failGUI??=UIManager.Instance.MapUI.FailGUI;
    }
    #endregion
}