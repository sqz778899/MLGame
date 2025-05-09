using DG.Tweening;
using UnityEngine;

public class BattleCameraController
{
    Camera _mainCamera => Camera.main;
    BattleLogic _battleLogic => GM.Root.BattleMgr.battleLogic;
    
    Transform _firstBulletTrans => 
        GM.Root.InventoryMgr.CurBulletsInFight != null &&
        GM.Root.InventoryMgr.CurBulletsInFight.Count > 0 &&
        GM.Root.InventoryMgr.CurBulletsInFight[0] != null && 
        GM.Root.InventoryMgr.CurBulletsInFight[0].gameObject != null
            ? GM.Root.InventoryMgr.CurBulletsInFight[0].transform
            : null;
    
    bool isBeginCatch;
    float _cameraCurSpeed = 0f;
    
    public BattleCameraController()
    {
        _mainCamera.transform.position = new Vector3(2.5f,1,-10);
        _mainCamera.orthographicSize = 5;
        _battleLogic.IsBeginCameraMove = false;
        isBeginCatch = false;
        
        // 绑定事件
        BattleEventBus.OnBulletHit -= ShakeCamera;
        BattleEventBus.OnBulletHit += ShakeCamera;
    }
    
    void ShakeCamera()
    {
        //Debug.Log("ShakeCamera");
        Vector3 startPos = _mainCamera.transform.position;
        Vector3 endPos = new Vector3(startPos.x + 2f, startPos.y, startPos.z);
        // 创建一个序列
        Sequence cameraSequence = DOTween.Sequence();
        // 连续震动，每次间隔0.5s
        cameraSequence.Append(_mainCamera.transform.DOShakePosition(0.3f, strength: new Vector3(1f, 1.5f, 0f), vibrato: 20, randomness: 20));
        cameraSequence.Append(_mainCamera.transform.DOMove(endPos, 3f));
    }
  
    public void HandleCameraFollow()
    {
        if (!_battleLogic.IsBeginCameraMove) return;
        if (_firstBulletTrans == null) return; //第一颗子弹消失，则停止摄像机追踪
    
        Vector3 bulletViewportPos = _mainCamera.WorldToViewportPoint(_firstBulletTrans.position);
    
        // 如果子弹快飞出屏幕，镜头开始追
        if (bulletViewportPos.x > 0.9f || bulletViewportPos.x < 0.1f)
            isBeginCatch = true;

        if (isBeginCatch)
        {
            BulletInner firstBullet = GM.Root.InventoryMgr.CurBulletsInFight[0];
            float curSpeed = firstBullet.controller.AttackSeed; // 直接拿实时速度
            // 镜头的加速度
            _cameraCurSpeed = Mathf.Lerp(_cameraCurSpeed, Mathf.Max(curSpeed * 1.5f, curSpeed + 10f), Time.deltaTime * 14f);
            
            // 目标视口位置永远在屏幕中心 0.5f
            Vector3 targetViewPos = new Vector3(0.5f, 0.5f, 0);
            Vector3 targetWorldPos = _mainCamera.ViewportToWorldPoint(targetViewPos);
            Vector3 targetPos = _mainCamera.transform.position;
            
            targetPos.x = Mathf.MoveTowards(
                _mainCamera.transform.position.x,
                _firstBulletTrans.position.x - (targetWorldPos.x - _mainCamera.transform.position.x),
                _cameraCurSpeed * Time.deltaTime);

            _mainCamera.transform.position = targetPos;
        }
    }
}