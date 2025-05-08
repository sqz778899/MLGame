using System;
using System.Collections.Generic;
using UnityEngine;

public class ElementZoneManager
{
    public event Action OnZoneChanged;
    public List<ElementZoneData> ActiveZones = new();
    List<BulletData> bullets => GM.Root.InventoryMgr._BulletInvData.EquipBullets;
    List<ElementReactionType> _predictReaction;
    int curReactionIndex = 0;

    //开火之后先预计算元素反应
    public void InitDataOnFire() => _predictReaction = PredictReactions();
    
    //子弹压入元素场域
    public void ApplyZone(BulletData bulletData)
    {
        if (bulletData.ElementalType == ElementalTypes.Non) return;
        ActiveZones.Add(new ElementZoneData(bulletData.ElementalType, bulletData.ElementalInfusionValue));
        OnZoneChanged?.Invoke();
    }

    public void TriggerReaction()
    {
        Debug.Log("TriggerReaction");
        if (curReactionIndex >= _predictReaction.Count) return;
        
        ElementZoneData first = ActiveZones.Count > 0 ? ActiveZones[0] : null;
        ElementZoneData second = (1 < ActiveZones.Count) ? ActiveZones[1] : null;
        ElementZoneData third = (2 < ActiveZones.Count) ? ActiveZones[2] : null;
        
        //没有第二个场域，不处理元素反应
        if (first == null || second == null) return;
        
        ElementReactionType reaction = ElementReactionResolver.Resolve(first, second, third);
        //没有元素反应
        if (reaction == ElementReactionType.Non) return;
        //和预测不一样，证明是需要后续元素的三元素反应，等下个子弹打过来，更新场域之后再触发
        ElementReactionType expected = _predictReaction[curReactionIndex];
        if (reaction != expected || reaction == ElementReactionType.Non) return;
        
        // 处理反应伤害
        DamageCalculate.CalculateElementReaction(reaction, first, second, third);
        // 消耗掉场域
        int consumeCount = ElementReactionResolver.GetReactionCount(reaction);
        for (int i = consumeCount - 1; i >= 0; i--)
            ActiveZones.RemoveAt(i);

        curReactionIndex++;
        OnZoneChanged?.Invoke();
    }
    
    //预测全部子弹的元素反应
    public List<ElementReactionType> PredictReactions()
    {
        List<ElementReactionType> results = new();
        int i = 0;

        while (i < bullets.Count)
        {
            BulletData first = bullets[i];
            BulletData second = (i + 1 < bullets.Count) ? bullets[i + 1] : null;
            BulletData third = (i + 2 < bullets.Count) ? bullets[i + 2] : null;

            if (second == null)
            {
                results.Add(ElementReactionType.Non); // 无法反应
                i++;
                continue;
            }

            ElementZoneData firstZone = new ElementZoneData(first.ElementalType, first.ElementalInfusionValue);
            ElementZoneData secondZone = new ElementZoneData(second.ElementalType, second.ElementalInfusionValue);
            ElementZoneData thirdZone = third != null
                ? new ElementZoneData(third.ElementalType, third.ElementalInfusionValue)
                : null;

            ElementReactionType reaction = ElementReactionResolver.Resolve(firstZone, secondZone, thirdZone);

            results.Add(reaction);

            // 根据反应类型判断消耗多少颗子弹
            int consumeCount = ElementReactionResolver.GetReactionCount(reaction);
            i += consumeCount > 0 ? consumeCount : 1;
        }

        return results;
    }

}

public class ElementReactionPrediction
{
    public int Index; // 第几颗子弹
    public ElementalTypes CurrentElement;
    public ElementReactionType? Reaction; // 是否触发反应
}


public enum ElementReactionType
{
    Non = 0,
    Explosion = 1,
    Overload = 2,
    Superconduct = 3,
    CryoExplosion = 4,
    Collapse = 5,
    Shift = 6,
    Thunderburst = 7,
    EchoingThunder = 8,
    BlazingTrail = 9
}