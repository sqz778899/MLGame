

public class CommonAttribute
{
    public int damage;
    public int elementalType;
    public int elementalValue;
    public int Penetration;
}

public class SpeAttribute
{
    public int interest;//加减利息
    public int standbyAdd;//加减冷板凳位置
}

public class BuffDataJson
{
    public int ID;
    public string name;
    public int rare;
    public CommonAttribute comAttributes;
    public SpeAttribute speAttributes;
}