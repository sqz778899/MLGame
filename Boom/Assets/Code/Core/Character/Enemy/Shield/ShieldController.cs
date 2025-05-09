using UnityEngine;
using System.Collections;
using System.Linq;

public class ShieldController
{
    public ShieldData _data { get; private set; }
    ShieldView _view;
    
    MonoBehaviour _coroutineHost; // 用于挂载协程
    Coroutine _hitRoutine;
    
    public void Bind(ShieldData data, ShieldView view,MonoBehaviour host)
    {
        _data = data;
        _view = view;
        _coroutineHost = host;
        
        _view.Init(data);
    }
    
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
    
    IEnumerator HitToIdle()
    {
        float hitTime = 0f;
        AniUtility.PlayHit(_view.Ani, ref hitTime);
        yield return new WaitForSeconds(hitTime);
        if (!_data.IsDead)
            _data.EState = EnemyState.live;
    }
    
    #region IDamageable接口相关的实现
    public DamageResult TakeDamage(BulletData source)
    {
        //计算完全交给Data
        DamageResult result = _data.TakeDamage(source);
        //表现相关
        _view.ShowHitText(source.FinalDamage); //伤害跳字
        if (_hitRoutine != null)
            _coroutineHost.StopCoroutine(_hitRoutine);
        _hitRoutine = _coroutineHost.StartCoroutine(HitToIdle());
        return  result;
    }
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

    public bool IsDead => _data.IsDead;
    public int CurHP => _data.CurHP;
    public int MaxHP => _data.MaxHP;
    #endregion
}