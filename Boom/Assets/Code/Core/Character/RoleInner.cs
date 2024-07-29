using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;

public class RoleInner : BaseMove
{
    [Header("SpineAbout")]
    public SkeletonAnimation Ani;
    public SkeletonAnimation AttackFX;
    [Header("Others")]
    public float FireDelay = 0.3f;
    public RoleState State;
    public List<BulletInner> Bullets;
    public Transform ConnonNode;

    #region 初始化数据
    public void InitData()
    {
        Bullets = new List<BulletInner>();
        CreateBulletInner();
    }
    
    //在开始战斗的时候，根据角色槽位的子弹，创建五个跟着他跑的傻逼嘻嘻的小子弹
    void CreateBulletInner()
    {
        Vector3 startPos = new Vector3(-1.5f, -0.64f, 1f);
        for (int i = 0; i < MainRoleManager.Instance.CurBullets.Count; i++)
        {
            BulletReady curB = MainRoleManager.Instance.CurBullets[i];
            GameObject bulletIns = BulletManager.Instance.
                InstanceBullet(curB.bulletID, BulletInsMode.Inner);
            BulletInner curSC = bulletIns.GetComponent<BulletInner>();
            float offsetX = startPos.x -i * 1.5f;
            curSC.FollowDis = Mathf.Abs(offsetX);
            bulletIns.transform.position = new Vector3(offsetX,startPos.y,startPos.z + i);
            bulletIns.transform.SetParent(transform.parent,false);
            Bullets.Add(curSC);
        }
    }
    #endregion

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

    void CreateConnon(ref float AniTime)
    {
        //CreateConnon();
        GameObject ConnonIns = ResManager.instance.IntanceAsset(PathConfig.ConnonPB);
        ConnonIns.transform.SetParent(ConnonNode,false);
        Connon curSC = ConnonIns.GetComponent<Connon>();
        AniTime = curSC.AppearanceTime;
        Vector3 tmp = ConnonNode.transform.position;
        Vector3 targetPos = new Vector3(tmp.x,0.1f,tmp.z);
        curSC.Appear(targetPos);
        //PlayConnonAni();
    }

    internal override void MoveForward()
    {
        base.MoveForward();
        AniUtility.TrunAround(Ani,1);
        AniUtility.PlayRun(Ani);
    }

    internal override void MoveBack()
    {
        base.MoveBack();
        AniUtility.TrunAround(Ani,-1);
        AniUtility.PlayRun(Ani);
    }

    public void Fire()
    {
        State = RoleState.Attack;
        StartCoroutine(FireIEnu());
        StartCoroutine(PlayAttackFX());
    }

    IEnumerator PlayAttackFX()
    {
        AttackFX.gameObject.SetActive(true);
        float fxtime = 0f;
        AniUtility.PlayAttack(AttackFX,ref fxtime);
        yield return new WaitForSeconds(fxtime);
        AttackFX.state.ClearTracks();
        AttackFX.skeleton.SetToSetupPose();
        AttackFX.gameObject.SetActive(false);
    }

    IEnumerator FireIEnu()
    {
        float anitime = 0f;
        AniUtility.PlayAttack(Ani,ref anitime); // 播放攻击动画
        yield return new WaitForSeconds(anitime);

        float connonAniTime = 0f;
        CreateConnon(ref connonAniTime);
        State = RoleState.Idle;
        yield return new WaitForSeconds(anitime);
        StartCoroutine(FireWithDelay(FireDelay));
    }

    public IEnumerator FireWithDelay(float delay)
    {
        Debug.Log("fire");
        for (int i = 0; i < Bullets.Count; i++)
        {
            BulletInner curBullet = Bullets[i];
            StartCoroutine(curBullet.Attack());
            yield return new WaitForSeconds(delay);  // 在发射下一个子弹之前，等待delay秒
        }
    }
}
