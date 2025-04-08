using UnityEngine;

public class Effect_CrazyHat: IItemEffect
{
    public void OnBulletFire(BulletData bullet)
    {
        Debug.Log("OnBulletFire");
    }

    public void OnRoomStart() {}
    public void OnChestRoomEnter() {}
    public void OnDamageCalculate(BulletData bullet) {}
}

public class Effect_LuckyBoots : IItemEffect
{
    public void OnRoomStart()
    {
        Debug.Log("OnRoomStart");
    }

    public void OnBulletFire(BulletData bullet) {}
    public void OnChestRoomEnter() {}
    public void OnDamageCalculate(BulletData bullet) {}
}