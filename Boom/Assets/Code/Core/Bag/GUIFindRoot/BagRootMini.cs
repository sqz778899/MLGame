using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject DragObjRootGO;

    void Start()
    {
        SwichBullet();
    }


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
}
