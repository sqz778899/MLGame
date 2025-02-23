using System.Linq;
using DG.Tweening;
using UnityEngine;

public class MapLogic : MonoBehaviour
{
    public GameObject MapNodeRoot;
    [Header("地图内角色")]
    public GameObject Role;
    [Header("对话系统脚本")]
    public Dialogue CurDialogue;
    [Header("地图节点")]
    MapRoomNode[] _allMapRooms;
    
    void Awake()
    {
        MainRoleManager.Instance.MainRoleIns = Role;
        MainRoleManager.Instance.CurMapLogic = this;
        InitMapData();
    }
    
    void Update()
    {
        
        if (MainRoleManager.Instance.CurMapLogic == null)
        {
            Debug.LogError($"MainRoleManager.Instance.CurMapLogic 是 null");
        }
    }

    public void InitMapData()
    {
        //获取地图房间节点
        _allMapRooms = MapNodeRoot.GetComponentsInChildren<MapRoomNode>();
        //设置角色位置
        SetRolePos();
    }
    
    public void SetRolePos()
    {
        //找到当前房间的节点
        MapRoomNode curRoom = _allMapRooms.FirstOrDefault(
            each => each.RoomID == MainRoleManager.Instance.CurMapSate.CurRoomID);
        curRoom.State = MapRoomState.Unlocked;
        
        //设置角色&&摄像机位置
        Role.GetComponent<RoleInMap>().CurRoom = curRoom;
        //Role.GetComponent<RoleInMap>().InitRoomBounds();
        Role.transform.position = curRoom.RoleStartPos.position;
        Vector3 newCameraPos = new Vector3(curRoom.CameraStartPos.position.x,
            curRoom.CameraStartPos.position.y, Camera.main.transform.position.z);
        Camera.main.transform.DOMove(newCameraPos, 0.5f);
    }

    public void SetAllIDs()
    {
        for (int i = 0; i < _allMapRooms.Length; i++)
            _allMapRooms[i].RoomID = i + 1;
    }
}
