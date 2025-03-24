using UnityEngine;

public class BattleData: ScriptableObject
{
    [Header("角色")] 
    public Enemy CurEnemy;
    public RoleInner CurRole;
    
    [Header("战报")]
    public WarReport CurWarReport;
    [Header("关卡")]
    public MapSate CurMapSate;
    public LevelMono CurLevel;
    
    [Header("Display")] 
    public float Distance;            //与敌人的距离
    public bool IsBattleEnded;        //战斗是否结束
    public bool IsAttacking;          //是否正在攻击
    public bool IsAfterAttack;           //是否被已经攻击过了
    
    public void InitFightData(EnemyMiddleData _enemyMidData,int _levelID)
    {
        CurMapSate.CurLevelID = _levelID;
        CurLevel = LevelManager.LoadLevel(_levelID);
        CurRole = PlayerManager.Instance.RoleInFightGO.GetComponent<RoleInner>();
        CurRole.InitData(CurLevel);//初始化角色数据
        CurLevel.SetEnemy(_enemyMidData);//初始化敌人属性
        CurEnemy = CurLevel.CurEnemy;
        //初始化各类数据
        IsBattleEnded = false;
        IsAttacking = false;
        IsAfterAttack = false;
        Distance = 0f;
        //初始化战报
        CurWarReport = new();
        CurWarReport.CurWarIndex += 1;
    }
    
    public void ClearData()
    {
        CurEnemy = null;
        CurRole = null;
        CurWarReport = new WarReport();
        CurMapSate = new MapSate();
        CurLevel = null;
    }
}