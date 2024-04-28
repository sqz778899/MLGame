using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public float Distance = 1;
    SkeletonAnimation _ain;
    float _orginTimeScale;
    
    CurMoveState _state;
    CurMoveState _preState;
    
    bool IsMove = true;
    
    void Awake()
    {
        _ain = transform.GetChild(0).GetComponent<SkeletonAnimation>();
        _orginTimeScale = _ain.timeScale;
    }

    void Start()
    {
        Idle();
    }
    
    void Update()
    {
        CalState();
        if (IsMove)
        {
            _preState = _state;
            StartCoroutine(Move());
        }
    }
    

    float CurDistance()
    {
        return Math.Abs(transform.position.x - UIManager.Instance.CharILIns.transform.position.x);
    }


    void Idle()
    {
        if (_ain.AnimationName != "Idle")
        {
            _ain.AnimationName = "Idle";
            _ain.timeScale = _orginTimeScale;
        }
    }

    void Walk()
    {
        if (_ain.AnimationName != "Walk")
        {
            _ain.AnimationName = "Walk";
            _ain.timeScale = _orginTimeScale * 3;
        }    
    }

    void TrunAround(float face)
    {
        if (_preState != _state)
        {
            _preState = _state;
            _ain.skeleton.ScaleX = face;
        }
    }

    IEnumerator Move()
    {
        float temp = 0.3f;
        IsMove = false;
        yield return new WaitForSeconds(temp); 
        Vector3 targetPos = UIManager.Instance.CharILIns.transform.position;
        if (CurDistance() > Distance)
        {
            Walk();
            switch (_state)
            {
                case CurMoveState.Back:
                    TrunAround(1);
                    transform.DOMoveX(targetPos.x - 1,temp);
                    //transform.position = new Vector3(targetPos.x - 1, targetPos.y, targetPos.z);
                    break;
                case CurMoveState.Front:
                    TrunAround(-1);
                    transform.DOMoveX(targetPos.x + 1,temp);
                    //transform.position = new Vector3(targetPos.x + 1, targetPos.y, targetPos.z);
                    break;
            }
        }
        if (CurDistance() == Distance)
        {
            Idle();
        }
        if (CurDistance() < Distance)
        {
            Idle();
            switch (_state)
            {
                case CurMoveState.Front:
                    _ain.skeleton.ScaleX = -1;
                    break;
            }
        }
        IsMove = true;
    }
    
    void CalState()
    {
        float curDis = transform.position.x - UIManager.Instance.CharILIns.transform.position.x;
        if (curDis > 0)
            _state = CurMoveState.Front;
        else if (curDis == 0)
            _state = CurMoveState.Mid;
        else
            _state = CurMoveState.Back;
      
    }
    
    enum CurMoveState
    {
        Back,
        Mid,
        Front
    }
}
