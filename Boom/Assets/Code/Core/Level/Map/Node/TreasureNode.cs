using UnityEngine;

public class TreasureNode:MapNodeBase
{
    public void OpenTreasureBox()
    {
        State = MapNodeState.IsFinish;
        ChangeState();
        Debug.Log("Open Tressure Box !!");
    }
}