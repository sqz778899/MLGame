public interface IItemEffect
{
    void OnRoomStart();
    void OnBulletFire(BulletData bullet);
    void OnChestRoomEnter();
    void OnDamageCalculate(BulletData bullet);
}

public static class ItemEffectFactory
{
    public static IItemEffect CreateEffect(int itemId)
    {
        return itemId switch
        {
            1 => new Effect_CrazyHat(),
            7 => new Effect_LuckyBoots(),
            // ...
            401 => new Effect_RainbowBook(),
            _ => null,
        };
    }
}
