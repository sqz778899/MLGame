public static class BuffFactory
{
    public static IBuffEffect Create(int id)
    {
        return id switch
        {
            1 => new Buff_ApplyBewilderHex(),
            2 => new Buff_LingeringManaTaint(),
            3 => new Buff_CurseExhaustion(),
            4 => new Buff_ArcaneOverload(),
            5 => new Buff_AeverseManaSurge(),
            6 => new Buff_IncantationKnot(),
            100 => new Buff_WeakenDefenses(),
            101 => new Buff_SpellLag(),
            201 => new Buff_DeflectedShot(),
            202 => new Buff_ResonanceBarrier(),
            _ => null,
        };
    }
}