using UnityEngine;

public class RoleInMap : BaseMove
{
    [Header("Others")]
    [SerializeField]
    MapRoomNode _curRoom;
    public MapRoomNode CurRoom
    {
        get { return _curRoom; }
        set
        {
            if (_curRoom == null || _curRoom != value)
            {
                InitRoomBounds();
                _curRoom = value;
            }
        }
    }

    //public MapRoomNode CurRoom;
    public Transform TextNode;
    
    [Header("移动范围设置")]
    Bounds _roomBounds;

    internal override void Start()
    {
        base.Start();
        InitRoomBounds();
    }

    void InitRoomBounds()
    {
        if (CurRoom != null)
        {
            Collider2D roomCollider = CurRoom.GetComponent<Collider2D>();
            _roomBounds = roomCollider.bounds;
        }
    }

    internal override void Move(Vector3 direction)
    {
        Vector3 newPosition = transform.position + direction * Speed * Time.deltaTime;
        // 限制角色在房间范围内
        newPosition.x = Mathf.Clamp(newPosition.x, _roomBounds.min.x, _roomBounds.max.x);
        transform.position = newPosition;
        // 限制摄像机跟随
        Vector3 cameraPosition = _mCamera.transform.position + direction * Speed * Time.deltaTime;
        cameraPosition.x = Mathf.Clamp(cameraPosition.x, _roomBounds.min.x, _roomBounds.max.x);
        _mCamera.transform.position = cameraPosition;
        
        AniUtility.TrunAround(Ani,direction.x);//朝向
        AniUtility.PlayRun(Ani);
    }
}
