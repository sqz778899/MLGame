using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IDamageable
{
    DamageResult TakeDamage(BulletData source, int damage);
    bool IsDead { get; }
    int CurHP { get; }
    int MaxHP { get; }
    Vector3 GetHitPosition();
}
public struct DamageResult
{
    public int TotalDamage;         // 实际造成的伤害
    public int EffectiveDamage;     // 有效命中（未溢出）部分
    public int OverflowDamage;      // 溢出伤害（超出当前 HP）
    public bool IsDestroyed;        // 是否击破（目标死亡）
    public int TargetIndex;         // 可选：敌人或盾牌的索引（如 ShieldIndex）

    public DamageResult(int total, int effective, int overflow, bool destroyed, int index = -1)
    {
        TotalDamage = total;
        EffectiveDamage = effective;
        OverflowDamage = overflow;
        IsDestroyed = destroyed;
        TargetIndex = index;
    }
}

#region 战报相关
public struct BattleOnceHit
{
    public int HitIndex;
    public int ShieldIndex;
    public int EnemyIndex;
    public int EffectiveDamage;
    public int OverflowDamage;
    public int Damage;
    public bool IsDestroyed;

    public BattleOnceHit(int hitIndex = -1, int shieldIndex = -1, int enemyIndex = -1,
        int effectiveDamage = 0, int overflowDamage = 0, int damage = 0, bool isDestroyed = false)
    {
        HitIndex = hitIndex;
        ShieldIndex = shieldIndex;
        EnemyIndex = enemyIndex;
        EffectiveDamage = effectiveDamage;
        OverflowDamage = overflowDamage;
        Damage = damage;
        IsDestroyed = isDestroyed;
    }
}

// 单个子弹在一场战斗中的全部表现
public class BulletAttackRecord
{
    public BulletData BulletData;                // 攻击时使用的子弹数据
    public int BulletSlotID;                     // 子弹槽位索引
    public List<BattleOnceHit> Hits;             // 每次Hit记录

    public BulletAttackRecord(BulletData bulletData, int bulletSlotID)
    {
        BulletData = bulletData;
        BulletSlotID = bulletSlotID;
        Hits = new List<BattleOnceHit>();
    }
    public void RecordHit(BattleOnceHit hit) => Hits.Add(hit);
}

// 单场战斗信息（一个战斗回合）
public class SingleBattleReport
{
    public List<BulletAttackRecord> BulletAttackRecords = new List<BulletAttackRecord>();

    // 快速查找当前子弹的记录（按SlotID）
    public BulletAttackRecord GetOrCreateBulletRecord(BulletData bulletData)
    {
        int slotID = bulletData.CurSlotController.SlotID;
        var record = BulletAttackRecords.FirstOrDefault(r => r.BulletSlotID == slotID);
        if (record == null)
        {
            record = new BulletAttackRecord(bulletData, slotID);
            BulletAttackRecords.Add(record);
        }
        return record;
    }
}

public class WarReport
{
    public int CurWarIndex;
    public bool IsWin;
    public int TotalDamage;
    public int EffectiveDamage;
    public int OverFlowDamage;
    public Dictionary<int, SingleBattleReport> WarIndexToBattleInfo;

    public WarReport()
    {
        CurWarIndex = 0;
        IsWin = false;
        WarIndexToBattleInfo = new Dictionary<int, SingleBattleReport>();
    }
    
    public SingleBattleReport GetCurBattleInfo()
    {
        if (!WarIndexToBattleInfo.TryGetValue(CurWarIndex, out var report))
        {
            report = new SingleBattleReport();
            WarIndexToBattleInfo[CurWarIndex] = report;
        }
        return report;
    }
}
#endregion