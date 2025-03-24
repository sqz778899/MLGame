using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BagRootMini : MonoBehaviour
{
    public bool IsUnLockedItem = false;
    [Header("背包页签相关资源")]
    public GameObject BagBulletRootGO;
    public GameObject BagItemRootGO;
    public GameObject BagGemRootGO;
    
    [Header("背包页签按钮脚本")]
    public ButtonStateSwitch BtnBulletSC;
    public ButtonStateSwitch BtnItemSC;
    public ButtonStateSwitch BtnGemSC;

    [Header("背包其它资源")] 
    public GameObject GroupBulletSpawnerSlot;
    public CanvasGroup MiniSlotCanvasGroup;
    public GameObject DragObjRootGO;
    
    [Header("摄像机移动表现相关")]
    public Vector3 OriginCameraPos;
    public float orthographicSize;
    
    public Vector3 TargetCameraOffset;
    public float TargetOrthographicSize;
    bool IsCameraNear = false;
    void Start()
    {
        SwichBullet();
        TargetCameraOffset = new Vector3(-2.5f,-0.65f,0);
        TargetOrthographicSize = 3.35f;
    }
    
    public void InitData()
    {
        OriginCameraPos = Camera.main.transform.position;
        orthographicSize = Camera.main.orthographicSize;
    }
    
    //响应开始在战斗场景内拖拽的事件
    public void BulletDragged()
    {
        if(!IsCameraNear)
        SetCameraEdit();//拉近摄像机
        SetBulletInnerTextUp();//把子弹的伤害数字抬起来
    }
    
    //响应开始在战斗场景内推远摄像机的事件
    public void EditEnd()
    {
        if (IsCameraNear)
        {
            SetCameraBattle();//推远摄像机
            SetBulletInnerTextReturn();//把伤害数字位置还原
        }
    }

    
    //把伤害数字抬起来
    void SetBulletInnerTextUp()
    {
        RoleInner role = PlayerManager.Instance.RoleInFightGO.GetComponent<RoleInner>();
        role.Bullets.ForEach(b=>b.UpText());
    }
    //把伤害数字位置还原
    void SetBulletInnerTextReturn()
    {
        RoleInner role = PlayerManager.Instance.RoleInFightGO.GetComponent<RoleInner>();
        role.Bullets.ForEach(b=>b.ReturnText());
    }
    //拉近摄像机
    void SetCameraEdit()
    {
        float duration = 0.5f;
        IsCameraNear = true;
        InitData();
        Sequence seq = DOTween.Sequence();
        Vector3 targetCameraPos = OriginCameraPos + TargetCameraOffset;
        seq.Append(Camera.main.transform.DOMove(targetCameraPos, duration).SetEase(Ease.InOutQuad));
        seq.Join(DOTween.To(() => Camera.main.orthographicSize, 
            x => Camera.main.orthographicSize = x, 
            TargetOrthographicSize, duration).SetEase(Ease.InOutQuad));
        GameObject RoleGO = PlayerManager.Instance.RoleInMapGO;
        seq.Join(RoleGO.transform.DOMove(new Vector3(targetCameraPos.x,
            RoleGO.transform.position.y,RoleGO.transform.position.z), duration).SetEase(Ease.InOutQuad));
        
        MiniSlotCanvasGroup.alpha = 0; // 从透明状态开始
        seq.Join(MiniSlotCanvasGroup.DOFade(1, duration).SetEase(Ease.Linear));
        
        seq.onComplete = () => { RoleGO.GetComponent<RoleInner>().SetBulletPos(); };
    }
    
    //推远摄像机
    void SetCameraBattle()
    {
        IsCameraNear = false;
        Sequence seq = DOTween.Sequence();
        seq.Append(Camera.main.transform.DOMove(OriginCameraPos, 0.0f).SetEase(Ease.InOutQuad));
        seq.Join(DOTween.To(() => Camera.main.orthographicSize, 
            x => Camera.main.orthographicSize = x, 
            orthographicSize, 0.0f).SetEase(Ease.InOutQuad));
        
        seq.Join(MiniSlotCanvasGroup.DOFade(0, 0).SetEase(Ease.Linear));
    }

    #region Switch Tab
    //页签切换为Bullet
    public void SwichBullet()
    {
        BagBulletRootGO.SetActive(true);
        BagItemRootGO.SetActive(false);
        BagGemRootGO.SetActive(false);
        //页签选中状态
        BtnBulletSC.State = UILockedState.isSelected;
        BtnItemSC.State = IsUnLockedItem? UILockedState.isNormal : UILockedState.isLocked;
        BtnGemSC.State = UILockedState.isNormal;
    }

    //页签切换为Item
    public void SwichItem()   
    {
        if(!IsUnLockedItem) return;
        
        BagBulletRootGO.SetActive(false);
        BagItemRootGO.SetActive(true);
        BagGemRootGO.SetActive(false);
        //页签选中状态
        BtnBulletSC.State = UILockedState.isNormal;
        BtnItemSC.State = UILockedState.isSelected;
        BtnGemSC.State = UILockedState.isNormal;
    }
    
    //页签切换为Gem
    public void SwichGem()
    {
        BagBulletRootGO.SetActive(false);
        BagItemRootGO.SetActive(false);
        BagGemRootGO.SetActive(true);
        //页签选中状态
        BtnBulletSC.State = UILockedState.isNormal;
        BtnItemSC.State = IsUnLockedItem? UILockedState.isNormal : UILockedState.isLocked;
        BtnGemSC.State = UILockedState.isSelected;
    }
    #endregion
}
