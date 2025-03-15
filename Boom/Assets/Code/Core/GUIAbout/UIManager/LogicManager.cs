using UnityEngine;

public class LogicManager
{
    public MapManager MapManagerSC;
    public BattleLogic BattleLogicSC;
    
    public void InitData()
    {
        MapManagerSC = GameObject.Find("MapManager").GetComponent<MapManager>();
        BattleLogicSC = GameObject.Find("BattleLogic").GetComponent<BattleLogic>();
    }
}