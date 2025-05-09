using UnityEngine;

public class Shield : MonoBehaviour,IDamageable
{
    public ShieldController Controller { get; private set; }
    public ShieldView View { get; private set; }

    void Awake()
    {
        View = GetComponent<ShieldView>();
        Controller = new ShieldController();
    }

    public void BindData(ShieldData data) => Controller.Bind(data, View,this);
    void Update() => Controller.Tick();
    
    // 代理接口实现
    public bool IsDead => Controller.IsDead;
    public int CurHP => Controller.CurHP;
    public int MaxHP => Controller.MaxHP;
    public DamageResult TakeDamage(BulletData source) => Controller.TakeDamage(source);
    public DamageResult TakeReactionDamage(int damage) => Controller.TakeReactionDamage(damage);
}