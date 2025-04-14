public class ItemEffectManager
{
    public void Trigger(ItemTriggerTiming timing)
    {
        foreach (var item in GM.Root.InventoryMgr._InventoryData.EquipItems)
        {
            if (item.EffectLogic?.TriggerTiming == timing)
                item.ApplyEffect(GetNewBattleContext());
        }
    }
    
    BattleContext GetNewBattleContext() =>
        new BattleContext
        {
            AllBullets = GM.Root.InventoryMgr._BulletInvData.EquipBullets,
            CurEnemy = BattleManager.Instance.battleData.CurLevel.CurEnemy.Data,
            RoundIndex = BattleManager.Instance.battleData.CurWarReport.CurWarIndex,
            IsTreasureRoom = false,
        };
}