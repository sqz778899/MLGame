﻿using UnityEngine;

public class BaseMove : MonoBehaviour
{
    public float Speed = 10.0f;

    internal Vector3 forward = new Vector3(1, 0, 0);
    internal Camera _mCamera;
    internal FightLogic _fightLogic;
    
    [Header("移动范围设置")]
    public Collider2D roomCollider; 

    internal virtual void Awake()
    {
        // 直接缓存 Camera.main 和 FightLogic 组件
        _mCamera = Camera.main;
    }

    internal virtual void Start()
    {
        // 延迟初始化 FightLogic 组件
        if (_fightLogic == null)
            _fightLogic = UIManager.Instance.FightLogicGO.GetComponent<FightLogic>();
    }

    internal virtual void Update()
    {
        Vector3 direction = Vector3.zero;
        if (Input.GetKey("d")) direction = Vector3.right;
        else if (Input.GetKey("a")) direction = Vector3.left;
        else if (Input.GetKey("w")) direction = Vector3.up;
        else if (Input.GetKey("s")) direction = Vector3.down;

        if (direction != Vector3.zero)
        {
            Move(direction);
        }
        
        /*
        // 检测按键，并根据输入来移动
        if (Input.GetKey("d"))
        {
            Move(Vector3.forward);  // 向前移动
        }
        else if (Input.GetKey("a"))
        {
            // 只有当角色在屏幕内时才能向后移动
            if (_mCamera.WorldToViewportPoint(transform.position).x > 0)
            {
                Move(-Vector3.forward);  // 向后移动
            }
        }*/
    }

    internal virtual void Move(Vector3 direction)
    {
        transform.Translate(direction * Speed * Time.deltaTime);// 移动角色
        _mCamera.transform.Translate(direction * Speed * Time.deltaTime);// 移动摄像机
    }
}