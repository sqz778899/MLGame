public static class BuffFactory
{
    public static IBuffEffect Create(string id)
    {
        return id switch
        {
            "SecondBulletNerf" => new Buff_SecondBulletNerf(),
            "StrengthUp" => new Buff_StrengthUp(),
            _ => null,
        };
    }
}