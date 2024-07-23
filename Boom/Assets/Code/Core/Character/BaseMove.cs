using UnityEngine;

public class BaseMove : MonoBehaviour
{
    public float Speed = 10.0f;
    float CameraFollowingThreshold = -1;

    Vector3 forward = new Vector3(1, 0, 0);
    internal Camera _mCamera;
    internal FightLogic _fightLogic;

    internal virtual void Awake()
    {
        _mCamera = Camera.main;
    }
    internal virtual void Start()
    {
        if (_fightLogic==null)
            _fightLogic = UIManager.Instance.FightLogicGO.GetComponent<FightLogic>();
    }
    
    internal virtual void Update()
    {
        if (Input.GetKey("d"))
        {
            MoveForward();
        }
        else if (Input.GetKey("a") && _mCamera.WorldToViewportPoint(transform.position).x > 0)
        {
            MoveBack();
        }
    }

    internal virtual void MoveForward()
    {
        if (_mCamera.transform.position.x < transform.position.x + CameraFollowingThreshold)
        {
            _mCamera.transform.Translate( forward * Speed * Time.deltaTime);
        }
        transform.Translate( forward * Speed * Time.deltaTime);
    }

    internal virtual void MoveBack()
    {
        if (_mCamera.WorldToViewportPoint(transform.position).x > 0)
        {
            transform.Translate( -forward * Speed * Time.deltaTime);
        }
    }
}