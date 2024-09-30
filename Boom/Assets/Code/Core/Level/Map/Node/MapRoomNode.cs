using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MapRoomNode : MonoBehaviour
{
    [Header("重要属性")]
    public MapRoomState State;
    public int RoomID;
    ArrowNode[] _arrows;
    MapNodeBase[] _mapNodes;
    void Start()
    {
        //获取地图节点
        _arrows = GetComponentsInChildren<ArrowNode>();
        _mapNodes = GetComponentsInChildren<MapNodeBase>();
        OnOffArrows();
    }

    void HideArrows()
    {
        foreach (var each in _arrows)
            each.gameObject.SetActive(false);
    }
    
    void ShowArrows()
    {
        foreach (var each in _arrows)
            each.gameObject.SetActive(true);
    }
    
    void OnOffArrows()
    {
        bool isAllFinish = true;
        foreach (var each in _mapNodes)
        {
            if (each.State != MapNodeState.IsFinish)
                isAllFinish = false;
        }
        if (isAllFinish)
            ShowArrows();
        else
            HideArrows();
    }
}
