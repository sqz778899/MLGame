using UnityEngine;

public class ShieldNew : MonoBehaviour,IDamageable
{
    public ShieldController Controller { get; private set; }
    public ShieldView View { get; private set; }

    void Awake()
    {
        View = GetComponent<ShieldView>();
        Controller = new ShieldController();
    }

    public void BindData(ShieldData data) => Controller.Bind(data, View);
     
    // 代理接口实现
    public bool IsDead => Controller.IsDead;
    public int CurHP => Controller.CurHP;
    public int MaxHP => Controller.MaxHP;
    public DamageResult TakeDamage(BulletData source, int damage) => Controller.TakeDamage(source, damage);
}