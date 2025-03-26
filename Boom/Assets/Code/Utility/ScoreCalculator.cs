using System;
using UnityEngine;

public struct AllScoreStruct
{
    public int BaseScore;
    public int OverflowBonusScore;
    public int PerfectBonusScore;
    public int TotalScore;
    
    public AllScoreStruct(int baseScore, int overflowBonusScore, int perfectBonusScore, int totalScore)
    {
        BaseScore = baseScore;
        OverflowBonusScore = overflowBonusScore;
        PerfectBonusScore = perfectBonusScore;
        TotalScore = totalScore;
    }
}

public static class ScoreCalculator
{
    public static AllScoreStruct CalculateScore(int BaseScore)
    {
        AllScoreStruct allScoreStruct = new AllScoreStruct();
        allScoreStruct.BaseScore = BaseScore;
        
        int perfectBonusScore = 0;
        int totalScore = 0;

        int overFlowDamage = BattleManager.Instance.battleData.CurWarReport.OverFlowDamage;
        
        int overflowDamageBonus = PlayerManager.Instance._PlayerData.OverflowDamageBonus;
        int perfectDamageBonus = PlayerManager.Instance._PlayerData.PerfectDamageBonus;
        
        if (overFlowDamage > 0)
        {
            float temp = (float)overflowDamageBonus / 100;
            float overflowPercent = (float)Math.Round(temp, 3, MidpointRounding.AwayFromZero);
            allScoreStruct.OverflowBonusScore = (int)Math.Round(overflowPercent * BaseScore);
        }

        if (overFlowDamage == 0)
        {
            float temp = (float)perfectDamageBonus / 100;
            float perfectPercent = (float)Math.Round(temp, 3, MidpointRounding.AwayFromZero);
            allScoreStruct.PerfectBonusScore = (int)Math.Round(perfectPercent * BaseScore);
        }
        
        allScoreStruct.TotalScore = allScoreStruct.OverflowBonusScore 
                                    + allScoreStruct.PerfectBonusScore + BaseScore ;
        return allScoreStruct;
    }
    
    // 积分转魔尘
    public static int ScoreToDust(int score) => (int)Math.Round(score * PlayerManager.Instance._PlayerData.ScoreToDustRate);
    
    // 金币转魔尘
    public static int CoinToDust(int coin) => (int)Math.Round(coin * PlayerManager.Instance._PlayerData.CoinToDustRate);
}