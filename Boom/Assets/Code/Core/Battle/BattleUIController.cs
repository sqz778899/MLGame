using UnityEngine;

public class BattleUIController
{
    BattleLogic _battleLogic;
    //窗口
    GameObject WarReportRootGUI;
    GameObject WinGUI;
    GameObject FailGUI;
    BagRootMini BagRootMiniSC;
    EnemyMiniMap EnemyMiniMapSC;
    
    public BattleUIController(BattleLogic battleLogic)
    {
        _battleLogic = battleLogic;
        WarReportRootGUI = _battleLogic.WarReportRootGUI;
        WinGUI = _battleLogic.WinGUI;
        FailGUI = _battleLogic.FailGUI;
        //初始化小地图背包界面
        BagRootMiniSC = _battleLogic.BagRootMiniSC;
        BagRootMiniSC.InitData();
        //初始化小地图敌人信息界面
        EnemyMiniMapSC = _battleLogic.EnemyMiniMapSC;
        EnemyMiniMapSC.InitData(_battleLogic.CurEnemy);
        FailGUI.SetActive(false);
        WinGUI.SetActive(false);
        MainRoleManager.Instance.CurWarReport.CurWarIndex += 1;//战报系统，战斗次数+1
    }
    
    public void ShowWinUI()
    {
        MainRoleManager.Instance.CurWarReport.IsWin = true;
        WarReportRootGUI.SetActive(true);
        UIManager.Instance.WarReportGO.GetComponent<GUIWarReport>().SyncReport();
    }
    
    public void ShowFailUI()
    {
        MainRoleManager.Instance.CurWarReport.IsWin = false;
        WarReportRootGUI.SetActive(true);
        UIManager.Instance.WarReportGO.GetComponent<GUIWarReport>().SyncReport();
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