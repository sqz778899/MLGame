using System.Collections.Generic;
using System.Linq;

public class BuffManager
{
    readonly List<IBuffEffect> activeBuffs = new();

    public void AddBuff(string buffId, int stack = 1)
    {
        var existing = activeBuffs.FirstOrDefault(b => b.BuffId == buffId);
        if (existing != null)
        {
            existing.Stack += stack;
        }
        else
        {
            var buff = BuffFactory.Create(buffId);
            if (buff != null)
            {
                buff.Stack = stack;
                activeBuffs.Add(buff);
            }
        }
    }

    public void Trigger(ItemTriggerTiming timing, BattleContext ctx)
    {
        foreach (var buff in activeBuffs)
        {
            if (buff.TriggerTiming == timing)
            {
                buff.Apply(ctx);
            }
        }
    }

    public IEnumerable<IBuffEffect> GetAllBuffs() => activeBuffs;

    public void Clear() => activeBuffs.Clear();
    
    public void OnBattleEnd()
    {
        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            var buff = activeBuffs[i];
            buff.RemainingBattles--;
            if (buff.IsExpired())
                activeBuffs.RemoveAt(i);
        }
    }
}