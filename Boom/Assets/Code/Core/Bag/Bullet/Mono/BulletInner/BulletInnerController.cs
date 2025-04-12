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

    // 处理攻击动画和相关逻辑（子弹前摇、发射状态切换）
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

    public void HandleCollision(Collider2D collider)
    {
        if (_state != BulletInnerState.Attacking) return;

        if (collider.CompareTag("Enemy") || collider.CompareTag("Shield"))
        {
            var enemy = collider.GetComponent<EnemyBase>();
            CalculateDamageManager.Instance.CalDamage(Data, enemy);
            _view.PlayHitEffect();

            _piercingCount++;
            if (_piercingCount >= Data.FinalPiercing)
                _state = BulletInnerState.Dead;
        }
    }
    
    // BulletInnerController 内的命中逻辑
    public void OnBulletHitEnemy(EnemyBase enemy,BulletData bulletData, int enemyIndex, int damage, int shieldIndex, bool isDestroyed)
    {
        SingleBattleReport battleReport = BattleManager.Instance.battleData.CurWarReport.GetCurBattleInfo();

        var bulletRecord = battleReport.GetOrCreateBulletRecord(bulletData);

        int overflowDamage = Mathf.Max(0, damage - enemy.CurHP);
        int effectiveDamage = damage - overflowDamage;

        BattleOnceHit hit = new BattleOnceHit(
            hitIndex: bulletRecord.Hits.Count,
            shieldIndex: shieldIndex,
            enemyIndex: enemyIndex,
            effectiveDamage: effectiveDamage,
            overflowDamage: overflowDamage,
            damage: damage,
            isDestroyed: isDestroyed
        );

        bulletRecord.RecordHit(hit);
    }
}