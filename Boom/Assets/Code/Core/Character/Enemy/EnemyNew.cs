using UnityEngine;

public class EnemyNew : MonoBehaviour,IDamageable
{
    public EnemyController Controller { get; private set; }
    public EnemyView View { get; private set; }

    void Awake()
    {
        View = GetComponent<EnemyView>();
        Controller = new EnemyController();
    }

    public void BindData(EnemyData data) => Controller.Bind(data, View,this);
    void Update() => Controller.Tick(Time.deltaTime);
    void OnDestroy() => Controller?.Dispose();
    
    //添加代理接口实现（转发给 Controller）
    public bool IsDead => Controller.IsDead;
    public int CurHP => Controller.CurHP;
    public int MaxHP => Controller.MaxHP;
    public Vector3 GetHitPosition() => Controller.GetHitPosition();
    public DamageResult TakeDamage(BulletData source, int damage) => Controller.TakeDamage(source, damage);
}