using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IDamageable
{
    DamageResult TakeDamage(BulletData source, int damage);
    bool IsDead { get; }
    int CurHP { get; }
    int MaxHP { get; }
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
    public int FinalDamage; //子弹的实际伤害（加上各种BUFF的）
    public int FinalPiercing; //子弹的穿透（加上各种BUFF的）
    public int FinalResonance; //子弹的共振（加上各种BUFF的）
    public int HitIndex;
    public int ShieldIndex;
    public int EnemyIndex;
    public int EffectiveDamage;
    public int OverflowDamage;
    public int Damage;
    public bool IsDestroyed;

    public BattleOnceHit(int _finalDamage, int _finalPiercing, int _finalResonance,
        int hitIndex = -1, int shieldIndex = -1, int enemyIndex = -1,
        int effectiveDamage = 0, int overflowDamage = 0, int damage = 0, bool isDestroyed = false)
    {
        FinalDamage = _finalDamage;
        FinalPiercing = _finalPiercing;
        FinalResonance = _finalResonance;
        
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

    public BulletAttackRecord GetBulletRecord(BulletData bullet) =>
        BulletAttackRecords.FirstOrDefault(r => r.BulletData == bullet);
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

#region 战场Buff之类相关
public class BattleContext
{
    public List<BulletData> AllBullets;
    public EnemyData CurEnemy; // 当前敌人
    public BulletData CurBullet; // 当前命中的子弹
    public IDamageable TargetEnemy; // 当前命中的敌人
    //进入房间相关
    public int EnterRoomID;
    public bool IsFirstEnterRoom;
    // 是否跳过命中
    public bool ShieldSkipCount;
    // 是否击破护盾
    public bool IsShieldBreak;

    public BattleContext() => InitCommonData();
    
    public BattleContext(BulletData _curBullet,IDamageable _curTargetEnemy)
    {
        InitCommonData();
        CurBullet = _curBullet;
        TargetEnemy = _curTargetEnemy;
    }
    
    void InitCommonData()
    {
        AllBullets = GM.Root.InventoryMgr._BulletInvData.EquipBullets;
        if (GM.Root.BattleMgr.battleData.CurLevel == null)
            CurEnemy = null;
        else if (GM.Root.BattleMgr.battleData.CurLevel.CurEnemy == null)
            CurEnemy = null;
        else
            CurEnemy = GM.Root.BattleMgr.battleData.CurLevel.CurEnemy.Data;
        CurBullet = null;
        TargetEnemy = null;
        // 默认房间数据
        EnterRoomID = -1;
        IsFirstEnterRoom = false;
        ShieldSkipCount = false;
    }
}

public class BattleTempState
{
    // key: 唯一标识符（如 ItemID），value: 任意对象（每个道具自定义数据结构）
    private Dictionary<string, object> _data = new();

    /// <summary>
    /// 设置一个状态数据（同Key覆盖）
    /// </summary>
    public void Set<T>(string key, T value) where T : class
    {
        _data[key] = value;
    }

    /// <summary>
    /// 获取一个状态数据，如果没有则返回 null
    /// </summary>
    public T Get<T>(string key) where T : class
    {
        if (_data.TryGetValue(key, out var value))
            return value as T;
        return null;
    }

    public void Clear()
    {
        _data.Clear();
    }
}


#endregion