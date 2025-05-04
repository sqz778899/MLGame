using System.Collections;
using UnityEngine;

public class EnemyController:IDamageable
{
    public EnemyData _data { get; private set; }
    EnemyView _view;
    
    MonoBehaviour _coroutineHost; // 用于挂载协程
    Coroutine _hitRoutine;

    public void Bind(EnemyData data, EnemyView view,MonoBehaviour host)
    {
        _data = data;
        _view = view;
        _coroutineHost = host;
        
        _view.InitSpine(_data);
        _view.HealthBar.InitHealthBar(() => _data.CurHP, () => _data.MaxHP);
        _data.OnTakeDamage += OnTakeDamage;
    }
    
    void OnTakeDamage()=>_view.HealthBar.Refresh();

    public void Tick()
    {
        switch (_data.EState)
        {
            case EnemyState.live:
                _view.PlayIdle(_data.CurHP == _data.MaxHP);
                break;
            case EnemyState.hit:
                // hit 状态期间不做 Idle 刷新，动画交由协程控制
                break;
            case EnemyState.dead:
                _view.PlayDead();
                break;
        }
    }

    public EnemyState GetEState() => _data.EState;
    public Award GetAward() => _data.CurAward;
    public void SetEState(EnemyState EState) => _data.EState = EState;
    
    IEnumerator HitToIdle()
    {
        float hitTime = 0f;
        AniUtility.PlayHit01(_view.Skeleton, ref hitTime);
        yield return new WaitForSeconds(hitTime);
        if (!_data.IsDead)
            _data.EState = EnemyState.live;
    }
    
    public void Dispose() => _data.OnTakeDamage -= OnTakeDamage;

    #region IDamageable接口相关的实现
    public bool IsDead => _data.IsDead;
    public int CurHP => _data.CurHP;
    public int MaxHP => _data.MaxHP;
    public Vector3 GetHitPosition() => _view.HitTextPos.position;
    public DamageResult TakeDamage(BulletData source, int damage)
    {
        if (_data.IsDead)
            return new DamageResult(0, 0, 0, true, -1);
        
        int overflow = Mathf.Max(0, damage - _data.CurHP);
        int effective = damage - overflow;

        _data.TakeDamage(damage);
        _view.ShowHitText(damage);//伤害跳字
        _data.EState = _data.IsDead ? EnemyState.dead : EnemyState.hit;

        if (_hitRoutine != null)
            _coroutineHost.StopCoroutine(_hitRoutine);
        _hitRoutine = _coroutineHost.StartCoroutine(HitToIdle());

        return new DamageResult(damage, effective, overflow, _data.IsDead, /* target index */ -1);
    }
    #endregion
}