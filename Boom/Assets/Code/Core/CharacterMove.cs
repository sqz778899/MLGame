using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float Speed = 10.0f;
    public float CameraFollowingThreshold = 0;
    Vector3 forward = new Vector3(1, 0, 0);
    Camera _mCamera;
   
    void Awake()
    {
        _mCamera = Camera.main;
    }
    
    void Update()
    {
        Move();
    }

    void Move()
    {
        if(Input.GetKey("d"))
        {
            if (_mCamera.transform.position.x < transform.position.x + CameraFollowingThreshold)
            {
                _mCamera.transform.Translate( forward * Speed * Time.deltaTime);
            }
            transform.Translate( forward * Speed * Time.deltaTime);
        }
        if(Input.GetKey("a"))
        {
            transform.Translate( -forward * Speed * Time.deltaTime);
        }
    }
}
