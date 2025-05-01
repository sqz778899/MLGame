using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemEffectManager
{
    //特质
    List<IItemSynergies> _allTraits = new();
    List<IItemSynergies> _activeTraits = new();
    List<IItemSynergies> _cashTraits = new();
    //道具
    List<ItemData> equipItems => GM.Root.InventoryMgr._InventoryData.EquipItems;
    List<ItemData> _activeAllTimeItems = new();

    public void InitData()
    {
        RegisterAllComboTraits();
        ApplyAlltimesEffectsToBullets();
    }

    public void Trigger(ItemTriggerTiming timing,BattleContext ctx = null)
    {
        ctx ??= new BattleContext();
        //触发每一个道具的效果
        foreach (var item in equipItems)
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
            {
                trait.ApplyEffect(ctx);
            }
        }
    }
    
    public void TriggerCash(ItemTriggerTiming timing,BattleContext ctx = null)
    {
        ctx ??= new BattleContext();
        //触发每一个道具的Cash信息
        foreach (var item in equipItems)
        {
            if (item.EffectLogic?.TriggerCash == timing)
                item.ApplyEffectCash(ctx);
        }
    }
    
    public void ApplyAlltimesEffectsToBullets()
    {
        foreach (var each in _activeAllTimeItems)
            each.RemoveEffect();
        
        foreach (var item in equipItems)
        {
            if (item.EffectLogic?.TriggerTiming == ItemTriggerTiming.OnAlltimes)
            {
                _activeAllTimeItems.Add(item);
                item.ApplyEffect(new BattleContext());
            }
        }

        #region 处理Alltimes的特质
        // 清除之前的特质
        _cashTraits.ForEach(t=>t.RemoveEffect());
        _cashTraits.Clear();
        // 检测并缓存满足条件的组合特质
        RefreshComboTraits();
        // 触发当前时机的组合特质
        foreach (var trait in _activeTraits)
        {
            if (trait.TriggerTiming == ItemTriggerTiming.OnAlltimes)
            {
                _cashTraits.Add(trait);
                trait.ApplyEffect(null);
            }
        }
        #endregion
    }
    
    public bool ShouldRetryGambling()
    {
        foreach (var item in equipItems)
        {
            if (item.EffectLogic?.TriggerTiming == ItemTriggerTiming.OnAlltimes &&
                item.ID == 204) // Scavenger's Handbook
            {
                return UnityEngine.Random.value < 0.5f;
            }
        }
        return false;
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
        List<ItemData> equipped = GM.Root.InventoryMgr._InventoryData.EquipItems;
        if (equipped.Count > 3) return;
        
        _activeTraits.Clear();
        foreach (var trait in _allTraits)
        {
            if (trait.Match(equipped))
                _activeTraits.Add(trait);
        }
    }
    
    void RegisterAllComboTraits()
    {
        _allTraits.Clear();

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
                    _allTraits.Add(instance);
            }
            catch (Exception e)
            {
                Debug.LogError($"[ItemEffectManager] Failed to create trait: {type.Name} - {e.Message}");
            }
        }

        Debug.Log($"[ItemEffectManager] Registered {_allTraits.Count} combo traits.");
    }
    #endregion
}