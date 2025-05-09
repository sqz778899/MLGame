﻿using System.Collections.Generic;
using UnityEngine;

public class BattleData: ScriptableObject
{
    [Header("角色")] 
    public Enemy CurEnemy;
    public List<Shield> CurShields;
    public RoleInner CurRole;
    public IDamageable CurDamageable;//实时更新的当前命中目标
    public BulletData CurAttackBullet;//实时更新的当前命中的子弹
    
    [Header("战报")]
    public WarReport CurWarReport;
    [Header("关卡")]
    //public MapSate CurMapSate;
    public LevelMono CurLevel;

    [Header("Buff")] 
    public BattleTempBuffManager BattleTempBuffMgr;
    public BattleTempState BattleStateCash; //临时记录状态的缓存，用来处理条件和目标不一致的Buff
    
    [Header("Display")] 
    public float Distance;            //与敌人的距离
    public bool IsBattleEnded;        //战斗是否结束
    public bool IsAttacking;          //是否正在攻击
    public bool IsAfterAttack;           //是否被已经攻击过了
    
    public void InitFightData(EnemyConfigData _enemyConfig,int _levelID)
    {
        //CurMapSate.CurLevelID = _levelID;
        BattleTempBuffMgr = new BattleTempBuffManager();
        BattleStateCash = new BattleTempState();
        GM.Root.InventoryMgr._BulletInvData.OnBulletDataChanged +=
            BattleTempBuffMgr.ApplyAll;//子弹数据变化时，应用所有临时buff
        CurLevel = LevelManager.LoadLevel(_levelID);
        CurRole = GM.Root.PlayerMgr.RoleInFightSC;
        GM.Root.InventoryMgr.CreateAllBulletToFight();//初始化局内子弹
        CurRole.InitData(CurLevel);//初始化角色数据
        CurLevel.SetEnemy(_enemyConfig);//初始化敌人属性
        CurEnemy = CurLevel.CurEnemy;
        CurShields = CurEnemy.GetComponent<EnemyView>().Shields;
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
        GM.Root.InventoryMgr._BulletInvData.OnBulletDataChanged -=
            BattleTempBuffMgr.ApplyAll;//注销事件注册
        BattleTempBuffMgr.Clear();
        CurEnemy = null;
        CurRole = null;
        CurWarReport = new WarReport();
        CurLevel = null;
    }
}