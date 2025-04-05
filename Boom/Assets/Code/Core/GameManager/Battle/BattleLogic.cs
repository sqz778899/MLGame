using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class BattleLogic : MonoBehaviour
{
    [Header("状态机")] 
    IFightState currentState;
    
    [Header("Camera")] 
    public bool IsBeginCameraMove;    //是否开始摄像机移动,外部唯一关心参数
    public BattleCameraController _battleCameraController;
    BattleData _battleData;
    
    void Start()
    {
        TrunkManager.Instance.IsGamePause = false;
        _battleData = BattleManager.Instance.battleData;
    }

    void Update()
    {
        if (_battleData.IsBattleEnded) return;
        
        // 每帧调用当前状态的更新逻辑
        currentState?.Update();
    }

    void FixedUpdate()
    {
        //摄像机跟随子弹命中敌人动画
        _battleCameraController.HandleCameraFollow();
    }

    public void InitFightData()
    {
        //初始化状态机
        currentState = null;
        ChangeState(new InLevelState());
    }
   
    //切换状态机
    public void ChangeState(IFightState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    //判定战斗是否结束
    public bool IsBattleOver()
    {
        if (_battleData.CurEnemy.EState == EnemyState.dead)
            return true;
        
        if (!_battleData.IsAfterAttack)//如果还没有攻击过,则不会结束
            return false;
        
        bool isOver = _battleData.CurRole.Bullets.All(each => each == null);
        return isOver;
    }
    
    //判定是否敌人已经死亡
    public bool CurrentEnemyIsDead() => _battleData.CurEnemy != null && _battleData.CurEnemy.EState == EnemyState.dead;
}
