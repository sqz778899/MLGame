public class DamageState
{
    public int Ice;
    public int Fire;
    public int Electric;

    public DamageState()
    {
        Ice = 0;
        Fire = 0;
        Electric = 0;
    }
}

public enum EnemyState
{
    live = 1,
    dead = 2
}