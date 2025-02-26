using UnityEngine;
using Spine.Unity;

public class BaseMove : MonoBehaviour
{
    [Header("SpineAbout")]
    public SkeletonAnimation Ani;
    public RoleState State;
    public float Speed = 10.0f;

    internal Vector3 forward = new Vector3(1, 0, 0);
    internal Camera _mCamera;
    internal FightLogic _fightLogic;
    public bool IsLocked = false; //剧情教程等使用

    internal virtual void Awake()
    {
        // 直接缓存 Camera.main 和 FightLogic 组件
        _mCamera = Camera.main;
    }
    
    internal virtual void Update()
    {
        if (UIManager.Instance.IsLockedClick) return;
   
        if (Input.GetKey("d"))
        {
            State = RoleState.MoveForward;
        }
        else if (Input.GetKey("a") && _mCamera.WorldToViewportPoint(transform.position).x > 0)
        {
            State = RoleState.MoveBack;
        }
        else if (State == RoleState.Attack) {}
        else
        {
            State = RoleState.Idle;
        }

        switch (State)
        {
            case RoleState.Idle:
                AniUtility.PlayIdle(Ani);
                break;
            case RoleState.MoveForward:
                Move(forward);
                break;
            case RoleState.MoveBack:
                Move(-forward);
                break;
        }
    }

    internal virtual void Start()
    {
        // 延迟初始化 FightLogic 组件
        if (_fightLogic == null)
            _fightLogic = UIManager.Instance.FightLogicGO.GetComponent<FightLogic>();
    }

    internal virtual void Move(Vector3 direction) {}
}