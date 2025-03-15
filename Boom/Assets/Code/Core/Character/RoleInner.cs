using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class RoleInner : BaseMove
{
    [Header("SpineAbout")]
    public SkeletonAnimation AttackFX;
    [Header("Others")]
    public float FireDelay = 0.3f;
    public List<BulletInner> Bullets;
    public Connon CurConnon;
    public Transform ConnonNode;
    [Header("移动范围设置")]
    Bounds _mapBounds;
    float _cameraOffsetX;

    #region 初始化数据
    public void InitData(LevelMono CurLevel = null)
    {
        IsLocked = false;
        if (CurLevel != null)
            _mapBounds = CurLevel.MapCollider.bounds;
        _cameraOffsetX = _mCamera.transform.position.x - transform.position.x;
        //_cameraOffsetX = 2.5f;
        CreateBulletInner();
    }
    
    public void SetBulletPos()
    {
        foreach (var curBullet in Bullets)
        {
            curBullet.transform.position = new Vector3(
                transform.position.x - curBullet._data.CurSlot.SlotID,
                -0.64f,
                -0.15f
            );
        }
    }
    
    //在开始战斗的时候，根据角色槽位的子弹，创建五个跟着他跑的傻逼嘻嘻的小子弹
    public void CreateBulletInner()
    {
        //清空子弹
        if (Bullets != null)
        {
            Bullets.RemoveAll(bullet => bullet == null);
            if (Bullets.Count > 0)
            {
                for (int i = 0; i < Bullets.Count; i++)
                    Destroy(Bullets[i].gameObject);
            }
        }
        Bullets = new List<BulletInner>();
        //创建子弹
        Vector3 startPos = new Vector3(transform.position.x - 1, -0.64f, -0.15f);
        for (int i = 0; i < MainRoleManager.Instance.CurBullets.Count; i++)
        {
            BulletData curB = MainRoleManager.Instance.CurBullets[i];
            GameObject bulletIns = BulletFactory.CreateBullet(curB, BulletInsMode.Inner).gameObject;
            BulletInner curSC = bulletIns.GetComponent<BulletInner>();
            curSC.CurRole = this;
            float offsetX = startPos.x - (curB.CurSlot.SlotID - 1) * 1f;
            curSC.FollowDis = Mathf.Abs(curB.CurSlot.SlotID  * 1f);
            bulletIns.transform.position = new Vector3(offsetX,startPos.y,startPos.z + i);
            bulletIns.transform.SetParent(UIManager.Instance.Logic.MapManagerSC.MapBuleltRoot.transform,false);
            Bullets.Add(curSC);
        }
    }
    #endregion

    #region 角色移动
    internal override void Move(Vector3 direction)
    {
        if (UIManager.Instance.IsLockedClick) return; //UI锁
        if (IsLocked) return;
        //地图边缘限制角色移动。。。。。。。。。。。。。。
        Vector3 newPos = transform.position + direction * Speed * Time.deltaTime;
        newPos.x = Mathf.Clamp(newPos.x, _mapBounds.min.x, _mapBounds.max.x);
        transform.position = newPos;
        //地图边缘限制摄像机移动
        _mCamera.transform.position = new Vector3(newPos.x + _cameraOffsetX,_mCamera.transform.position.y
            ,_mCamera.transform.position.z);
        
        AniUtility.TrunAround(Ani,direction.x);//朝向
        AniUtility.PlayRun(Ani);    
    }
    #endregion

    #region 开火相关
    void CreateConnon(ref float AniTime)
    {
        GameObject ConnonIns = ResManager.instance.CreatInstance(PathConfig.ConnonPB);
        ConnonIns.transform.SetParent(ConnonNode,false);
        Connon curSC = ConnonIns.GetComponent<Connon>();
        
        Vector3 tmp = ConnonNode.transform.position;
        Vector3 targetPos = new Vector3(tmp.x,-1f,tmp.z);
        curSC.Appear(targetPos,ref AniTime);
        CurConnon = curSC;
    }
    
    public void Fire()
    {
        State = RoleState.Attack;
        IsLocked = true;
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
        //.................播放角色攻击动画.......................
        float anitime = 0f;
        AniUtility.PlayAttack(Ani,ref anitime); // 播放攻击动画
        yield return new WaitForSeconds(anitime);

        //.................创建大炮............................
        float connonAniTime = 0f;
        CreateConnon(ref connonAniTime);
        State = RoleState.Idle;
        yield return new WaitForSeconds(connonAniTime);
        StartCoroutine(FireWithDelay(FireDelay));
    }

    public IEnumerator FireWithDelay(float delay)
    {
        //填弹药动画
        float connonReloadTime = 0f;
        CurConnon.Reload(ref connonReloadTime);
        yield return new WaitForSeconds(connonReloadTime);  //大炮装填子弹动画
        //...
        //进行子弹装填
        for (int i = 0; i < Bullets.Count; i++)
        {
            BulletInner curBullet = Bullets[i];
            CurConnon.AllBullets.Add(Bullets[i]);//并且把弹药数据装填进大炮
            StartCoroutine(curBullet.ReadyToAttack(CurConnon.FillNode.transform.position));
            yield return new WaitForSeconds(delay);  // 在发射下一个子弹之前，等待delay秒
        }
        //播放大炮攻击动画
        for (int i = 0; i < Bullets.Count; i++)
        {
            //填弹药动画
            float connonAttackTime = 0f;
            CurConnon.Attack(i,ref connonAttackTime);
            yield return new WaitForSeconds(connonAttackTime);  // 在发射下一个子弹之前，等待delay秒
        }
        yield return new WaitForSeconds(2);
        Destroy(CurConnon.gameObject);
    }
    #endregion
}