using System;
using Unity.VisualScripting;
using UnityEngine;

public class BattleUIController
{
    BattleLogic _battleLogic => UIManager.Instance.Logic.BattleLogicSC;
    //窗口
    GameObject WarReportRootGUI => UIManager.Instance.MapUI.WarReportGO.transform.GetChild(0).gameObject;
    GameObject WinGUI => UIManager.Instance.MapUI.WinGUI;
    GameObject FailGUI => UIManager.Instance.MapUI.FailGUI;
    BagRootMini BagRootMiniSC => UIManager.Instance.BagUI.BagRootMiniGO.GetComponent<BagRootMini>();
    EnemyMiniMap EnemyMiniMapSC => UIManager.Instance.MapUI.EnemyMiniMapGO.GetComponent<EnemyMiniMap>();
    
    public BattleUIController()
    {
        //初始化小地图背包界面
        BagRootMiniSC.InitData();
        //初始化小地图敌人信息界面
        EnemyMiniMapSC.InitData(_battleLogic.CurEnemy);
        FailGUI.SetActive(false);
        WinGUI.SetActive(false);
        MainRoleManager.Instance.CurWarReport.CurWarIndex += 1;//战报系统，战斗次数+1
        EternalCavans.Instance.OnFightContinue += WarReportContinue;
    }
    public void OnDestroy()
    {
        EternalCavans.Instance.OnFightContinue -= WarReportContinue;
    }
    public void ShowWinUI()
    {
        MainRoleManager.Instance.CurWarReport.IsWin = true;
        WarReportRootGUI.SetActive(true);
        WarReportRootGUI.GetComponent<GUIWarReport>().SyncReport();
    }
    
    public void ShowFailUI()
    {
        MainRoleManager.Instance.CurWarReport.IsWin = false;
        WarReportRootGUI.SetActive(true);
        WarReportRootGUI.GetComponent<GUIWarReport>().SyncReport();
    }
    
    public void WarReportContinue()
    {
        WarReportRootGUI.SetActive(false);
        if (MainRoleManager.Instance.CurWarReport.IsWin)
        {
            WinGUI.SetActive(true);
            WinGUI.GetComponent<GUIWin>().Win(_battleLogic.CurEnemy.CurAward);
        }
        else
        {
            MainRoleManager.Instance.HP -= 1;
            FailGUI.SetActive(true);
            FailGUI.GetComponent<GUIFail>().SetHertAni();
        }
    }
}