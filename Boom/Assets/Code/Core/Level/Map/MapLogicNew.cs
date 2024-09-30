using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class MapLogicNew : MonoBehaviour
{
    public GameObject MapNodeRoot;
    [Header("地图内角色")]
    public GameObject Role;
    [Header("地图节点")]
    public List<GameObject> MapRooms;
    MapRoomNode[] _mapRooms;
    public GameObject CurMapRoom;
    
    void Start()
    {
        //获取地图节点
        MapRooms = new List<GameObject>();
        _mapRooms = MapNodeRoot.GetComponentsInChildren<MapRoomNode>();
        foreach (var each in _mapRooms)
            MapRooms.Add(each.gameObject);
        
        //设置角色位置
        SetRolePos();
    }
    
    public void SetRolePos()
    {
        //设置角色位置
        foreach (var each in _mapRooms)
        {
            if (each.RoomID == MainRoleManager.Instance.CurLevelID)
            {
                Debug.Log($"{each.gameObject.name}  {each.transform.position}");
                Role.transform.position = new Vector3(each.transform.position.x,
                    each.transform.position.y - 3.59f, Role.transform.position.z);
                CurMapRoom = each.gameObject;
                break;
            }
        }
        
        //设置摄像机位置
        Vector3 newCameraPos = new Vector3(Role.transform.position.x, Role.transform.position.y + 3.59f, Camera.main.transform.position.z);
        Camera.main.transform.DOMove(newCameraPos, 0.5f);
    }
}
