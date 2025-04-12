using System.Collections;
using UnityEngine;

public class EnemyController
{
    EnemyData _data;
    EnemyView _view;
    
    MonoBehaviour _coroutineHost; // 用于挂载协程
    Coroutine _hitRoutine;

    public void Bind(EnemyData data, EnemyView view)
    {
        _data = data;
        _view = view;

        _view.InitSpine(_data);
        _view.HealthBar.InitHealthBar(() => _data.CurHP, () => _data.MaxHP);
        _data.OnTakeDamage += OnTakeDamage;
    }
    
    void OnTakeDamage()=>_view.HealthBar.Refresh();

    public void Tick(float dt)
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

    public void ReceiveDamage(BulletData fromBullet, int damage)
    {
        if (_data.IsDead) return;

        _view.ShowHitText(damage);

        _data.TakeDamage(damage);
        _data.EState = EnemyState.hit;
        if (_hitRoutine != null)
            _coroutineHost.StopCoroutine(_hitRoutine);
        _hitRoutine = _coroutineHost.StartCoroutine(HitToIdle());
        
        // 战报记录等后续逻辑
        // WarReportManager.Instance.RecordHit(...);

        if (_data.IsDead)
            _data.EState = EnemyState.dead;
    }
    
    IEnumerator HitToIdle()
    {
        float hitTime = 0f;
        AniUtility.PlayHit01(_view.Ani, ref hitTime);
        yield return new WaitForSeconds(hitTime);
        if (!_data.IsDead)
            _data.EState = EnemyState.live;
    }
    
    public void Dispose() => _data.OnTakeDamage -= OnTakeDamage;
}