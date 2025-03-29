public interface ITalentUnlockStrategy
{
    void Learn(TalentJson talent);
}

public enum TalentEffectType
{
    UnlockBulletSlot = 0,
    CarryGem = 1,
    CarryBullet = 2,
    CarryRoomKey = 3,
    GemBonus = 4,
    ResBonus = 5,
    BulletResBonus = 6,
    UnlockGem = 7,
    CarryGemLevelBonus = 8,
    ScoreToDustBonus = 9,
    CoinToDustBonus = 10,
}

public class TalentGemBonus
{
    public int GemID;
    public int BonusValue;

    public TalentGemBonus(int _id, int _value)
    {
        GemID = _id;
        BonusValue = _value;
    }
}