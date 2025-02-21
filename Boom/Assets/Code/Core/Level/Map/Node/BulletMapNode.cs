using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Spine.Unity;
using Unity.VisualScripting;

public class BulletMapNode : MapNodeBase
{
    public int BulletID;
    public string DialogueName;
    SkeletonAnimation _ain;
    Renderer _renderer;
    
    void Awake()
    {
        _ain = transform.GetChild(0).GetComponent<SkeletonAnimation>();
        AniUtility.PlayIdle(_ain);
        _renderer = transform.GetChild(0).GetComponent<Renderer>();
        SpineQuitHighLight();
    }

    internal override void Start() {}
    
    internal override void OnMouseEnter()
    {
        if (UIManager.Instance.IsLockedClick) return;
        SpineHighLight();
    }

    internal override void OnMouseExit()
    {
        if (UIManager.Instance.IsLockedClick) return;
        SpineQuitHighLight();
    }

    public void JoinYou()
    {
        SpineQuitHighLight();
        MMapLogic.CurDialogue.LoadDialogue(DialogueName);
        MMapLogic.CurDialogue.OnDialogueEnd += OnDiaCallBack;
    }

    void OnDiaCallBack()
    {
        MainRoleManager.Instance.AddSpawner(BulletID);
        BulletJson bulletDesignJson = TrunkManager.Instance.BulletDesignJsons
            .FirstOrDefault(b => b.ID == BulletID) ?? new BulletJson();
        FloatingGetItemText(bulletDesignJson.Name);
        Destroy(gameObject);
    }

    #region 一些私有方法
    void SpineHighLight()
    {
        uint layerToAdd = 1u << 1;
        _renderer.renderingLayerMask |= layerToAdd;
    }
    
    public void SpineQuitHighLight()
    {
        uint layerToRemove = 1u << 1;
        _renderer.renderingLayerMask &= ~layerToRemove;
    }
    #endregion
}
