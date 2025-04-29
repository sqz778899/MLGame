using DG.Tweening;
using UnityEngine;

//战斗状态机
#region 接口
public interface IFightState
{
    // 进入状态时调用
    void Enter();
    // 每帧更新状态时调用
    void Update();
    // 离开状态时调用
    void Exit();
}
#endregion

#region 战斗中状态机
public class InLevelState : IFightState
{
    BattleLogic _battleLogic;
    BattleData _battleData;
    Enemy _curEnemy;
    RoleInner _curRole;

    public InLevelState()
    {
        _battleLogic = BattleManager.Instance.battleLogic;
        _battleData = BattleManager.Instance.battleData;
        _curEnemy = _battleData.CurEnemy;
        _curRole = _battleData.CurRole;
    }

    public void Enter() => GM.Root.InventoryMgr.
        _ItemEffectMrg.Trigger(ItemTriggerTiming.OnBattleStart);
    
    public void Update()
    {
        // 检查战斗是否结束：子弹用完或敌人死亡
        if (_battleLogic.IsBattleOver())
        {
            _battleData.IsBattleEnded = true;
            GM.Root.InventoryMgr.SyncMainBulletSlot();//战斗结束时，子弹槽的子弹都要回到背包
            // 根据敌人状态切换到胜利或失败状态
            if (_battleLogic.CurrentEnemyIsDead())
                _battleLogic.ChangeState(new WinState());
            else
                _battleLogic.ChangeState(new FailState());
        }
        
        UpdateDistance();//实时计算与敌人的距离
        HandleInput();//开火
    }
    
    //开火
    void HandleInput()
    {
        if (UIManager.Instance.IsLockedClick) return;
        
        if (Input.GetKeyDown(KeyCode.Space) && !_battleData.IsAttacking)
        {
            GM.Root.InventoryMgr._ItemEffectMrg.Trigger(ItemTriggerTiming.OnBulletFire);
            
            _battleData.IsAttacking = true;
            _battleData.IsAfterAttack = true;
            _curRole.Fire();
            _battleLogic.IsBeginCameraMove = true;
        }
    }
    
    //实时计算与敌人的距离
    void UpdateDistance()
    {
        if(_curEnemy == null) return;
        
        _battleData.Distance = Vector2.Distance(_curEnemy.transform.position,
            _curRole.transform.position);
    }
    
    public void Exit()
    {
        Debug.Log("退出战斗中状态");
    }
}
#endregion

#region 胜利状态机
public class WinState : IFightState
{
    public WinState() {}
    
    public void Enter()
    {
        Debug.Log("进入胜利状态");
        DOVirtual.DelayedCall(3f, () =>
            { BattleManager.Instance.ShowWarReport(true); });
    }
    
    public void Update()
    {
        // 可在这里添加胜利后需要持续处理的逻辑
    }
    
    public void Exit()
    {
        Debug.Log("退出胜利状态");
    }
}
#endregion

#region 失败状态机
public class FailState : IFightState
{
    public FailState() {}
    
    public void Enter()
    {
        Debug.Log("进入失败状态");
        PlayerManager.Instance._PlayerData.ModifyHP(-1);
        DOVirtual.DelayedCall(3f, () => 
            { BattleManager.Instance.ShowWarReport(false); });
    }
    
    public void Update()
    {
        // 可在这里添加失败后需要持续处理的逻辑
    }
    
    public void Exit()
    {
        Debug.Log("退出失败状态");
    }
}
#endregion