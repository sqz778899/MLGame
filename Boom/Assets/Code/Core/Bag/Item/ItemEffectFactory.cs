public interface IItemEffect
{
    ItemTriggerTiming TriggerTiming { get; }
    ItemTriggerTiming TriggerCash { get; }
    void ApplyCash(BattleContext ctx);
    void Apply(BattleContext ctx);
    void RemoveEffect();
    string GetDescription();
}


public static class ItemEffectFactory
{
    public static IItemEffect CreateEffectLogic(int itemId)
    {
        return itemId switch
        {
            1 => new Effect_ShabbyHat(),
            2 => new Effect_ScreamingClayJar(),
            3 => new Effect_MismatchedTacticalNotes(),
            4 => new Effect_DizzyOwlFigurine(),
            5 => new Effect_GoldenKey(),
            6 => new Effect_DraftyFlyingBroom(),
            7 => new Effect_LeftFootOfFortune(),
            8 => new Effect_TransmutationPracticeQuill(),
            9 => new Effect_CrackedToyWand(),
            10 => new Effect_MoldyTrainingLog(),
            //
            200 => new Effect_MisspelledSpellbook(),
            201 => new Effect_FlickeringCandle(),
            202 => new Effect_GrudgeTangledScarf(),
            203 => new Effect_SilentWatcherBlindfold(),
            204 => new Effect_ScavengerHandbook(),
            // ...
            300 => new Effect_StickyWizardGloves(),
            301 => new Effect_BloodhoundRing(),
            302 => new Effect_ResonantMagicChain(),
            //...
            401 => new Effect_IridescentGrimoire(),
            402 => new Effect_ArrogantKingCrown(),
            403 => new Effect_EchoingEdict(),
            _ => null,
        };
    }
}
