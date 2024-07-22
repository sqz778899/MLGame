using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;

public class RoleMove : BaseMove
{
    [Header("SpineAbout")]
    public SkeletonAnimation Ani;
    public RoleState State;

    internal override void Update()
    {
        if (Input.GetKey("d"))
        {
            State = RoleState.MoveForward;
        }
        else if (Input.GetKey("a") &&
                 _mCamera.WorldToViewportPoint(transform.position).x > 0)
        {
            State = RoleState.MoveBack;
        }
        else if (State == RoleState.Attack)
        {
            
        }
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
                MoveForward();
                break;
            case RoleState.MoveBack:
                MoveBack();
                break;
        }
    }

    internal override void MoveForward()
    {
        base.MoveForward();
        AniUtility.PlayRun(Ani);
    }

    internal override void MoveBack()
    {
        base.MoveBack();
        AniUtility.PlayRun(Ani);
    }

    public void Fire()
    {
        State = RoleState.Attack;
        StartCoroutine(FireIEnu());
    }

    IEnumerator FireIEnu()
    {
        float anitime = 0f;
        AniUtility.PlayAttack(Ani,ref anitime);
        Debug.Log("anitime:" + anitime);
        yield return new WaitForSeconds(anitime);
        State = RoleState.Idle;
    }
}
