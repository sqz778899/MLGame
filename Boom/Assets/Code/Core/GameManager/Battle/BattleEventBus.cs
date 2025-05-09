using System;

public static class BattleEventBus
{
    public static event Action OnFire;
    public static event Action OnBulletHit;
    public static event Action<ElementReactionType, int> OnElementReactionTriggered;
    public static event Action<IDamageable> OnTargetDestroyed;
    // 甚至包括
    public static event Action OnBattleStart;
    public static event Action OnBattleEnd;

    public static void BulletHit() => OnBulletHit?.Invoke();
    public static void StartOnFire() => OnFire?.Invoke();

    public static void ElementReactionTriggered(ElementReactionType type, int count) =>
        OnElementReactionTriggered?.Invoke(type, count);

    public static void TargetDestroyed(IDamageable target) =>
        OnTargetDestroyed?.Invoke(target);
}