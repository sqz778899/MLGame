using UnityEngine;

public static class LevelManager
{
    public static void InitLevel(BattleLogic battleLogic)
    {
        
        LevelMono curLevel = LoadLevel();
        battleLogic.CurLevel = curLevel;
        battleLogic.CurEnemy = curLevel.CurEnemy;
        battleLogic.CurRole = UIManager.Instance.Logic.MapManagerSC.RoleInFight;
        MainRoleManager.Instance.MainRoleIns = battleLogic.CurRole.gameObject;
        MainRoleManager.Instance.WinOrFailState = WinOrFail.InLevel;
    }
    
    
    public static LevelMono LoadLevel()
    {
        Debug.Log("LoadLevel");
        GameObject levelRoot = GameObject.Find("MapFightRoot").transform.GetChild(0).gameObject;
        //清理之前的Level
        for (int i = levelRoot.transform.childCount - 1; i >=0; i--)
            GameObject.Destroy(levelRoot.transform.GetChild(i).gameObject);
        //Load Map
        GameObject levelIns = ResManager.instance.CreatInstance(
            PathConfig.GetLevelPath(MainRoleManager.Instance.CurMapSate.CurLevelID));
        levelIns.transform.SetParent(levelRoot.transform,false);
        LevelMono curlevel = levelIns.GetComponent<LevelMono>();
        curlevel.SetEnemy(MainRoleManager.Instance.CurEnemyMidData);//初始化敌人属性
        //设置角色位置
        Vector3 curRolePos = UIManager.Instance.Logic.MapManagerSC.RoleInFight.transform.position;
        UIManager.Instance.Logic.MapManagerSC.RoleInFight.transform.position = new Vector3(0, curRolePos.y, 0);
        //Set Award
        return curlevel;
    }
}
