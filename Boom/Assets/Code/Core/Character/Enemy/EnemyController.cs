using System.Collections;
using UnityEngine;

public class EnemyController
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
    
    void OnTakeDamage() => _view.HealthBar.Refresh();

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
    public DamageResult TakeDamage(BulletData source)
    {
        //计算完全交给Data
        DamageResult result = _data.TakeDamage(source);
        
        //表现相关
        _view.ShowHitText(source.FinalDamage);//伤害跳字
        if (_hitRoutine != null)
            _coroutineHost.StopCoroutine(_hitRoutine);
        _hitRoutine = _coroutineHost.StartCoroutine(HitToIdle());

        return result;
    }
    
    //空实现。因为元素反应的伤害不在这里处理
    public DamageResult TakeReactionDamage(int damage)
    {
        //计算完全交给Data
        DamageResult result = _data.TakeReactionDamage(damage);
        //表现相关
        _view.ShowHitText(damage); //伤害跳字
        if (_hitRoutine != null)
            _coroutineHost.StopCoroutine(_hitRoutine);
        _hitRoutine = _coroutineHost.StartCoroutine(HitToIdle());
        return  result;
    }

    #endregion
}