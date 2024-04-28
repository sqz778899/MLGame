using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float Speed = 10.0f;
    public float CameraFollowingThreshold = 0;

    public float timeScale;
    public GameObject test01;
    
    Vector3 forward = new Vector3(1, 0, 0);
    Camera _mCamera;
    
    LevelLogicMono LevelLogic;
   
    void Awake()
    {
        _mCamera = Camera.main;
        LevelLogic =  GameObject.Find("LevelLogic").GetComponent<LevelLogicMono>();
    }
    
    void Update()
    {
        if (Input.GetKey("d"))
        {
            MoveForward();
        }
        else if (Input.GetKey("a") && _mCamera.WorldToViewportPoint(transform.position).x > 0)
        {
            MoveBack();
        }
       
        //快捷键响应
        LevelLogic.CheckForKeyPress(transform.position);
    }



    void MoveForward()
    {
        if (_mCamera.transform.position.x < transform.position.x + CameraFollowingThreshold)
        {
            _mCamera.transform.Translate( forward * Speed * Time.deltaTime);
        }
        transform.Translate( forward * Speed * Time.deltaTime);
    }

    void MoveBack()
    {
        if (_mCamera.WorldToViewportPoint(transform.position).x > 0)
        {
            transform.Translate( -forward * Speed * Time.deltaTime);
        }
    }

    public void Fire()
    {
        LevelLogic.Fire(transform.position);
    }
}
