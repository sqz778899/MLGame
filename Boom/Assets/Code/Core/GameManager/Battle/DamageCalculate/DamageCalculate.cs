using System.Collections.Generic;

public static class DamageCalculate
{
    public static void CalculateElementReaction(
        ElementReactionType reaction,ElementZoneData first,
        ElementZoneData second, ElementZoneData third)
    {
        
    }
}

public static class ElementReactionResolver
{
    public static ElementReactionType Resolve(ElementZoneData first,
        ElementZoneData second, ElementZoneData third)
    {
        if (first == null || second == null)
            return ElementReactionType.Non;

        //元素反应 冰火
        if (first.ElementalType == ElementalTypes.Ice &&
            second.ElementalType == ElementalTypes.Fire)
            return ElementReactionType.Explosion;
        if (first.ElementalType == ElementalTypes.Fire &&
            second.ElementalType == ElementalTypes.Ice)
            return ElementReactionType.Explosion;
        
        //元素反应 雷火
        if (first.ElementalType == ElementalTypes.Fire &&
            second.ElementalType == ElementalTypes.Thunder)
        {
            if (third == null || third.ElementalType != ElementalTypes.Thunder)
                return ElementReactionType.Overload;
            //元素反应 雷霆回响
            if (third.ElementalType == ElementalTypes.Thunder)
                return ElementReactionType.EchoingThunder;
        }
        if (first.ElementalType == ElementalTypes.Thunder &&
            second.ElementalType == ElementalTypes.Fire)
            return ElementReactionType.Overload;
        
        //元素反应 冰雷
        if (first.ElementalType == ElementalTypes.Ice &&
            second.ElementalType == ElementalTypes.Thunder)
        {
            if (third == null || third.ElementalType != ElementalTypes.Thunder)
                return ElementReactionType.Superconduct;
            //元素反应 雷涡
            if (third.ElementalType == ElementalTypes.Thunder)
                return ElementReactionType.Thunderburst;
        }
        if (first.ElementalType == ElementalTypes.Thunder &&
            second.ElementalType == ElementalTypes.Ice)
            return ElementReactionType.Superconduct;
        
        //元素反应 霜爆
        if (first.ElementalType == ElementalTypes.Ice &&
            second.ElementalType == ElementalTypes.Ice &&
            third != null && third.ElementalType == ElementalTypes.Fire)
            return ElementReactionType.CryoExplosion;
        
        //元素反应 坍缩
        if (first.ElementalType == ElementalTypes.Fire &&
            second.ElementalType == ElementalTypes.Fire &&
            third != null && third.ElementalType == ElementalTypes.Ice)
            return ElementReactionType.Collapse;
        
        //元素反应 迁跃
        if (first.ElementalType == ElementalTypes.Ice &&
            second.ElementalType == ElementalTypes.Ice &&
            third != null && third.ElementalType == ElementalTypes.Thunder)
            return ElementReactionType.Shift;
        
        //元素反应 流火
        if (first.ElementalType == ElementalTypes.Fire &&
            second.ElementalType == ElementalTypes.Fire &&
            third != null && third.ElementalType == ElementalTypes.Thunder)
            return ElementReactionType.BlazingTrail;
        
        return ElementReactionType.Non;
    }

    public static int GetReactionCount(ElementReactionType _reactionType)
    {
        switch (_reactionType)
        {
            case ElementReactionType.Explosion:
                return 2;
            case ElementReactionType.Overload:
                return 2;
            case ElementReactionType.Superconduct:
                return 2;
            case ElementReactionType.CryoExplosion:
                return 3;
            case ElementReactionType.Collapse:
                return 3;
            case ElementReactionType.Shift:
                return 3;
            case ElementReactionType.EchoingThunder:
                return 3;
            case ElementReactionType.Thunderburst:
                return 3;
            case ElementReactionType.BlazingTrail:
                return 3;
        }
        return 0;
    }
}