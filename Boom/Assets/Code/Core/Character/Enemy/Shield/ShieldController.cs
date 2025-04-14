using UnityEngine;

public class ShieldController:IDamageable
{
    ShieldData _data;
    ShieldView _view;

    public void Bind(ShieldData data, ShieldView view)
    {
        _data = data;
        _view = view;

        _view.Init(data);
    }
    
    #region IDamageable接口相关的实现
    public DamageResult TakeDamage(BulletData source, int damage)
    {
        int overflow = Mathf.Max(0, damage - _data.CurHP);
        int effective = damage - overflow;

        _data.TakeDamage(damage);
        _view.ShowHitText(damage); // 实际值根据需求调整

        bool isDestroyed = _data.IsDestroyed;
        if (isDestroyed)
            GameObject.Destroy(_view.gameObject);

        return new DamageResult(damage, effective, overflow, isDestroyed, _data.ShieldIndex);
    }

    public bool IsDead => _data.IsDestroyed;
    public int CurHP => _data.CurHP;
    public int MaxHP => _data.MaxHP;
    #endregion
}