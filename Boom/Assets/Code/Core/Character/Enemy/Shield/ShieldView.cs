using UnityEngine;

public class ShieldView : MonoBehaviour
{
    public HealthBar HealthBar;
    public float InsStep;  //根据资源大小直接填在Prefab上
    public Color HitColor;
    public Transform HitTextPos;

    public void Init(ShieldData data) =>
        HealthBar.InitHealthBar(() => data.CurHP, () => data.MaxHP);
    public void ShowHitText(int damage) =>
        FloatingTextFactory.CreateWorldText($"-{damage}",HitTextPos.position,HitColor,18f);
}