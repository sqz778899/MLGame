using UnityEngine;

public class MapNode:MonoBehaviour
{
    public MapNodeView view;

    public void Init(MapNodeData data)
    {
        view = GetComponent<MapNodeView>();
        view.Init(data);
    }
}