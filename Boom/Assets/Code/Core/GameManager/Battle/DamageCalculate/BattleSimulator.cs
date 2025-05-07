using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BattleSimulator
{
    
    public static float SimulateBattle(int simulateCount = 100)
    {
        int winCount = 0;
        for (int i = 0; i < simulateCount; i++)
        {
            bool isWin = SimulateBattleSingle();
            if (isWin)
                winCount++;
        }
        return (float)winCount / simulateCount;
    }

    public static bool SimulateBattleSingle()
    {
        List<BulletData> bullets = GM.Root.InventoryMgr._BulletInvData.EquipBullets;
        EnemyData targetEnemyData = GM.Root.BattleMgr.battleData.CurEnemy.Data;
        
        
        // 复制敌人状态（深拷贝）
        EnemyData enemy = new EnemyData(targetEnemyData.ID,
            targetEnemyData.MaxHP, targetEnemyData.Shields);
        // 构造敌人目标列表（从前到后打）
        List<IDamageable> targets = new List<IDamageable>();
        for (int i = enemy.Shields.Count - 1; i >= 0; i--)
            targets.Add(enemy.Shields[i]);
        targets.Add(enemy); // 本体在最后
        
        int totalDamage = 0;
        int bulletUsed = 0;
        
        
        GM.Root.InventoryMgr.MiracleOddityMrg.Trigger(MiracleOddityTriggerTiming.OnBattleStart);
        GM.Root.InventoryMgr.MiracleOddityMrg.Trigger(MiracleOddityTriggerTiming.OnBulletFire);
        // 模拟每颗子弹
        foreach (BulletData bullet in bullets)
        {
            int piercingLeft = bullet.FinalPiercing;
            if (piercingLeft < 0) continue;

            bulletUsed++;
            
            for (int i = 0; i < targets.Count && piercingLeft >= 0; i++)
            {
                IDamageable target = targets[i];
                if (target.IsDead) continue;

                #region 触发 OnBulletHit 相关道具&&Buff
                BattleContext context = new BattleContext(bullet, target);
                GM.Root.InventoryMgr.MiracleOddityMrg.Trigger(MiracleOddityTriggerTiming.OnBulletHitBefore, context);
                #endregion

                if (context.ShieldSkipCount) continue; // 如果被标记跳过

                // 命中伤害计算
                DamageResult result = target.TakeDamage(bullet);
                totalDamage += result.TotalDamage;
                piercingLeft--;

                #region 触发 OnBulletHitAfter 的Cash 相关道具&&Buff
                BattleContext ctxCash = new BattleContext(bullet, target);
                GM.Root.InventoryMgr.MiracleOddityMrg.TriggerCash(MiracleOddityTriggerTiming.OnBulletHitAfter, ctxCash);
                #endregion

                // 击中敌人直接终止（规则约定）
                if (target is EnemyData && result.IsDestroyed)
                    break;
            }

            // 如果敌人已死，提前结束
            if (enemy.IsDead) break;
        }
        return enemy.IsDead;
    }
}
