using System;
using System.Collections.Generic;
using System.Linq;

public class ItemEffectManager
{
    //特质
    List<IItemSynergies> _comboTraits = new();
    List<IItemSynergies> _activeTraits = new();

    public void InitData() => RegisterAllComboTraits();
    
    public void Trigger(ItemTriggerTiming timing,BattleContext ctx = null)
    {
        ctx ??= new BattleContext();
        //触发每一个道具的效果
        foreach (var item in GM.Root.InventoryMgr._InventoryData.EquipItems)
        {
            if (item.EffectLogic?.TriggerTiming == timing)
                item.ApplyEffect(ctx);
        }
        
        // 检测并缓存满足条件的组合特质
        RefreshComboTraits();
        // 触发当前时机的组合特质
        foreach (var trait in _activeTraits)
        {
            if (trait.TriggerTiming == timing)
                trait.ApplyEffect(ctx);
        }
    }
    
    public List<TraitData> GetCurrentSynergiesInfos()
    {
        // 检测并缓存满足条件的组合特质
        RefreshComboTraits();
        var infos = new List<TraitData>();
        foreach (IItemSynergies combo in _activeTraits)
        {
            infos.Add(combo.Data);
        }
        return infos;
    }

    #region 不关心的方法
    //收集当前生效的特质
    void RefreshComboTraits()
    {
        _activeTraits.Clear();
        List<ItemData> equipped = GM.Root.InventoryMgr._InventoryData.EquipItems;

        foreach (var trait in _comboTraits)
        {
            if (trait.Match(equipped))
                _activeTraits.Add(trait);
        }
    }
    
    void RegisterAllComboTraits()
    {
        _comboTraits.Clear();

        var allTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IItemSynergies).IsAssignableFrom(type) &&
                           !type.IsInterface &&
                           !type.IsAbstract);

        foreach (var type in allTypes)
        {
            try
            {
                var instance = Activator.CreateInstance(type) as IItemSynergies;
                if (instance != null)
                    _comboTraits.Add(instance);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"[ItemEffectManager] Failed to create trait: {type.Name} - {e.Message}");
            }
        }

        UnityEngine.Debug.Log($"[ItemEffectManager] Registered {_comboTraits.Count} combo traits.");
    }
    #endregion
}