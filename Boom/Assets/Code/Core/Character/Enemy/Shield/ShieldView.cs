using Spine.Unity;
using UnityEngine;

public class ShieldView : MonoBehaviour
{
    [Header("Shield资产")]
    public HealthBar HealthBar;
    public SkeletonAnimation Ani;
    
    public float InsStep;  //根据资源大小直接填在Prefab上
    public Color HitColor;
    public Transform HitTextPos;

    public void Init(ShieldData data) =>
        HealthBar.InitHealthBar(() => data.CurHP, () => data.MaxHP);
    public void ShowHitText(int damage) =>
        FloatingTextFactory.CreateWorldText($"-{damage}",
            HitTextPos.position + Vector3.up*0.5f,FloatingTextType.Damage,HitColor,10f);
    
    public void PlayIdle(bool isFullHP)
    {
        if (isFullHP)
            AniUtility.PlayIdle(Ani);
        else
            AniUtility.PlayIdle(Ani);
    }
    
    public void PlayDead()
    {
        AniUtility.PlayBroken(Ani);
        //禁用所有 Collider2D（包括子物体）
        foreach (var col in GetComponentsInChildren<Collider2D>())
            col.enabled = false;
    }
}