using UnityEngine;

public class Effect_RainbowBook : IItemEffect
{
    public void ModifyGemEffect(int baseValue, BulletData bullet)
    {
        Debug.Log("ModifyGemEffect");
    }

    // 其他方法默认空实现
    public void OnRoomStart() {}
    public void OnBulletFire(BulletData bullet) {}
    public void OnChestRoomEnter() {}
    public void OnDamageCalculate(BulletData bullet) {}
}