using UnityEngine;
using System.Collections;

public class ShieldController:IDamageable
{
    ShieldData _data;
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
        if (!_data.IsDestroyed)
            _data.EState = EnemyState.live;
    }
    
    #region IDamageable接口相关的实现
    public DamageResult TakeDamage(BulletData source, int damage)
    {
        int overflow = Mathf.Max(0, damage - _data.CurHP);
        int effective = damage - overflow;

        _data.TakeDamage(damage);
        _view.ShowHitText(damage); //伤害跳字
        _data.EState = _data.IsDestroyed ? EnemyState.dead : EnemyState.hit;
        
        if (_hitRoutine != null)
            _coroutineHost.StopCoroutine(_hitRoutine);
        _hitRoutine = _coroutineHost.StartCoroutine(HitToIdle());

        bool isDestroyed = _data.IsDestroyed;
        /*if (isDestroyed)
            GameObject.Destroy(_view.gameObject);*/

        return new DamageResult(damage, effective, overflow, isDestroyed, _data.ShieldIndex);
    }

    public bool IsDead => _data.IsDestroyed;
    public int CurHP => _data.CurHP;
    public int MaxHP => _data.MaxHP;
    #endregion
}