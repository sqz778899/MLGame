using System;
using UnityEngine;
using Spine.Unity;

public class BaseMove : MonoBehaviour
{
    [Header("SpineAbout")]
    public SkeletonAnimation Ani;
    
    BagRootMini _bagRootMini;
    public Action OnEditEndInner;
    RoleState _state;
    public RoleState State
    {
        get => _state;
        set
        {
            if (_state != value)
            {
                _state = value;
                if (_state == RoleState.Attack || 
                    _state == RoleState.MoveForward ||
                    _state == RoleState.MoveBack)
                {
                    OnEditEndInner?.Invoke();
                }
            }
        }
    }
    
    public float Speed = 10.0f;

    internal Vector3 forward = new Vector3(1, 0, 0);
    internal Camera _mCamera;
    internal BattleLogic BattleLogic;
    public bool IsLocked = false; //剧情教程等使用

    internal virtual void Awake()
    {
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
        if (BattleLogic == null)
            BattleLogic = BattleManager.Instance.battleLogic;
        _bagRootMini = UIManager.Instance.BagUI.BagRootMiniGO.GetComponent<BagRootMini>();
        
        OnEditEndInner += _bagRootMini.EditEnd;
    }

    internal virtual void Move(Vector3 direction) {}

    private void OnDestroy()
    {
        OnEditEndInner -= _bagRootMini.EditEnd;
    }
}