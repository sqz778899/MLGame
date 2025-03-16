using UnityEngine;

public class LogicManager
{
    public MapManager MapManagerSC;
    
    public void InitData()
    {
        MapManagerSC = GameObject.Find("MapManager").GetComponent<MapManager>();
    }
}