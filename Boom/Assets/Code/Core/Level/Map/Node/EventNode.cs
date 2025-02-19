using UnityEngine;

public class EventNode:MapNodeBase
{
    public MapEventType CurType;
    public int EventID;
    //Event
    public void EnterEvent()  
    {
        //LoadEvent()
        UIManager.Instance.IsLockedClick = false;
        string curREventPath = PathConfig.GetREventPath(EventID,CurType);
        GameObject REventIns =  ResManager.instance.CreatInstance(curREventPath);
        REventIns.transform.SetParent(UIManager.Instance.REventRoot.transform,false);
        REvent curREvent = REventIns.GetComponent<REvent>();
        curREvent.CurEventNode = this;
        Debug.Log("Random Event");
    }
}