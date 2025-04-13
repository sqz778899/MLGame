using System.Collections.Generic;
using UnityEngine;

public class BulletInnerController 
{
    BulletInnerView _view;
    public BulletData Data{get; private set; }
    BulletInnerState _state;
    int _piercingCount;
    float _followDistance;
    float runSpeed = 10.0f;
    public float AttackSeed = 60f;
    
    public void BindView(BulletInnerView view) => _view = view;

    public void Init(BulletData data)
    {
        Data = data;
        _state = BulletInnerState.Idle;
        _piercingCount = 0;
        _followDistance = Mathf.Abs(Data.CurSlotController.SlotID);
        _view.BindDataView(data);
    }

    public void Tick(float deltaTime)
    {
        switch (_state)
        {
            case BulletInnerState.Idle:
                HandleFollowPlayer(deltaTime);
                break;
            case BulletInnerState.AttackBegin:
                HandleAttackAnimation(deltaTime);
                break;
            case BulletInnerState.Dead:
                _view.HandleDisappear();
                break;
        }
    }

    public void FixedTick(float fixedDeltaTime)
    {
        if (_state == BulletInnerState.Attacking)
            _view.AttackingFly(AttackSeed, fixedDeltaTime);
    }

    //追随角色
    void HandleFollowPlayer(float deltaTime)
    {
        Vector3 playerPos = PlayerManager.Instance.RoleInFightGO.transform.position;
        var dis = _view.GetHorizontalDistance(playerPos);

        if (Mathf.Abs(dis) > _followDistance)
        {
            _view.FaceDirection(dis < 0 ? Vector3.right : Vector3.left);
            _view.PlayRunAnimation();
            _view.TranslateHorizontally(-Mathf.Sign(dis) * runSpeed* deltaTime);
        }
        else
            _view.PlayIdleAnimation();
    }

    //处理攻击动画和相关逻辑（子弹前摇、发射状态切换）
    void HandleAttackAnimation(float deltaTime) {}

    //外部调用
    public void StartAttack(Vector3 targetPos)
    {
        _state = BulletInnerState.AttackBegin;
        _view.PlayAttackChargeAnimation(targetPos);
    }
    //外部调用
    public void Attacking()
    {
        _state = BulletInnerState.Attacking;
        _view.PlayAttackingLoopAnimation();
    }
    
    //子弹击中敌人之后
    public void HandleCollision(Collider2D collider)
    {
        if (_state != BulletInnerState.Attacking) return;
        
        if (collider.TryGetComponent<IDamageable>(out var target))
        {
            // 命中处理：伤害结算
            DamageResult result = target.TakeDamage(Data, Data.FinalDamage);
            _view.PlayHitEffect();

            // 战报记录
            RecordBattleHit(result, target);
            if (_piercingCount >= Data.FinalPiercing)
                _state = BulletInnerState.Dead;
            _piercingCount++;
            if (target is EnemyNew) //最后一个如果是敌人，则不再贯穿
                _state = BulletInnerState.Dead;
        }
    }
    
    void RecordBattleHit(DamageResult result, IDamageable target)
    {
        SingleBattleReport report = BattleManager.Instance.battleData.CurWarReport.GetCurBattleInfo();
        BulletAttackRecord record = report.GetOrCreateBulletRecord(Data);// 单个子弹在一场战斗中的全部表现
        var hit = new BattleOnceHit(
            record.Hits.Count,
            result.TargetIndex,
            (target is EnemyController) ? 1 : -1,  //1表示敌人，-1表示不是
            result.EffectiveDamage,
            result.OverflowDamage,
            result.TotalDamage,
            result.IsDestroyed
        );
        record.RecordHit(hit);
    }
}