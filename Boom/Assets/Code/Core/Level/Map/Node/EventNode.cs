using UnityEngine;

public class EventNode:MapNodeBase
{
    public int EventID;
    //Event
    public void RandomEvent()  
    {
        //LoadEvent()
        string curREventPath = PathConfig.GetREventPath(EventID);
        GameObject curREventPB = Instantiate(ResManager
            .instance.GetAssetCache<GameObject>(curREventPath));
        curREventPB.transform.SetParent(
            UIManager.Instance.REventRoot.transform,false);
        Debug.Log("Random Event");
    }
}