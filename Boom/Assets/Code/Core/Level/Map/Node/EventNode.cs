using UnityEngine;

public class EventNode:MapNodeBase
{
    public int EventID;
    //Event
    public void EnterEvent()  
    {
        //LoadEvent()
        string curREventPath = PathConfig.GetREventPath(EventID);
        GameObject REventIns =  ResManager.instance.CreatInstance<GameObject>(curREventPath);
        REventIns.transform.SetParent(UIManager.Instance.REventRoot.transform,false);
        REvent curREvent = REventIns.GetComponent<REvent>();
        curREvent.CurEventNode = this;
        Debug.Log("Random Event");
    }

    public void QuitEvent()
    {
        State = MapNodeState.IsFinish;
        ChangeState();
    }
}