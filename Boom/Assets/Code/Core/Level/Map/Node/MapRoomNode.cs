using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class MapRoomNode : MonoBehaviour
{
    [Header("重要属性")]
    public int RoomID;
    public MapRoomState State;
    MapRoomState preState;
    public Transform CameraStartPos;
    public Transform RoleStartPos;

    GameObject resRoot;
    GameObject _resRoot
    {
        get
        {
            if (resRoot == null)
                resRoot = transform.Cast<Transform>()
                    .FirstOrDefault(t => t.name == "ResRoot")?.gameObject;
            return resRoot;
        }
    }
    
    //Room节点下全部资产信息
    ArrowNode[] _arrows;     //全部的箭头
    MapNodeBase[] _resources; //全部的资源
    
    void Start()
    {
        //获取地图节点
        _arrows = GetComponentsInChildren<ArrowNode>();
        _resources = _resRoot.GetComponentsInChildren<MapNodeBase>();
    }

    void Update()
    {
        if (State == MapRoomState.IsFinish && preState != MapRoomState.IsFinish)
        {
            //解锁资源
            ShowArrows();
            for (int i = 0; i < _resources.Length; i++)
                _resources[i].IsLocked = false;
            preState = State;
        }
        else if (State != MapRoomState.IsFinish)
        {
            //锁定资源
            HideArrows();
            for (int i = 0; i < _resources.Length; i++)
                _resources[i].IsLocked = true;
            preState = State;
        }
    }

    #region 箭头相关
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
    #endregion
}
