using UnityEngine;

public class EnemyNew : MonoBehaviour
{
    public EnemyController Controller { get; private set; }
    public EnemyView View { get; private set; }

    void Awake()
    {
        View = GetComponent<EnemyView>();
        Controller = new EnemyController();
    }

    public void BindData(EnemyData data) => Controller.Bind(data, View);
    void Update() => Controller.Tick(Time.deltaTime);
    void OnDestroy() => Controller?.Dispose();
}