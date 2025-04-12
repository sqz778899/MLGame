using UnityEngine;

public class BulletInnerNew : ItemBase
{
    public BulletInnerController controller { get; private set; }
    public BulletInnerView view{ get; private set; }

    void Awake()
    {
        view = GetComponent<BulletInnerView>();
        controller = new BulletInnerController();
        controller.BindView(view);
    }

    public override void BindData(ItemDataBase data) => controller.Init(data as BulletData);
    
    void Update() => controller.Tick(Time.deltaTime);

    void FixedUpdate() => controller.FixedTick(Time.fixedDeltaTime);

    void OnTriggerEnter2D(Collider2D other)=> controller.HandleCollision(other);
}